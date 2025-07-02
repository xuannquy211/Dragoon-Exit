using System;
using UnityEngine;

public class GirlTriggerController : MonoBehaviour
{
    [SerializeField] private GirlController _girlController;

    private void OnTriggerEnter(Collider other)
    {
        _girlController.ActiveWalking();
        gameObject.SetActive(false);
    }
}