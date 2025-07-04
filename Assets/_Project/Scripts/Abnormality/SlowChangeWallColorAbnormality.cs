using DG.Tweening;
using UnityEngine;

public class SlowChangeWallColorAbnormality : Abnormality
{
    [SerializeField] private MeshRenderer[] walls;
    [SerializeField] private Color colorTarget;
    
    private bool isActive;
    
    public override void Active()
    {
        isActive = true;
        foreach (var wall in walls)
        {
            wall.materials[0].DOColor(colorTarget, 5f);
        }
    }

    public override void Deactive()
    {
        if (isActive)
        {
            foreach (var wall in walls)
            {
                wall.materials[0].DOColor(Color.white, 5f);
            }
            isActive = false;
        }
    }
}