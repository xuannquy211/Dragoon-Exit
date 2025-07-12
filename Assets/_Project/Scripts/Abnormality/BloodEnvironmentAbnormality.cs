using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BloodEnvironmentAbnormality : Abnormality
{
    [SerializeField] private GameObject trigger;
    [SerializeField] private GameObject container;
    
    public override void Active()
    {
        trigger.SetActive(true);
        container.SetActive(false);
        if(EnvironmentManager.Instance.PostProcessing.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            chromaticAberration.active = false;
        }
    }

    public override void Deactive()
    {
        trigger.SetActive(false);
        container.SetActive(false);
        if(EnvironmentManager.Instance.PostProcessing.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            chromaticAberration.active = false;
        }
    }
}