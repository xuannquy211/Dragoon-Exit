using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour
{
    [Header("Smoothing")]
    public float smoothTime = 0.05f;

    [Header("Clamp Vertical Look")]
    public float minPitch = -60f;
    public float maxPitch = 60f;

    private Transform playerBody;
    private float pitch = 0f;

    private Vector2 currentMouseDelta;
    private Vector2 currentMouseDeltaVelocity;

    public CameraInputProvider inputProvider;

    void Start()
    {
        playerBody = transform.parent;
        if (inputProvider == null)
        {
            Debug.LogError("Missing CameraInputProvider component!");
        }
    }

    void Update()
    {
        if (inputProvider == null) return;

        Vector2 lookInput = inputProvider.GetLookInput();
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, lookInput, ref currentMouseDeltaVelocity, smoothTime);
        pitch -= currentMouseDelta.y;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        playerBody.Rotate(Vector3.up * currentMouseDelta.x);
    }
}