using System;
using System.Linq;
using UnityEngine;

public class UserData
{
    private const string CurrentWaveIndexKey = "CurrentWaveIndex";
    private const string AbnormalityKey = "Abnormality";

    public static int CurrentWaveIndex
    {
        get => PlayerPrefs.GetInt(CurrentWaveIndexKey, 0);
        set => PlayerPrefs.SetInt(CurrentWaveIndexKey, value);
    }

    public static int[] AbnormalitiesUsed
    {
        get
        {
            if (PlayerPrefs.HasKey(AbnormalityKey))
                return JsonUtility.FromJson<int[]>(PlayerPrefs.GetString(AbnormalityKey, ""));
            return Array.Empty<int>();
        }
        set => PlayerPrefs.SetString(AbnormalityKey, JsonUtility.ToJson(value));
    }

    public static void AddAbnormalityUsed(int abnormalityIndex)
    {
        var abnormalitiesUsed = AbnormalitiesUsed.ToList();
        abnormalitiesUsed.Add(abnormalityIndex);
        AbnormalitiesUsed = abnormalitiesUsed.ToArray();
    }
}