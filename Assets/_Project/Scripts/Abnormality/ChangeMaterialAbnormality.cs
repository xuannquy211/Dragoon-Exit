using UnityEngine;
using UnityEngine.Serialization;

public class ChangeMaterialAbnormality : Abnormality
{
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private Material matAbnormality;
    [SerializeField] private Material normalAbnormality;
    
    public override void Active()
    {
        meshRenderer.sharedMaterial = matAbnormality;
    }

    public override void Deactive()
    {
        meshRenderer.sharedMaterial = normalAbnormality;
    }
}