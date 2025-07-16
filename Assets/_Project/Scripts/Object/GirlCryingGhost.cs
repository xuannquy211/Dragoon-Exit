using System;
using UnityEngine;

public class GirlCryingGhost : MonoBehaviour
{
    [SerializeField] private float _radius = 10f;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}