using UnityEngine;

public class WheelChairAbnormality : Abnormality
{
    [SerializeField] private Transform wheelChair;
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject triggerPoint;
    
    public override void Active()
    {
        wheelChair.gameObject.SetActive(true);
        wheelChair.position = startPoint.position;
        triggerPoint.SetActive(true);
    }

    public override void Deactive()
    {
        wheelChair.gameObject.SetActive(false);
        wheelChair.position = startPoint.position;
        triggerPoint.SetActive(false);
    }
}