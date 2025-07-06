using System;
using UnityEngine;

public class DoorMadTrigger : MonoBehaviour
{
    [SerializeField] private Animator[] animators;

    private void OnTriggerEnter(Collider other)
    {
        foreach (var animator in animators) animator.enabled = true;
        gameObject.SetActive(false);
    }
}