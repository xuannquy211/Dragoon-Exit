using UnityEngine;

namespace _Project.Scripts.Object
{
    public class Ball : InteractiveObject
    {
        [SerializeField] private Rigidbody rigidbody;
        
        public override void Activate()
        {
            rigidbody.AddForce(Vector3.up * 2f, ForceMode.Impulse);
        }
    }
}