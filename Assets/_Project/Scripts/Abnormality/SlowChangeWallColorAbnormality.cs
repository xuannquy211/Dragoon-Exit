using DG.Tweening;
using UnityEngine;

public class SlowChangeWallColorAbnormality : Abnormality
{
    [SerializeField] private Material mainMaterial;
    [SerializeField] private Color colorTarget;
    
    private bool isActive;
    
    public override void Active()
    {
        isActive = true;
        mainMaterial.DOColor(colorTarget, 5f);
    }

    public override void Deactive()
    {
        if (isActive)
        {
            mainMaterial.DOColor(Color.white, 5f);
            isActive = false;
        }
    }
}