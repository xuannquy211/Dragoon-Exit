using System;
using UnityEngine;

public class GhostBehindTrigger : MonoBehaviour
{
    [SerializeField] private GhostBehindFollow ghost;

    private void OnTriggerEnter(Collider other)
    {
        ghost.Active();
        gameObject.SetActive(false);
    }
}