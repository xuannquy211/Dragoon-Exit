using System;
using UnityEngine;

public class KnockAndScream : InteractiveObject
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip knock;
    [SerializeField] private AudioClip scream;

    private bool _isScreamed;
    
    private void OnEnable()
    {
        _isScreamed = false;
    }

    public override void Activate()
    {
        source.PlayOneShot(knock);
        if (_isScreamed) return;
        _isScreamed = true;
        UnityMainThread.Instance.AddDelayAction(1f, () => source.PlayOneShot(scream));
    }
}