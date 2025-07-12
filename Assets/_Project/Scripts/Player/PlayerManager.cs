using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private FirstPersonCameraController _viewController;
    [SerializeField] private FirstPersonMovementController _movementController;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private CameraBobbing cameraBobbing;
    [SerializeField] private Transform mainCamera;

    public FirstPersonCameraController ViewController => _viewController;
    public FirstPersonMovementController MovementController => _movementController;
    public CameraBobbing CameraBobbing => cameraBobbing;
    public Transform MainCamera => mainCamera;

    public void Stop()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
}