using UnityEngine;

public class PosterScaleAnormality : Abnormality
{
    [SerializeField] private GameObject posterScaleAnormality;
    [SerializeField] private GameObject posterScaleNormal;
    
    public override void Active()
    {
        posterScaleAnormality.SetActive(true);
        posterScaleNormal.SetActive(false);
    }

    public override void Deactive()
    {
        posterScaleAnormality.SetActive(false);
        posterScaleNormal.SetActive(true);
    }
}