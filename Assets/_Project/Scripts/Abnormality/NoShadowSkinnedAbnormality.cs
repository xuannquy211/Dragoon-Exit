using UnityEngine;
using UnityEngine.Rendering;

public class NoShadowSkinnedAbnormality : Abnormality
{
    [SerializeField] private SkinnedMeshRenderer _renderer;
    
    public override void Active()
    {
        _renderer.shadowCastingMode = ShadowCastingMode.Off;
    }

    public override void Deactive()
    {
        _renderer.shadowCastingMode = ShadowCastingMode.On;
    }
}