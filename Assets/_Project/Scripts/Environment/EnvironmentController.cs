using UnityEngine;
using UnityEngine.Serialization;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] private Transform nextEnvironmentTarget;
    [SerializeField] private Transform backEnvironmentTarget;
    [SerializeField] private Transform nextDestinationTarget;
    [SerializeField] private Transform backDestinationTarget;
    
    [Space(10)]
    [Header("========== Abnormalities ==========")]
    [SerializeField] private Abnormality[] abnormalities;
    
    public Transform NextEnvironmentTarget => nextEnvironmentTarget;
    public Transform BackEnvironmentTarget => backEnvironmentTarget;
    public Transform NextDestinationTarget => nextDestinationTarget;
    public Transform BackDestinationTarget => backDestinationTarget;

    public void ActiveAbnormality()
    {
        var index = Random.Range(0, abnormalities.Length);
        var totalAbnormalities = abnormalities.Length;
        for (var i = 0; i < totalAbnormalities; i++)
        {
            if(i == index) abnormalities[i].Active();
            else abnormalities[i].Deactive();
        }
    }

    public void ClearAbnormalities()
    {
        var totalAbnormalities = abnormalities.Length;
        for (var i = 0; i < totalAbnormalities; i++)
        {
            abnormalities[i].Deactive();
        }
    }
}