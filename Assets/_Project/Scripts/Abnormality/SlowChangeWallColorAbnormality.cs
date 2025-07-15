using DG.Tweening;
using UnityEngine;

public class SlowChangeWallColorAbnormality : Abnormality
{
    [SerializeField] private MeshRenderer[] walls;
    [SerializeField] private Color colorTarget;
    [SerializeField] private GPUInstancing gpuInstancing;
    
    private bool isActive;
    
    public override void Active()
    {
        isActive = true;
        
        //gpuInstancing.Active(false);
        foreach (var wall in walls)
        {
            var mats = wall.materials;
            mats[1].color = colorTarget;
            wall.materials = mats;
        }
    }

    public override void Deactive()
    {
        if (isActive)
        {
            //gpuInstancing.Active(true);
            foreach (var wall in walls)
            {
                var mats = wall.materials;
                mats[1].color = Color.white;
                wall.materials = mats;
            }
            isActive = false;
        }
    }
}