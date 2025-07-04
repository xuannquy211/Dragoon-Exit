using UnityEngine;

public class ActiveRandomSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;

    public void ActiveSound()
    {
        var index = Random.Range(0, clips.Length);
        audioSource.PlayOneShot(clips[index]);
    }
}