using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityMainThread : SingletonDontDestroy<UnityMainThread>
{
    readonly Queue<UnityAction> actions = new();

    public void AddAction(UnityAction action)
    {
        actions.Enqueue(action);
    }

    public void AddDelayAction(float delay, Action action)
    {
        StartCoroutine(JobDelay(delay, action));
    }
    
    public void AddDelayFrameAction(int delay, Action action)
    {
        StartCoroutine(JobDelayFrame(delay, action));
    }

    public void AddWaitUtilActive(Func<bool> condition, Action callback)
    {
        StartCoroutine(JobWaitUntil(condition, callback));
    }

    private void Update()
    {
        while(actions.Count > 0)
        {
            UnityAction action = actions.Dequeue();
            action?.Invoke();
        }
    }

    IEnumerator JobDelay(float delaySecond, Action callback)
    {
        yield return new WaitForSeconds(delaySecond);
        callback?.Invoke();
    }

    IEnumerator JobDelayFrame(int frame, Action callback)
    {
        for (var i = 0; i < frame; i++) yield return null;
        callback?.Invoke();
    }
    
    IEnumerator JobWaitUntil(Func<bool> condition, Action callback)
    {
        yield return new WaitUntil(condition);
        callback?.Invoke();
    }
}
