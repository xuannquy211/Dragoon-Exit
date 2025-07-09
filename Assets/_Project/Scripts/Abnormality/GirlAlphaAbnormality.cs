using UnityEngine;

public class GirlAlphaAbnormality : Abnormality
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Material abnormalityMaterial;
    [SerializeField] private Material normalMaterial;
    
    public override void Active()
    {
        var mats = skinnedMeshRenderer.sharedMaterials;
        mats[0] = abnormalityMaterial;
        skinnedMeshRenderer.sharedMaterials = mats;
    }

    public override void Deactive()
    {
        var mats = skinnedMeshRenderer.sharedMaterials;
        mats[0] = normalMaterial;
        skinnedMeshRenderer.sharedMaterials = mats;
    }
}