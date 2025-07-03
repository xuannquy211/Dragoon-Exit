using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private EnvironmentController environmentPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private Transform destinationPrefab;
    [SerializeField] private float environmentWidth = 80f;
    [SerializeField] private Vector3 centerOffset;
    [SerializeField] private int _totalPreviewMap = 2;

    private readonly List<EnvironmentController> _environments = new List<EnvironmentController>();
    private bool _isHavingAbnormality = false;
    private Transform _destination;

    public static List<int> AbnormalitiesSeen = new List<int>();

    private int CurrentWaveIndex
    {
        get => UserData.CurrentWaveIndex;
        set => UserData.CurrentWaveIndex = value;
    }
    
    public static EnvironmentManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        FirstInitEnvironment();
        if (!UserData.IsFirstTime) RandomAbnormality();
        else
        {
            _environments[0].ClearAbnormalities();
            _environments[1].ClearAbnormalities();
            _environments[2].ClearAbnormalities();
        }

        UserData.SessionCount++;
    }

    private void FirstInitEnvironment()
    {
        var abnormalityUsed = UserData.GetAbnormalitiesUsed();
        _environments.Clear();

        var centerEnvironment = Instantiate(environmentPrefab, transform);
        centerEnvironment.InitAbnormality(abnormalityUsed);
        _environments.Add(centerEnvironment);
        centerEnvironment.gameObject.name = "CenterEnvironment";

        var backRotation = new Quaternion(0f, 1f, 0f, 0f);
        var backPosition = centerEnvironment.BackEnvironmentTarget.position;

        var backEnvironment = Instantiate(environmentPrefab, backPosition, backRotation, transform);
        
        backEnvironment.InitAbnormality(abnormalityUsed);
        _environments.Insert(0, backEnvironment);
        backEnvironment.gameObject.name = "BackEnvironment";

        var nextPosition = centerEnvironment.NextEnvironmentTarget.position;
        var nextEnvironment = Instantiate(environmentPrefab, nextPosition, Quaternion.identity, transform);
        nextEnvironment.InitAbnormality(abnormalityUsed);
        _environments.Add(nextEnvironment);
        nextEnvironment.gameObject.name = "NextEnvironment";
        
        player.SetParent(centerEnvironment.transform);
    }


    private void ShiftToNext()
    {
        var oldCenter = _environments[1];
        _environments.RemoveAt(1);
        var oldBack = _environments[0];
        _environments.RemoveAt(0);

        var currentCenter = _environments[0];
        player.SetParent(currentCenter.transform);
        currentCenter.transform.position = Vector3.zero;
        currentCenter.transform.rotation = Quaternion.identity;

        var nextEuler = currentCenter.transform.eulerAngles;
        var backEuler = Mathf.Abs(currentCenter.transform.rotation.y - 1);
        oldBack.transform.eulerAngles = nextEuler;
        oldBack.transform.position = currentCenter.NextEnvironmentTarget.position;
        oldCenter.transform.rotation = new Quaternion(0f, backEuler, 0f, 0f);
        oldCenter.transform.position = currentCenter.BackEnvironmentTarget.position;

        _environments.Insert(0, oldCenter);
        _environments.Add(oldBack);

        ActiveEnvironment();
        UpdateHolderToCenter();
        GameSignal.MOVE_TO_ENVIRONMENT.Notify(_environments[1]);

        UpdateEnvironmentNames();
    }

    private void ShiftToBack()
    {
        var oldNext = _environments[2];
        _environments.RemoveAt(2);
        var oldCenter = _environments[1];
        _environments.RemoveAt(1);

        var currentCenter = _environments[0];
        player.SetParent(currentCenter.transform);
        currentCenter.transform.position = Vector3.zero;
        currentCenter.transform.rotation = Quaternion.identity;
        
        var nextEuler = currentCenter.transform.eulerAngles;
        var backEuler = Mathf.Abs(currentCenter.transform.rotation.y - 1);
        oldNext.transform.eulerAngles = nextEuler;
        oldNext.transform.position = currentCenter.NextEnvironmentTarget.position;
        oldCenter.transform.rotation = new Quaternion(0f, backEuler, 0f, 0f);
        oldCenter.transform.position = currentCenter.BackEnvironmentTarget.position;

        _environments.Insert(0, oldCenter);
        _environments.Add(oldNext);

        ActiveEnvironment();
        UpdateHolderToCenter();
        GameSignal.MOVE_TO_ENVIRONMENT.Notify(_environments[1]);

        UpdateEnvironmentNames();
    }

    private void ActiveEnvironment()
    {
        var totalEnvironments = _environments.Count;
        for (var i = 0; i < totalEnvironments; i++)
        {
            _environments[i].gameObject.SetActive(true);
            _environments[i].ReInit();
        }
    }

    private void UpdateHolderToCenter()
    {
        var centerPosition = _environments[1].transform.localPosition;
        var holderPosition = centerPosition * -1f;

        transform.position = holderPosition;
    }


    private void UpdateEnvironmentNames()
    {
        _environments[0].gameObject.name = "BackEnvironment";
        _environments[1].gameObject.name = "CenterEnvironment";
        _environments[2].gameObject.name = "NextEnvironment";
    }

    private EnvironmentController GetCenterEnvironment()
    {
        return _environments[1];
    }

    private void FixedUpdate()
    {
        var centerEnvironment = GetCenterEnvironment();
        var offset =  1f - 2f * centerEnvironment.transform.rotation.y;
        var centerEnvironmentPosition = centerEnvironment.transform.position + centerOffset * offset;
        if (Mathf.Abs(player.position.x - centerEnvironmentPosition.x) < environmentWidth) return;
        if (Mathf.Abs(player.position.z - centerEnvironmentPosition.z) > environmentWidth / 2f) return;

        Debug.Log($"{Mathf.Abs(player.position.y - centerEnvironmentPosition.y)}, {environmentWidth / 2f}");
        
        var centerEnvironmentForward = centerEnvironment.transform.right;
        var direction = Vector3.Normalize(player.position - centerEnvironmentPosition);
        var dot = Vector3.Dot(centerEnvironmentForward, direction);

        if (dot < 0)
        {
            if (_isHavingAbnormality) OnTrueWay();
            else OnWrongWay();
            if (CurrentWaveIndex > Configs.TARGET_WAVE) return;
            ShiftToBack();
        }
        else
        {
            if (_isHavingAbnormality) OnWrongWay();
            else OnTrueWay();
            if (CurrentWaveIndex > Configs.TARGET_WAVE) return;
            ShiftToNext();
        }

        if (UserData.IsFirstTime)
        {
            _totalPreviewMap--;
            if (_totalPreviewMap > 0)
            {
                Debug.Log("TotalPreviewMap: " + _totalPreviewMap);
                return;
            }
            UserData.IsFirstTime = false;
        }
        else RandomAbnormality();

        if (CurrentWaveIndex != Configs.TARGET_WAVE)
        {
            if (_destination) _destination.gameObject.SetActive(false);
            return;
        }

        ActiveDestination();
    }

    private void ActiveDestination()
    {
        _destination ??= Instantiate(destinationPrefab, transform);
        _destination.gameObject.SetActive(true);
        var centerEnvironment = GetCenterEnvironment();

        if (!_isHavingAbnormality)
        {
            _environments[2].gameObject.SetActive(false);
            _destination.position = centerEnvironment.NextDestinationTarget.position;
            _destination.rotation = centerEnvironment.transform.rotation;
        }
        else
        {
            _environments[0].gameObject.SetActive(false);
            _destination.position = centerEnvironment.BackDestinationTarget.position;
            _destination.eulerAngles = centerEnvironment.transform.eulerAngles + Vector3.up * 180f;
        }
    }

    public void Win()
    {
        CurrentWaveIndex = 0;
    }

    private void RandomAbnormality()
    {
        var centerEnvironment = GetCenterEnvironment();
        _isHavingAbnormality = Random.value < 0.5f;
        if (_isHavingAbnormality) centerEnvironment.ActiveAbnormality();
        else centerEnvironment.ClearAbnormalities();

        _environments[0].ClearAbnormalities();
        _environments[2].ClearAbnormalities();
    }

    private void OnTrueWay()
    {
        if(UserData.IsFirstTime) return;
        if (CurrentWaveIndex > Configs.TARGET_WAVE) return;
        CurrentWaveIndex++;
        Debug.Log(CurrentWaveIndex);
    }

    private void OnWrongWay()
    {
        if(UserData.IsFirstTime) return;
        CurrentWaveIndex = 0;
        Debug.Log(CurrentWaveIndex);
    }

    private void OnDrawGizmos()
    {
        if (_environments == null || _environments.Count == 0) return;
        Gizmos.color = Color.red;
        var offset = 1f - 2f * GetCenterEnvironment().transform.rotation.y;
        Gizmos.DrawWireCube(GetCenterEnvironment().transform.position + centerOffset * offset,
            new Vector3(environmentWidth * 2f, environmentWidth, environmentWidth));
    }

    public Vector3 GetPlayerPosition()
    {
        return player.position;
    }
}