using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private EnvironmentController environmentPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private Transform destinationPrefab;
    [SerializeField] private float environmentLength = 80f;

    private readonly List<EnvironmentController> _environments = new List<EnvironmentController>();
    private bool _isHavingAbnormality = false;
    private Transform _destination;

    private int CurrentWaveIndex
    {
        get => UserData.CurrentWaveIndex;
        set => UserData.CurrentWaveIndex = value;
    }

    private void Start()
    {
        Debug.LogError(CurrentWaveIndex);
        
        FirstInitEnvironment();
        RandomAbnormality();
    }

    private void FirstInitEnvironment()
    {
        _environments.Clear();

        var centerEnvironment = Instantiate(environmentPrefab, transform);
        _environments.Add(centerEnvironment);
        centerEnvironment.gameObject.name = "CenterEnvironment";

        var backRotation = Quaternion.Euler(0f, 180f, 0f);
        var backPosition = centerEnvironment.BackEnvironmentTarget.position +
                           backRotation * Vector3.forward;

        var backEnvironment = Instantiate(environmentPrefab, backPosition, backRotation, transform);
        _environments.Insert(0, backEnvironment);
        backEnvironment.gameObject.name = "BackEnvironment";

        var nextPosition = centerEnvironment.NextEnvironmentTarget.position;
        var nextEnvironment = Instantiate(environmentPrefab, nextPosition, Quaternion.identity, transform);
        _environments.Add(nextEnvironment);
        nextEnvironment.gameObject.name = "NextEnvironment";
    }


    private void ShiftToNext()
    {
        var oldCenter = _environments[1];
        _environments.RemoveAt(1);
        var oldBack = _environments[0];
        _environments.RemoveAt(0);

        var currentCenter = _environments[0];
        var nextEuler = currentCenter.transform.eulerAngles;
        var backEuler = currentCenter.transform.eulerAngles + Vector3.up * 180f;
        oldBack.transform.eulerAngles = nextEuler;
        oldBack.transform.position = currentCenter.NextEnvironmentTarget.position;
        oldCenter.transform.eulerAngles = backEuler;

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
        var nextEuler = currentCenter.transform.eulerAngles;
        oldNext.transform.eulerAngles = nextEuler;
        oldNext.transform.position = currentCenter.NextEnvironmentTarget.position;

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
        var centerEnvironmentPosition = centerEnvironment.transform.position;
        if (Vector3.Distance(player.position, centerEnvironmentPosition) < environmentLength) return;

        var centerEnvironmentForward = centerEnvironment.transform.forward;
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

        RandomAbnormality();
        if (CurrentWaveIndex != Configs.TARGET_WAVE)
        {
            if(_destination) _destination.gameObject.SetActive(false);
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
        if (CurrentWaveIndex > Configs.TARGET_WAVE) return;
        CurrentWaveIndex++;
        Debug.Log(CurrentWaveIndex);
    }

    private void OnWrongWay()
    {
        CurrentWaveIndex = 0;
        Debug.Log(CurrentWaveIndex);
    }

    private void OnDrawGizmos()
    {
        if (_environments == null || _environments.Count == 0) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GetCenterEnvironment().transform.position,
            new Vector3(environmentLength, environmentLength, environmentLength * 2f));
    }
}