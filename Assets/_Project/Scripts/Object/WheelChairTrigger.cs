using System;
using DG.Tweening;
using UnityEngine;

public class WheelChairTrigger : MonoBehaviour
{
    [SerializeField] private Transform wheelChair;
    [SerializeField] private Transform endPoint;
    
    private void OnTriggerEnter(Collider other)
    {
        wheelChair.DOMove(endPoint.position, 2f).SetEase(Ease.OutSine);
        gameObject.SetActive(false);
    }
}