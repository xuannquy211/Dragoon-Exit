using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    [Header("References")]
    public Rigidbody playerRigidbody;

    [Header("Idle Breathing")]
    public float idleAmplitude = 0.02f;
    public float idleFrequency = 1.5f;

    [Header("Movement Bobbing")]
    public float moveAmplitude = 0.05f;
    public float moveFrequency = 8f;

    [Header("Speed Sensitivity")]
    public float maxSpeedForBobbing = 6f;

    private Vector3 initialLocalPos;
    private float bobbingTimer = 0f;

    void Start()
    {
        if (playerRigidbody == null)
        {
            Debug.LogError("CameraBobbing requires reference to Rigidbody.");
            enabled = false;
            return;
        }

        initialLocalPos = transform.localPosition;
    }

    void Update()
    {
        float speed = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z).magnitude;

        float amplitude, frequency;

        if (speed > 0.1f)
        {
            float speedPercent = Mathf.Clamp01(speed / maxSpeedForBobbing);
            amplitude = Mathf.Lerp(idleAmplitude, moveAmplitude, speedPercent);
            frequency = Mathf.Lerp(idleFrequency, moveFrequency, speedPercent);
        }
        else
        {
            amplitude = idleAmplitude;
            frequency = idleFrequency;
        }

        bobbingTimer += Time.deltaTime * frequency;

        float bobOffsetY = Mathf.Sin(bobbingTimer * Mathf.PI * 2f) * amplitude;
        float bobOffsetX = Mathf.Cos(bobbingTimer * Mathf.PI * 2f) * amplitude * 0.5f;

        Vector3 targetPosition = initialLocalPos + new Vector3(bobOffsetX, bobOffsetY, 0f);

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * 10f);
    }

}