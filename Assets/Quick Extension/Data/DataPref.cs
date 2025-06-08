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
            return Decrypt(value);
        }

        return defaultStr;
    }

    private static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, Encrypt(value));
    }
    
    private static string Encrypt(string decodedStr)
    {
        byte[] data = Encoding.UTF8.GetBytes(decodedStr);
        using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
        {
            csp.KeySize = 256;
            csp.BlockSize = 128;
            csp.Key = keyHash;
            csp.Padding = PaddingMode.PKCS7;
            csp.Mode = CipherMode.ECB;

            using (ICryptoTransform encrypter = csp.CreateEncryptor())
            {
                var arr = encrypter.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(arr);
            }
        }
    }

    private static string Decrypt(string encodedStr)
    {
        var data = Convert.FromBase64String(encodedStr);
        using var csp = new AesCryptoServiceProvider();
        csp.KeySize = 256;
        csp.BlockSize = 128;
        csp.Key = keyHash;
        csp.Padding = PaddingMode.PKCS7;
        csp.Mode = CipherMode.ECB;

        using var decrypter = csp.CreateDecryptor();
        var arr = decrypter.TransformFinalBlock(data, 0, data.Length);
        return Encoding.UTF8.GetString(arr);
    }
}