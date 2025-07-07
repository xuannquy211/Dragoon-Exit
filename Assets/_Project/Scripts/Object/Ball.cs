using System;
using UnityEngine;

namespace _Project.Scripts.Object
{
    public class Ball : InteractiveObject
    {
        [SerializeField] private Rigidbody rigidbody;

        private Vector3 _startPosition;
        private Quaternion _startRotation;
        private bool _isInit;

        private void Start()
        {
            _startPosition = transform.position;
            _startRotation = transform.rotation;
            _isInit = true;
        }

        public override void Activate()
        {
            rigidbody.AddForce(Vector3.up * 7f, ForceMode.Impulse);
        }
    }
}