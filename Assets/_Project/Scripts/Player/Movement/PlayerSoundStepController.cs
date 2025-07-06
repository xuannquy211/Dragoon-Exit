using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSoundStepController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private float speed = 0.1f;
    
    private float _currentProgress = 0f;

    private void FixedUpdate()
    {
        _currentProgress += rigidbody.velocity.magnitude * Time.fixedDeltaTime * speed;
        if (_currentProgress < 1f) return;
        
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        _currentProgress = 0f;
    }
}