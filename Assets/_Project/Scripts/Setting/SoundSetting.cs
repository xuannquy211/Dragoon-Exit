using System;
using UnityEngine;

public class SoundSetting : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float valueMax;
    
    #if UNITY_EDITOR
    private void OnValidate()
    {
        audioSource = GetComponent<AudioSource>();
        valueMax = audioSource.volume;
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

    private void Start()
    {
        UpdateSound();
        
        Observer.AddEvent("Sound", UpdateSound);
    }

    private void UpdateSound(object data = null)
    {
        audioSource.volume = UserData.SoundEnabled ? valueMax : 0f;
    }
}