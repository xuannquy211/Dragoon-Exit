using UnityEngine;

public class GirlNoHatAbnormality : Abnormality
{
    [SerializeField] private GirlController _girlController;

    public override void Active()
    {
            _girlController.SetHatActive(false);
    }

    public override void Deactive()
    {
            _girlController.SetHatActive(true); 
    }
}