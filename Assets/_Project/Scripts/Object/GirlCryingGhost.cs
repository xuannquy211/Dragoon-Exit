using System;
using DG.Tweening;
using UnityEngine;

public class GirlCryingGhost : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject _girlGhost;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _girlScreamSound;

    private void OnTriggerEnter(Collider other)
    {
        EnvironmentManager.Instance.PlayerManager.CameraHolder.localRotation = Quaternion.identity;
        EnvironmentManager.Instance.PlayerManager.CameraBobbing.enabled = false;
        EnvironmentManager.Instance.PlayerManager.ViewController.enabled = false;
        EnvironmentManager.Instance.PlayerManager.MovementController.enabled = false;
        EnvironmentManager.Instance.PlayerManager.Rigidbody.velocity = Vector3.zero;

        _girlGhost.SetActive(true);
        animator.Play("Lick");
        _girlGhost.transform.forward = -(EnvironmentManager.Instance.GetPlayerPosition() - _girlGhost.transform.position);
        var target = EnvironmentManager.Instance.GetPlayerPosition() +
                     EnvironmentManager.Instance.GetPlayer().forward * 0.5f;
        _girlGhost.transform.position = target;
        _audioSource.PlayOneShot(_girlScreamSound);
        
        UnityMainThread.Instance.AddDelayAction(4f, () =>
        {
            _girlGhost.SetActive(false);

            EnvironmentManager.Instance.PlayerManager.CameraBobbing.enabled = true;
            EnvironmentManager.Instance.PlayerManager.ViewController.enabled = true;
            EnvironmentManager.Instance.PlayerManager.MovementController.enabled = true;
        });

        gameObject.SetActive(false);
    }
}