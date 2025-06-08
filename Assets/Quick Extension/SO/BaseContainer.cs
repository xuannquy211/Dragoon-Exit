using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class BaseContainer<T> : ScriptableObject where T : Object
{
    public List<T> datas;

    public bool IsContains(int id)
    {
        foreach (var data in datas)
        {
            if (data.GetInstanceID() == id) return true;
        }

        return false;
    }
    
    public void ClearMissingAsset()
    {
        var dataClones = datas.ToList();
        
        for (var i = 0; i < dataClones.Count; i++)
        {
            if (dataClones[i] == null)
            {
                dataClones.RemoveAt(i);
                i--;
            }
        }

        datas = dataClones.ToList();
    }
}