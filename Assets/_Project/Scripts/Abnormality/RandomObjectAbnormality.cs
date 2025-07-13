using UnityEngine;

public class RandomObjectAbnormality : Abnormality
{
    [SerializeField] private GameObject[] abnormalities;


    public override void Active()
    {
        var randomIndex = Random.Range(0, abnormalities.Length);
        abnormalities[randomIndex].SetActive(true);
    }

    public override void Deactive()
    {
        foreach (var abnormality in abnormalities) abnormality.SetActive(false);
    }
}