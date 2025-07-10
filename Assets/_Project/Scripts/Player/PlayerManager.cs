using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private FirstPersonCameraController _viewController;
    [SerializeField] private FirstPersonMovementController _movementController;

    public FirstPersonCameraController ViewController => _viewController;
    public FirstPersonMovementController MovementController => _movementController;
}