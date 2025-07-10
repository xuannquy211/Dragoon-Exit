using System;
using DG.Tweening;
using UnityEngine;

public class WheelChairTrigger : MonoBehaviour
{
    [SerializeField] private Transform wheelChair;
    [SerializeField] private Transform endPoint;
    public static bool IsWheelChairMoving { get; private set; }
    private void OnTriggerEnter(Collider other)
    {
        IsWheelChairMoving = true;

        wheelChair.DOMove(endPoint.position, 2f)
            .SetEase(Ease.OutSine)
            .OnComplete(() => IsWheelChairMoving = false);

        gameObject.SetActive(false);
    }
}