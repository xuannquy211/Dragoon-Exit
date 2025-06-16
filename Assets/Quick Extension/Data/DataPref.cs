using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public abstract class DataPref<T> where T : class, new()
{
    private static T target;
    private static byte[] keyHash = Convert.FromBase64String("AQIDBAUGBWGJCGSMDQ0PAA==");
    
    public static T GetData(string keyAddition = "")
    {
        if (target != null) return target;
        
        var key = typeof(T) + keyAddition;
        if (PlayerPrefs.HasKey(key))
        {
            var rawData = GetString(key);
            target = JsonUtility.FromJson<T>(rawData);
        }
        else
        {
            target = new T();
            SaveData();
        }

        return target;
    }

    public static void SaveData(string keyAddition = "")
    {
        var rawData = JsonUtility.ToJson(target);
        SetString(typeof(T) + keyAddition, rawData);
    }
    
    private static string GetString(string key, string defaultStr = "")
    {
        var value = PlayerPrefs.GetString(key);
        if (!string.IsNullOrEmpty(value))
        {
            return value;
        }

        return defaultStr;
    }

    private static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
}