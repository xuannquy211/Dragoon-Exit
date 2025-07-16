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
    [SerializeField] private GameObject[] numbers;

    [Space(10)] [Header("========== Abnormalities ==========")] [SerializeField]
    private Abnormality[] abnormalities;

    public Transform NextEnvironmentTarget => nextEnvironmentTarget;
    public Transform BackEnvironmentTarget => backEnvironmentTarget;
    public Transform NextDestinationTarget => nextDestinationTarget;
    public Transform BackDestinationTarget => backDestinationTarget;
    private Dictionary<bool, List<int>> _abnormalitiesDictionary;

    private void Start()
    {
        GameSignal.ADD_ABNORMALITY.AddListener(OnAddAbnormalityUsed);
    }

    public void InitAbnormality(List<int> abnormalitiesUsed)
    {
        _abnormalitiesDictionary = new Dictionary<bool, List<int>>
        {
            { false, new List<int>() },
            { true, new List<int>() }
        };
        
        for (var i = 0; i < abnormalities.Length; i++)
        {
            if (abnormalitiesUsed.Contains(i)) continue;
            _abnormalitiesDictionary[abnormalities[i].IsSpecial].Add(i);
        }
        
        if(_abnormalitiesDictionary[true].Count == 0) ReInitAbnormalitiesDictionary(true);
        if(_abnormalitiesDictionary[false].Count == 0) ReInitAbnormalitiesDictionary(false);
    }

    private void ReInitAbnormalitiesDictionary(bool isSpecial)
    {
        foreach (var id in _abnormalitiesDictionary[isSpecial]) UserData.RemoveAbnormalityUsed(id);
        
        _abnormalitiesDictionary[isSpecial].Clear();
        for(var i = 0; i < abnormalities.Length; i++)
        {
            if(abnormalities[i].IsSpecial != isSpecial) continue;
            _abnormalitiesDictionary[isSpecial].Add(i);
        }
    }

    public void ActiveAbnormality()
    {
        var rateSpecialAbnormality = UserData.RateSpecialAbnormality;
        Debug.Log($"Rate special abnormality: {rateSpecialAbnormality * 100f}%");
        var isSpecial = Random.value < rateSpecialAbnormality;
        
        var randomIndex = Random.Range(0, _abnormalitiesDictionary[isSpecial].Count);
        var index = _abnormalitiesDictionary[isSpecial][randomIndex];
        var totalAbnormalities = abnormalities.Length;
        for (var i = 0; i < totalAbnormalities; i++)
        {
            if (i == index) continue;
            abnormalities[i].Deactive();
        }
        abnormalities[index].Active();
        
        _abnormalitiesDictionary[isSpecial].RemoveAt(randomIndex);
        UserData.AddAbnormalityUsed(index);
        EnvironmentManager.AbnormalitiesSeen.Add(index);
        
        if(_abnormalitiesDictionary[isSpecial].Count == 0) ReInitAbnormalitiesDictionary(isSpecial);
        if (isSpecial) UserData.RateSpecialAbnormality = 0f;
        else UserData.RateSpecialAbnormality += 0.4f;
    }

    private void OnAddAbnormalityUsed(int index)
    {
        if (_abnormalitiesDictionary[true].Contains(index))
        {
            _abnormalitiesDictionary[true].Remove(index);
            if(_abnormalitiesDictionary[true].Count == 0) ReInitAbnormalitiesDictionary(true);
        }
        else if (_abnormalitiesDictionary[false].Contains(index))
        {
            _abnormalitiesDictionary[false].Remove(index);
            if(_abnormalitiesDictionary[false].Count == 0) ReInitAbnormalitiesDictionary(false);
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
        girlController.StopWalking();
        girl.position = girlStartPoint.position;
        girl.forward = Vector3.forward;
        girlController.SetAnim("Idle");
    }

    public void ActiveNumber(int index)
    {
        for (var i = 0; i < numbers.Length; i++)
        {
            numbers[i].SetActive(i == index);
        }
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