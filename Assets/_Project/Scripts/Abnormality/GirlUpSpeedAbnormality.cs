using UnityEngine;

public class GirlUpSpeedAbnormality : Abnormality
{
    [SerializeField] private GirlController _girlController;
    
    public override void Active()
    {
        _girlController.Speed = 0.1f;
    }

    public override void Deactive()
    {
        _girlController.Speed = 0.05f;
    }
}