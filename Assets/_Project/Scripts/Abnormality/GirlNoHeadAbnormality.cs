using UnityEngine;

public class GirlNoHeadAbnormality : Abnormality
{
    [SerializeField] private GirlController _girlController;

    public override void Active()
    {
        _girlController.SetHeadActive(false);
    }

    public override void Deactive()
    {
        _girlController.SetHeadActive(true);
    }
}