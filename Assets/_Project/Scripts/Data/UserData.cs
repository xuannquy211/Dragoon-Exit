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

    public static void RemoveAbnormalityUsed(int abnormality)
    {
        var data = GetData();
        data.AbnormalitiesUsed.Remove(abnormality);
        SaveData();
    }
    
    private const string RateSpecialAbnormalityKey = "RateSpecialAbnormality";
    public static float RateSpecialAbnormality
    {
        get => PlayerPrefs.GetFloat(RateSpecialAbnormalityKey, 0.8f);
        set => PlayerPrefs.SetFloat(RateSpecialAbnormalityKey, value);
    }
    
    private const string GraphicOptionKey = "GraphicOption";

    public static int GraphicOption
    {
        get => PlayerPrefs.GetInt(GraphicOptionKey, 2);
        set
        {
            PlayerPrefs.SetInt(GraphicOptionKey, value);
            Observer.Notify("Graphic");
        }
    }

    private const string FOVKey = "FOV";

    public static float FOV
    {
        get => PlayerPrefs.GetFloat(FOVKey, 0f);
        set
        {
            PlayerPrefs.SetFloat(FOVKey, value);
            Observer.Notify("FOV");
        }
    }

    private const string SoundKey = "Sound";

    public static bool SoundEnabled
    {
        get => PlayerPrefs.GetInt(SoundKey, 1) > 0;
        set
        {
            PlayerPrefs.SetInt(SoundKey, value ? 1 : 0);
            Observer.Notify("Sound");
        }
    }
}