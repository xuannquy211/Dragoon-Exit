using System;
using UnityEngine;

public class GirlHuntingTrigger : MonoBehaviour
{
    [SerializeField] private GirlHuntingAbnormality girlHuntingAbnormality;
    
    private void OnTriggerEnter(Collider other)
    {
        girlHuntingAbnormality.Begin();
        gameObject.SetActive(false);
    }
}