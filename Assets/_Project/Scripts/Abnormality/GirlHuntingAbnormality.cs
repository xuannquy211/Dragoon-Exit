using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class GirlHuntingAbnormality : Abnormality
{
    [SerializeField] private GirlController girlController;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;
    [SerializeField] private Transform endPoint;
    [SerializeField] private GameObject trigger;
    [SerializeField] private GameObject normalTrigger;

    private bool _isActive;

    [Button]
    public override void Active()
    {
        //_isActive = true;
        girlController.enabled = false;
        normalTrigger.SetActive(false);
        trigger.SetActive(true);
        //agent.enabled = true;
        //girlController.SetAnim("Walking");
    }

    public override void Deactive()
    {
        agent.enabled = false;
        girlController.enabled = true;
        _isActive = false;
        girlController.SetAnim("Idle");
        trigger.SetActive(false);
        normalTrigger.SetActive(true);
    }

    public void Begin()
    {
        girlController.SetAnim("Walking");
        transform.DOMove(endPoint.position, 2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.forward = EnvironmentManager.Instance.GetPlayerPosition() - transform.position;
            agent.enabled = true;
            _isActive = true;
            audioSource.PlayOneShot(clip);
        });
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
            playerManager.Stop();
            playerManager.transform.DOLookAt(transform.position,
                0.5f).OnComplete(EnvironmentManager.Instance.Restart);
            Deactive();
        }
    }

    private void OnDrawGizmos()
    {
        if (!endPoint) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, endPoint.position);
    }
}