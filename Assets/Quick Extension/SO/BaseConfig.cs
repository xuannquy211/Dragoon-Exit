using System;
using UnityEngine;

public class BaseConfig<T> : ScriptableObject
{
    private static ConfigContainer container;
    private static T _ins;

    public static T GetConfig()
    {
        if (_ins == null)
        {
            container ??= Resources.Load<ConfigContainer>("ConfigContainer");
            var result = container.datas.Find(d => d.GetType() == typeof(T));
            _ins = (T)Convert.ChangeType(result, typeof(T));
        }
        
        return _ins;
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        var configContainer = Resources.Load<ConfigContainer>("ConfigContainer");
        var isContains = configContainer.IsContains(GetInstanceID());
        if (isContains) return;

        configContainer.datas.Add(this);
        configContainer.ClearMissingAsset();
        UnityEditor.EditorUtility.SetDirty(configContainer);
    }
    #endif
}