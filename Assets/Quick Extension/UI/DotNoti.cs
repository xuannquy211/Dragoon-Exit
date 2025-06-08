using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DotNoti : MonoBehaviour
{
    [SerializeField] private GameObject dot;
    
    protected List<bool> conditions;

    public void AddCondition(bool condition)
    {
        
        conditions.Add(condition);
    }

    public abstract void OnBeforeCheck();

    private void CheckCondition()
    {
        bool isEnable = !conditions.Contains(false);
        dot.SetActive(isEnable);
        
        if(isEnable) OnEnableDot();
        else OnDisableDot();
    }

    public void Check()
    {
        conditions = new();
        OnBeforeCheck();
        CheckCondition();
    }
    
    protected void Start()
    {
        Check();
    }

    public virtual void OnEnableDot()
    {
        
    }

    public virtual void OnDisableDot()
    {
        
    }
}