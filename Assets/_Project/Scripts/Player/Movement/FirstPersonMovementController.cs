using System;
using UnityEngine;

public class FirstPersonMovementController : MonoBehaviour
{
    public float walkingSpeed = 5f, runningSpeed = 10f;
    public float rayDistance = 0.6f;

    private Rigidbody rb;
    private MovementInputProvider inputProvider;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputProvider = GetComponent<MovementInputProvider>();
    }

    private void FixedUpdate()
    {
        if (inputProvider == null) return;

        Vector2 input = inputProvider.GetMoveInput();
        if (IsZeroInput(input))
        {
            var velo = rb.velocity;
            velo.x = 0f;
            velo.z = 0f;
            rb.velocity = velo;
            return;
        }

        Vector3 moveInput = (transform.forward * input.y + transform.right * input.x);
        moveInput.y = 0f;
        moveInput.Normalize();

        var moveSpeed = inputProvider.IsRunning() ? runningSpeed : walkingSpeed;
        Vector3 targetVelocity = moveInput * moveSpeed;
        Vector3 velocity = rb.velocity;
        if (velocity.magnitude > targetVelocity.magnitude) return;
        Vector3 velocityChange = targetVelocity - velocity;
        velocityChange.y = 0f;

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private bool IsZeroInput(Vector2 input)
    {
        return input is { x: 0f, y: 0f };
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
    }
}