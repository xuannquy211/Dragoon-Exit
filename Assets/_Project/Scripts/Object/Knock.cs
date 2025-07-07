using UnityEngine;

public class Knock : InteractiveObject
{
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip clip;

    public override void Activate()
    {
        audio.PlayOneShot(clip);
    }
}