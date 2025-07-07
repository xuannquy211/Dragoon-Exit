using UnityEngine;

public class GhostBehindAbnormality : Abnormality
{
    [SerializeField] private GameObject trigger;
    [SerializeField] private GhostBehindFollow ghost;
    
    public override void Active()
    {
        trigger.SetActive(true);
        ghost.Disable();
    }

    public override void Deactive()
    {
        trigger.SetActive(false);
        ghost.Disable();
    }
}