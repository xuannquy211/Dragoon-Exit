using UnityEngine;

public class LoaTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    
    private void OnTriggerEnter(Collider other)
    {
        _audioSource.Play();
        gameObject.SetActive(false);
    }
}