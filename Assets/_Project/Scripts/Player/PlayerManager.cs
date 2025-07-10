using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private FirstPersonCameraController _viewController;
    [SerializeField] private FirstPersonMovementController _movementController;
    [SerializeField] private Rigidbody rigidbody;

    public FirstPersonCameraController ViewController => _viewController;
    public FirstPersonMovementController MovementController => _movementController;

    public void Stop()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
}