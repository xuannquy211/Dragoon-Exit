using UnityEngine;
using UnityEngine.Serialization;

public class GirlCryingAbnormality : Abnormality
{
    [SerializeField] private GameObject _girlCryingAbnormality;
    [SerializeField] private GameObject _ghostTrigger;
    
    public override void Active()
    {
        _girlCryingAbnormality.SetActive(true);
        _ghostTrigger.SetActive(true);
    }

    public override void Deactive()
    {
        _girlCryingAbnormality.SetActive(false);
        _ghostTrigger.SetActive(false);
    }
}