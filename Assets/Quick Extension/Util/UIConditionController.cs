using System.Collections.Generic;
using MyBox;
using UnityEngine;

public abstract class UIConditionController : MonoBehaviour
{
    [SerializeField] protected bool hideOnNotEnoughtCondition;
    [ConditionalField(nameof(hideOnNotEnoughtCondition), true)] [SerializeField] protected GameObject notEnoughtConditionUI;
    [SerializeField] protected GameObject eligibleUI;

    protected HashSet<Signal> signals;
    protected List<bool> conditions;

    public void AddCondition(bool condition)
    {
        conditions.Add(condition);
    }
    
    public void AddListener(Signal signal)
    {
        signals.Add(signal);
    }

    public abstract void OnBeforeCheck();
    public abstract void OnAddListener();

    private void CheckCondition()
    {
        bool isEnable = !conditions.Contains(false);
        eligibleUI.SetActive(isEnable);
        if (isEnable) OnShowUI();
        
        if(hideOnNotEnoughtCondition) return;
        notEnoughtConditionUI.SetActive(!isEnable);
    }

    public void Check()
    {
        conditions = new();
        
        OnBeforeCheck();
        CheckCondition();
    }
    
    protected void OnEnable()
    {
        UnityMainThread.Instance.AddDelayFrameAction(2, () => Check());
    }

    private void Start()
    {
        //Check();
        
        signals = new HashSet<Signal>();
        
        OnAddListener();
        
        foreach (var s in signals)
        {
            s.AddListener(Check);
        }

        AfterStart();
    }
    
    protected virtual void AfterStart(){}

    public virtual void OnShowUI(){}
    
    private void OnDestroy()
    {
        try
        {
            foreach (var s in signals)
            {
                s.RemoveListener(Check);
            }
        }
        catch{}

        AfterDestroy();
    }
    
    protected virtual void AfterDestroy(){}
}