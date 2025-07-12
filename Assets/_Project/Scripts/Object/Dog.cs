using UnityEngine;

public class Dog : InteractiveObject
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;
    
    public override void Activate()
    {
        source.PlayOneShot(clip);
    }
}