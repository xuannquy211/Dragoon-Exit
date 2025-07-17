using UnityEngine;

public class Dog : InteractiveObject
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] clips;
    
    public override void Activate()
    {
        var clip = clips[Random.Range(0, clips.Length)];
        source.PlayOneShot(clip);
    }
}