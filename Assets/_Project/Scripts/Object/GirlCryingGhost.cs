using System;
using DG.Tweening;
using UnityEngine;

public class GirlCryingGhost : MonoBehaviour
{
    [SerializeField] private GameObject _girlGhost;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _girlScreamSound;
    
    private void OnTriggerEnter(Collider other)
    {
        _girlGhost.SetActive(true);
        _girlGhost.transform.up = -(EnvironmentManager.Instance.GetPlayerPosition() - _girlGhost.transform.position);
        var target = EnvironmentManager.Instance.GetPlayerPosition() + EnvironmentManager.Instance.GetPlayer().forward * 1f;

        EnvironmentManager.Instance.PlayerManager.CameraBobbing.enabled = false;
        EnvironmentManager.Instance.PlayerManager.ViewController.enabled = false;
        EnvironmentManager.Instance.PlayerManager.MovementController.enabled = false;
        EnvironmentManager.Instance.PlayerManager.Rigidbody.velocity = Vector3.zero;

        _girlGhost.transform.DOMove(target, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
        {
            _audioSource.PlayOneShot(_girlScreamSound);
            UnityMainThread.Instance.AddDelayAction(2.2f, () =>
            {
                _girlGhost.SetActive(false);
                
                EnvironmentManager.Instance.PlayerManager.CameraBobbing.enabled = true;
                EnvironmentManager.Instance.PlayerManager.ViewController.enabled = true;
                EnvironmentManager.Instance.PlayerManager.MovementController.enabled = true;
            });
        });
        EnvironmentManager.Instance.PlayerManager.CameraHolder.DOLocalRotate(Vector3.zero, 0.25f);
        
        gameObject.SetActive(false);
    }
}