using System;
using UnityEngine;

public class GirlController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private AnimationCurve wayCurve;
    [SerializeField] private Vector3 crossAxis;
    [SerializeField] private float speed;
    
    private bool _isWalking;
    private float _currentProgress;
    private Vector3 _direction;
    private Vector3 _per;
    
    public void ActiveWalking()
    {
        if(_isWalking) return;
        _isWalking = true;
        _currentProgress = 0f;
        transform.position = startPoint.position;
        _animator.Play("Walking");
        
        _direction = endPoint.position - startPoint.position;
        _per = Vector3.Cross(_direction, crossAxis).normalized;
    }

    private void Update()
    {
        if (!_isWalking) return;
        
        if (_currentProgress >= 1f)
        {
            _animator.Play("Idle");
            transform.position = endPoint.position;
            _isWalking = false;
            return;
        }
        
        UpdatePosition();
        
        _currentProgress += Time.deltaTime * speed;
        if (_currentProgress < 1f) return;
        _currentProgress = 1f;
    }

    private void UpdatePosition()
    {
        var progress = wayCurve.Evaluate(_currentProgress);
        var offset = curve.Evaluate(progress);
        var point = startPoint.position + _direction * progress + _per * offset;
        var dirLook = point - transform.position;
        
        transform.forward = dirLook.normalized;
        transform.position = point;
    }

    private void OnDrawGizmos()
    {
        if (!startPoint) return;
        if (!endPoint) return;
        
        var distance = Vector3.Distance(startPoint.position, endPoint.position);
        var dir = endPoint.position - startPoint.position;
        var totalPoint = distance * 10f;
        var per = Vector3.Cross(dir, crossAxis).normalized;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPoint.position, endPoint.position);

        Gizmos.color = Color.green;
        var lastPoint = startPoint.position;
        for (var i = 0; i < totalPoint; i++)
        {
            var progress = (float) i / totalPoint;
            var offset = curve.Evaluate(progress);
            var point = startPoint.position + dir * progress + per * offset;
            
            Gizmos.DrawLine(lastPoint, point);
            lastPoint = point;
        }
    }
}