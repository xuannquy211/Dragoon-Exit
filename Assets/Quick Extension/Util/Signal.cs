using System;
using System.Collections.Generic;

public class Signal
{
    private HashSet<Action> actions = new HashSet<Action>();

    public void AddListener(Action callback)
    {
        actions.Add(callback);
    }

    public void RemoveListener(Action callback)
    {
        if(actions.Contains(callback)) actions.Remove(callback);
    }

    public void Notify()
    {
        foreach (var action in actions)
        {
            action?.Invoke();
        }
    }
}

public class Signal<T>
{
    private HashSet<Action<T>> actions = new HashSet<Action<T>>();
    
    public void AddListener(Action<T> callback)
    {
        actions.Add(callback);
    }

    public void RemoveListener(Action<T> callback)
    {
        if(actions.Contains(callback)) actions.Remove(callback);
    }

    public void Notify(T data)
    {
        foreach (var action in actions)
        {
            action?.Invoke(data);
        }
    }
}

public class Signal<T1, T2>
{
    private HashSet<Action<T1, T2>> actions = new HashSet<Action<T1, T2>>();
    
    public void AddListener(Action<T1, T2> callback)
    {
        actions.Add(callback);
    }

    public void RemoveListener(Action<T1, T2> callback)
    {
        if(actions.Contains(callback)) actions.Remove(callback);
    }

    public void Notify(T1 data1, T2 data2)
    {
        foreach (var action in actions)
        {
            action?.Invoke(data1, data2);
        }
    }
}

public class Signal<T1, T2, T3>
{
    private HashSet<Action<T1, T2, T3>> actions = new HashSet<Action<T1, T2, T3>>();
    
    public void AddListener(Action<T1, T2, T3> callback)
    {
        actions.Add(callback);
    }

    public void RemoveListener(Action<T1, T2, T3> callback)
    {
        if(actions.Contains(callback)) actions.Remove(callback);
    }

    public void Notify(T1 data1, T2 data2, T3 data3)
    {
        foreach (var action in actions)
        {
            action?.Invoke(data1, data2, data3);
        }
    }
}
