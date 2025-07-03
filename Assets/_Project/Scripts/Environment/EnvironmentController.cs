using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] private Transform nextEnvironmentTarget;
    [SerializeField] private Transform backEnvironmentTarget;
    [SerializeField] private Transform nextDestinationTarget;
    [SerializeField] private Transform backDestinationTarget;
    [SerializeField] private GameObject girlTrigger;
    [SerializeField] private Transform girl, girlStartPoint;
    [SerializeField] private GirlController girlController;

    [Space(10)] [Header("========== Abnormalities ==========")] [SerializeField]
    private Abnormality[] abnormalities;

    public Transform NextEnvironmentTarget => nextEnvironmentTarget;
    public Transform BackEnvironmentTarget => backEnvironmentTarget;
    public Transform NextDestinationTarget => nextDestinationTarget;
    public Transform BackDestinationTarget => backDestinationTarget;
    private List<int> _abnormalitiesIndices;

    private void Start()
    {
        GameSignal.ADD_ABNORMALITY.AddListener(OnAddAbnormalityUsed);
    }

    public void InitAbnormality(List<int> abnormalitiesUsed)
    {
        _abnormalitiesIndices = new List<int>();
        for (var i = 0; i < abnormalities.Length; i++)
        {
            if (abnormalitiesUsed.Contains(i)) continue;
            _abnormalitiesIndices.Add(i);
        }
        
        if(_abnormalitiesIndices.Count == 0) ReInitAbnormalities();
    }

    public void ReInitAbnormalities()
    {
        _abnormalitiesIndices = new List<int>();
        for (var i = 0; i < abnormalities.Length; i++)
        {
            _abnormalitiesIndices.Add(i);
        }
        
        UserData.ClearAbnormalityUsed();
    }

    public void ActiveAbnormality()
    {
        var randomIndex = Random.Range(0, _abnormalitiesIndices.Count);
        var index = _abnormalitiesIndices[randomIndex];
        Debug.Log($"Abnormality: {index}");
        var totalAbnormalities = abnormalities.Length;
        for (var i = 0; i < totalAbnormalities; i++)
        {
            if (i == index) abnormalities[i].Active();
            else abnormalities[i].Deactive();
        }
        
        _abnormalitiesIndices.RemoveAt(randomIndex);
        UserData.AddAbnormalityUsed(index);
        EnvironmentManager.AbnormalitiesSeen.Add(index);
        
        if(_abnormalitiesIndices.Count == 0) ReInitAbnormalities();
    }

    private void OnAddAbnormalityUsed(int index)
    {
        if (_abnormalitiesIndices.Contains(index))
        {
            _abnormalitiesIndices.Remove(index);
            if(_abnormalitiesIndices.Count == 0) ReInitAbnormalities();
        }
        
    }

    public void ClearAbnormalities()
    {
        var totalAbnormalities = abnormalities.Length;
        for (var i = 0; i < totalAbnormalities; i++)
        {
            abnormalities[i].Deactive();
        }
    }

    public void ReInit()
    {
        girlTrigger.SetActive(true);
        girl.position = girlStartPoint.position;
        girl.forward = Vector3.forward;
        girlController.SetAnim("Walking");
    }

    [Button]
    public void ActiveAbnormality(int index)
    {
        abnormalities[index].Active();
    }
    
    private void OnDestroy()
    {
        GameSignal.ADD_ABNORMALITY.RemoveListener(OnAddAbnormalityUsed);
    }
}