using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    
    public static readonly Dictionary<Collider, List<InteractiveObject>> InteractiveObjects =
        new Dictionary<Collider, List<InteractiveObject>>();

    public abstract void Activate();

    private void OnEnable()
    {
        if (InteractiveObjects.TryAdd(_collider, new List<InteractiveObject>() { this })) return;
        if (InteractiveObjects[_collider].Contains(this)) return; 
        InteractiveObjects[_collider].Add(this);
    }

    private void OnDisable()
    {
        if (InteractiveObjects.ContainsKey(_collider) && InteractiveObjects[_collider].Contains(this)) InteractiveObjects[_collider].Remove(this);
    }
}