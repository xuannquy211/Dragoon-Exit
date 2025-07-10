using System;
using DG.Tweening;
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
        girlController.SetAnim("Walking");
    }

    public override void Deactive()
    {
        if (_isActive)
        {
            agent.enabled = false;
            girlController.enabled = true;
            _isActive = false;
            girlController.SetAnim("Idle");
        }
    }

    private void Update()
    {
        if (_isActive)
        {
            agent.SetDestination(EnvironmentManager.Instance.GetPlayerPosition());
            if (Vector3.Distance(transform.position, EnvironmentManager.Instance.GetPlayerPosition()) > 1f) return;
            agent.transform.LookAt(EnvironmentManager.Instance.GetPlayerPosition());
            agent.enabled = false;
            var playerManager = EnvironmentManager.Instance.PlayerManager;
            playerManager.MovementController.enabled = false;
            playerManager.ViewController.enabled = false;
            playerManager.transform.DOLookAt(transform.position,
                0.5f).OnComplete(EnvironmentManager.Instance.Restart);
            _isActive = false;
        }
    }
}