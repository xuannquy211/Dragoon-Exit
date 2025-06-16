using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData : DataPref<UserData>
{
    public List<int> AbnormalitiesUsed;
    
    private const string CurrentWaveIndexKey = "CurrentWaveIndex";
    private const string IsFirstTimeKey = "IsFirstTime";
    private const string SessionCountKey = "SessionCount";

    public static int CurrentWaveIndex
    {
        get => PlayerPrefs.GetInt(CurrentWaveIndexKey, 0);
        set => PlayerPrefs.SetInt(CurrentWaveIndexKey, value);
    }

    public static bool IsFirstTime
    {
        get => PlayerPrefs.GetInt(IsFirstTimeKey, -1) < 0;
        set => PlayerPrefs.SetInt(IsFirstTimeKey, value ? 1 : 0);
    }

    public static int SessionCount
    {
        get => PlayerPrefs.GetInt(SessionCountKey, 0);
        set => PlayerPrefs.SetInt(SessionCountKey, value);
    }

    public UserData()
    {
        AbnormalitiesUsed = new List<int>();
    }

    public static List<int> GetAbnormalitiesUsed()
    {
        return GetData().AbnormalitiesUsed;
    }

    public static void AddAbnormalityUsed(int abnormality)
    {
        var data = GetData();
        data.AbnormalitiesUsed.Add(abnormality);
        SaveData();
        
        GameSignal.ADD_ABNORMALITY.Notify(abnormality);
    }

    public static void ClearAbnormalityUsed()
    {
        var data = GetData();
        data.AbnormalitiesUsed.Clear();
        SaveData();
    }
}