using System;
using UnityEngine;

public class EndGamePoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EnvironmentManager.Instance.PlayerManager.CameraHolder.localRotation = Quaternion.identity;
        EnvironmentManager.Instance.PlayerManager.CameraBobbing.enabled = false;
        EnvironmentManager.Instance.PlayerManager.ViewController.enabled = false;
        EnvironmentManager.Instance.PlayerManager.MovementController.enabled = false;
        EnvironmentManager.Instance.PlayerManager.Rigidbody.velocity = Vector3.zero;
        
        EnvironmentManager.Instance.EndGame();
        gameObject.SetActive(false);
    }
}