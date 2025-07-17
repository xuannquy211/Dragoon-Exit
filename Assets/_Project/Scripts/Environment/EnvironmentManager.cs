using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class EnvironmentManager : MonoBehaviour
{
    //[SerializeField] private EnvironmentController environmentPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Transform destinationPrefab;
    [SerializeField] private float environmentWidth = 80f;
    [SerializeField] private Vector3 centerOffset;
    [SerializeField] private int _totalPreviewMap = 2;
    [SerializeField] private List<EnvironmentController> _environments;
    [SerializeField] private Transform light, ground;
    [SerializeField] private Transform playerPoint;
    [SerializeField] private Volume postProcessing;

    private bool _isHavingAbnormality = false;
    private Transform _destination;
    private bool _stopChecking = false;

    public Volume PostProcessing => postProcessing;

    public PlayerManager PlayerManager => playerManager;

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

        if (CurrentWaveIndex >= Configs.TARGET_WAVE) ActiveDestination();
        UserData.SessionCount++;
    }

    private void FirstInitEnvironment()
    {
        var abnormalityUsed = UserData.GetAbnormalitiesUsed();

        var centerEnvironment = _environments[1]; //Instantiate(environmentPrefab, transform);
        centerEnvironment.InitAbnormality(abnormalityUsed);
        //_environments.Add(centerEnvironment);
        centerEnvironment.gameObject.name = "CenterEnvironment";

        /*var backRotation = new Quaternion(0f, 1f, 0f, 0f);
        var backPosition = centerEnvironment.BackEnvironmentTarget.position;*/

        var backEnvironment = _environments[0]; //Instantiate(environmentPrefab, backPosition, backRotation, transform);

        backEnvironment.InitAbnormality(abnormalityUsed);
        //_environments.Insert(0, backEnvironment);
        backEnvironment.gameObject.name = "BackEnvironment";

        //var nextPosition = centerEnvironment.NextEnvironmentTarget.position;
        var nextEnvironment =
            _environments[2]; //Instantiate(environmentPrefab, nextPosition, Quaternion.identity, transform);
        nextEnvironment.InitAbnormality(abnormalityUsed);
        //_environments.Add(nextEnvironment);
        nextEnvironment.gameObject.name = "NextEnvironment";

        player.SetParent(centerEnvironment.transform);
        light.SetParent(centerEnvironment.transform);
        ground.SetParent(centerEnvironment.transform);
    }


    private void ShiftToNext()
    {
        var oldCenter = _environments[1];
        _environments.RemoveAt(1);
        var oldBack = _environments[0];
        _environments.RemoveAt(0);

        var currentCenter = _environments[0];
        player.SetParent(currentCenter.transform);
        light.SetParent(currentCenter.transform);
        ground.SetParent(currentCenter.transform);

        var dot = Vector3.Dot(Vector3.right, player.forward);
        if (dot < 0f)
        {
            _stopChecking = true;
            GameplayUIManager.Instance.CloseEye(() =>
            {
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
                _stopChecking = false;
                
                if (UserData.IsFirstTime)
                {
                    _totalPreviewMap--;
                    if (_totalPreviewMap > 0)
                    {
                        return;
                    }

                    UserData.IsFirstTime = false;
                    RandomAbnormality();
                }
                else RandomAbnormality();

                if (CurrentWaveIndex != Configs.TARGET_WAVE)
                {
                    if (_destination) _destination.gameObject.SetActive(false);
                    return;
                }

                ActiveDestination();
            }, 0.25f);
        }
        else
        {
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
            
            if (UserData.IsFirstTime)
            {
                _totalPreviewMap--;
                if (_totalPreviewMap > 0)
                {
                    return;
                }

                UserData.IsFirstTime = false;
                RandomAbnormality();
            }
            else RandomAbnormality();

            if (CurrentWaveIndex != Configs.TARGET_WAVE)
            {
                if (_destination) _destination.gameObject.SetActive(false);
                return;
            }

            ActiveDestination();
        }
    }

    private void ShiftToBack()
    {
        var oldNext = _environments[2];
        _environments.RemoveAt(2);
        var oldCenter = _environments[1];
        _environments.RemoveAt(1);

        var currentCenter = _environments[0];

        player.SetParent(currentCenter.transform);
        light.SetParent(currentCenter.transform);
        ground.SetParent(currentCenter.transform);

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
        
        if (UserData.IsFirstTime)
        {
            _totalPreviewMap--;
            if (_totalPreviewMap > 0)
            {
                return;
            }

            UserData.IsFirstTime = false;
            RandomAbnormality();
        }
        else RandomAbnormality();

        if (CurrentWaveIndex != Configs.TARGET_WAVE)
        {
            if (_destination) _destination.gameObject.SetActive(false);
            return;
        }

        ActiveDestination();
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
        if (GameManager.State != GameState.PLAYING) return;
        if (_stopChecking) return;

        var centerEnvironment = GetCenterEnvironment();
        var offset = 1f - 2f * centerEnvironment.transform.rotation.y;
        var centerEnvironmentPosition = centerEnvironment.transform.position + centerOffset * offset;
        if (Mathf.Abs(player.position.x - centerEnvironmentPosition.x) < environmentWidth) return;
        if (Mathf.Abs(player.position.z - centerEnvironmentPosition.z) > environmentWidth / 2f) return;

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
        if (_isHavingAbnormality)
        {
            centerEnvironment.ActiveAbnormality();
            centerEnvironment.ActiveNumber(CurrentWaveIndex);
            _environments[0].ActiveNumber(CurrentWaveIndex + 1);
            _environments[2].ActiveNumber(0);
        }
        else
        {
            centerEnvironment.ClearAbnormalities();
            centerEnvironment.ActiveNumber(CurrentWaveIndex);
            _environments[2].ActiveNumber(CurrentWaveIndex + 1);
            _environments[0].ActiveNumber(0);
        }

        _environments[0].ClearAbnormalities();
        _environments[2].ClearAbnormalities();
    }

    private void OnTrueWay()
    {
        if (UserData.IsFirstTime) return;
        if (CurrentWaveIndex > Configs.TARGET_WAVE) return;
        CurrentWaveIndex++;
        Debug.Log(CurrentWaveIndex);
    }

    private void OnWrongWay()
    {
        if (UserData.IsFirstTime) return;
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

    public void Restart()
    {
        GameManager.State = GameState.PAUSED;
        if (!UserData.IsFirstTime) CurrentWaveIndex = 0;

        GameplayUIManager.Instance.CloseEye(ResetEnvironment);
    }

    public void EndGame()
    {
        GameManager.State = GameState.PAUSED;
        if (!UserData.IsFirstTime) CurrentWaveIndex = 0;

        GameplayUIManager.Instance.ShowCreditPopup(() =>
        {

            GameplayUIManager.Instance.CloseEye(ResetEnvironment);
        });
    }

    private void ResetEnvironment()
    {
        if (_destination) _destination.gameObject.SetActive(false);
        foreach (var environment in _environments) environment.gameObject.SetActive(true);
        
        foreach (var env in _environments)
        {
            env.ClearAbnormalities();
            env.ReInit();
        }

        playerManager.CameraBobbing.enabled = true;
        playerManager.MainCamera.transform.localPosition = Vector3.zero;
        playerManager.MainCamera.transform.localRotation = Quaternion.identity;
        playerManager.CameraHolder.transform.localPosition = new Vector3(0f ,2f, 0f);
        playerManager.CameraHolder.transform.localRotation = Quaternion.identity;
        player.position = playerPoint.position;
        player.rotation = playerPoint.rotation;
        playerManager.ViewController.enabled = true;
        playerManager.MovementController.enabled = true;
        
        playerManager.CameraBobbing.enabled = true;

        foreach (var environment in _environments) environment.gameObject.SetActive(true);
        GameManager.State = GameState.PLAYING;

        if (UserData.IsFirstTime) return;
        RandomAbnormality();
    }

    public Vector3 GetPlayerPosition()
    {
        return player.position;
    }

    public Transform GetPlayer()
    {
        return player;
    }

    public void ActiveAbnormality(int index)
    {
        GetCenterEnvironment().ActiveAbnormality(index);
    }
}