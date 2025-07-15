using UnityEngine;

public class DoorMadAbnormality : Abnormality
{
    [SerializeField] private Animator[] doors;
    [SerializeField] private GameObject trigger;


    public override void Active()
    {
        foreach (var door in doors)
        {
            door.enabled = false;
            door.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }
        trigger.SetActive(true);
    }

    public override void Deactive()
    {
        foreach (var door in doors)
        {
            door.enabled = false;
            door.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }
        trigger.SetActive(false);
    }
}