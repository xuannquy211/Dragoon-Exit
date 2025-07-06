using UnityEngine;

public class HideObjectAbnormality : Abnormality
{
    [SerializeField] private GameObject[] obj;
    
    public override void Active()
    {
        foreach (var o in obj) o.SetActive(false);
    }

    public override void Deactive()
    {
        foreach (var o in obj) o.SetActive(true);
    }
}