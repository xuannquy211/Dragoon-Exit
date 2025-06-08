using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SOContainer
{
    public static T GetSO<T>()
    {
        var container = Resources.Load<ConfigContainer>("ConfigContainer");
        var result = container.datas.Find(d => d.GetType() == typeof(T));
        
        return (T)Convert.ChangeType(result, typeof(T));
    }
}