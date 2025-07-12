using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BloodEnvironmentTrigger : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private void OnTriggerEnter(Collider other)
    {
        Active();
        gameObject.SetActive(false);
    }

    private void Active()
    {
        container.SetActive(true);
        audioSource.PlayOneShot(audioClip);
        if(EnvironmentManager.Instance.PostProcessing.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            chromaticAberration.active = true;
        }
        
        UnityMainThread.Instance.AddDelayAction(3f, Disable);
    }

    private void Disable()
    {
        container.SetActive(false);
        if(EnvironmentManager.Instance.PostProcessing.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            chromaticAberration.active = false;
        }
    }
}