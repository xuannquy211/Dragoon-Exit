using System;
using UnityEngine;

public class GhostBehindFollow : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip hey;
    [SerializeField] private AudioClip disappear;
    [SerializeField] private GameObject body;

    private Vector3 _dir;
    private readonly float _distance = 1.5f;
    private Transform _player;
    private bool _isShowing;
    
    public void Active()
    {
        source.PlayOneShot(hey);
        _player = EnvironmentManager.Instance.GetPlayer();
        _dir = _player.forward * -1f * _distance;
        
        transform.position = _player.position + _dir;
        transform.forward = _player.forward;
        _isShowing = true;
        body.SetActive(true);
    }

    public void Disable()
    {
        _isShowing = false;
        body.SetActive(false);
    }

    private void Update()
    {
        if (!_player) return;
        if (!_isShowing) return;
        transform.position = _player.position + _dir;
        
        var toPlayer = Vector3.Normalize(transform.position - _player.position);
        var dot = Vector3.Dot(_player.forward, toPlayer);
        if (dot < 0.75f) return;
        JumpScare();
    }

    private void JumpScare()
    {
        body.SetActive(false);
        source.PlayOneShot(disappear);
        _isShowing = false;
    }
}