using System;
using UnityEngine;
using UnityEngine.AI;

public class GirlHuntingAbnormality : Abnormality
{
    [SerializeField] private GirlController girlController;
    [SerializeField] private NavMeshAgent agent;
    
    private bool _isActive;
    
    [Button]
    public override void Active()
    {
        _isActive = true;
        girlController.enabled = false;
        agent.enabled = true;
    }

    public override void Deactive()
    {
        if (_isActive)
        {
            agent.enabled = false;
            girlController.enabled = true;
            _isActive = false;
        }
    }

    private void Update()
    {
        if (_isActive) agent.SetDestination(EnvironmentManager.Instance.GetPlayerPosition());
    }
}