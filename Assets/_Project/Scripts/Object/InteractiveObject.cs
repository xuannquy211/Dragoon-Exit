using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public static readonly Dictionary<Collider, InteractiveObject> InteractiveObjects = new Dictionary<Collider, InteractiveObject>(); 
    
    public abstract void Activate();

    private void Awake()
    {
        InteractiveObjects.Add(GetComponent<Collider>(), this);
    }
}