using System;
using DG.Tweening;
using UnityEngine;

public class GirlCryingJumpscareTrigger : MonoBehaviour
{
    [SerializeField] private Material[] mats;
    [SerializeField] private GameObject _girlCrying;
    [SerializeField] private GameObject _girlGhostJumpScare;

    private void OnTriggerEnter(Collider other)
    {
        foreach (var mat in mats)
        {
            mat.DOFloat(1.5f, "_Alpha", 5f);
        }
        UnityMainThread.Instance.AddDelayAction(5f, () =>
        {
            _girlCrying.SetActive(false);
            foreach (var mat in mats) mat.SetFloat("_Alpha", 0f);
        });
        
        gameObject.SetActive(false);
    }
}