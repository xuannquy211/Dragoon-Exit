using UnityEngine;
using UnityEngine.Serialization;

public class GirlCryingAbnormality : Abnormality
{
    [SerializeField] private Material[] materials;
    [SerializeField] private GameObject _girlCryingAbnormality;
    [SerializeField] private GameObject _ghostTrigger;
    
    public override void Active()
    {
        _girlCryingAbnormality.SetActive(true);
        _ghostTrigger.SetActive(true);
        foreach (var mat in materials) mat.SetFloat("_Alpha", 1f);
    }

    public override void Deactive()
    {
        _girlCryingAbnormality.SetActive(false);
        _ghostTrigger.SetActive(false);
        foreach (var mat in materials) mat.SetFloat("_Alpha", 1f);
    }
}