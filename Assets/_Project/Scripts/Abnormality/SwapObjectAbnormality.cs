using UnityEngine;

public class SwapObjectAbnormality : Abnormality
{
    [SerializeField] private GameObject[] rawKnocks;
    [SerializeField] private GameObject[] abnormalities;
    
    public override void Active()
    {
        var randomIndex = UnityEngine.Random.Range(0, abnormalities.Length);
        for (var i = 0; i < rawKnocks.Length; i++)
        {
            rawKnocks[i].SetActive(i != randomIndex);
            abnormalities[i].SetActive(i == randomIndex);
        }
    }

    public override void Deactive()
    {
        for (var i = 0; i < rawKnocks.Length; i++)
        {
            rawKnocks[i].SetActive(true);
            abnormalities[i].SetActive(false);
        }
    }
}