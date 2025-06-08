using UnityEngine;

public class CommonAbnormality : Abnormality
{
    [SerializeField] private GameObject abnormality;


    public override void Active()
    {
        abnormality.SetActive(true);
    }

    public override void Deactive()
    {
        abnormality.SetActive(false);
    }
}