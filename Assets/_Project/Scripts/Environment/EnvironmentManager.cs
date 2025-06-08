using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private EnvironmentController environmentPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float environmentLength = 80f;

    private List<EnvironmentController> environments = new List<EnvironmentController>();

    private void Start()
    {
        FirstInitEnvironment();
    }

    private void FirstInitEnvironment()
    {
        environments.Clear();

        var centerEnvironment = Instantiate(environmentPrefab, transform);
        environments.Add(centerEnvironment);
        centerEnvironment.gameObject.name = "CenterEnvironment";

        var backRotation = Quaternion.Euler(0f, 180f, 0f);
        var backPosition = centerEnvironment.BackEnvironmentTarget.position + 
                           backRotation * Vector3.forward;

        var backEnvironment = Instantiate(environmentPrefab, backPosition, backRotation, transform);
        environments.Insert(0, backEnvironment);
        backEnvironment.gameObject.name = "BackEnvironment";

        var nextPosition = centerEnvironment.NextEnvironmentTarget.position;
        var nextEnvironment = Instantiate(environmentPrefab, nextPosition, Quaternion.identity, transform);
        environments.Add(nextEnvironment);
        nextEnvironment.gameObject.name = "NextEnvironment";
    }


    private void ShiftToNext()
    {
        var oldCenter = environments[1];
        environments.RemoveAt(1);
        var oldBack = environments[0];
        environments.RemoveAt(0);
        
        var currentCenter = environments[0];
        var nextEuler = currentCenter.transform.eulerAngles;
        var backEuler = currentCenter.transform.eulerAngles + Vector3.up * 180f;
        oldBack.transform.eulerAngles = nextEuler;
        oldBack.transform.position = currentCenter.NextEnvironmentTarget.position;
        oldCenter.transform.eulerAngles = backEuler;
        
        environments.Insert(0, oldCenter);
        environments.Add(oldBack);
        
        UpdateEnvironmentNames();
        GameSignal.MOVE_TO_ENVIRONMENT.Notify(environments[1]);
    }

    private void ShiftToBack()
    {
        var oldNext = environments[2];
        environments.RemoveAt(2);
        var oldCenter = environments[1];
        environments.RemoveAt(1);
        
        var currentCenter = environments[0];
        var nextEuler = currentCenter.transform.eulerAngles;
        oldNext.transform.eulerAngles = nextEuler;
        oldNext.transform.position = currentCenter.NextEnvironmentTarget.position;
        
        environments.Insert(0, oldCenter);
        environments.Add(oldNext);

        UpdateEnvironmentNames();
        GameSignal.MOVE_TO_ENVIRONMENT.Notify(environments[1]);
    }

    private void UpdateEnvironmentNames()
    {
        environments[0].gameObject.name = "BackEnvironment";
        environments[1].gameObject.name = "CenterEnvironment";
        environments[2].gameObject.name = "NextEnvironment";
    }

    private EnvironmentController GetCenterEnvironment()
    {
        return environments[1];
    }

    private void FixedUpdate()
    {
        var centerEnvironment = GetCenterEnvironment();
        var centerEnvironmentPosition = centerEnvironment.transform.position;
        if (Vector3.Distance(player.position, centerEnvironmentPosition) < environmentLength) return;
        
        var centerEnvironmentForward = centerEnvironment.transform.forward;
        var direction = Vector3.Normalize(player.position - centerEnvironmentPosition);
        var dot = Vector3.Dot(centerEnvironmentForward, direction);
        
        if(dot < 0) ShiftToBack();
        else ShiftToNext();
    }

    private void OnDrawGizmos()
    {
        if (environments == null || environments.Count == 0) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GetCenterEnvironment().transform.position, new Vector3(environmentLength, environmentLength, environmentLength * 2f));
    }
}
