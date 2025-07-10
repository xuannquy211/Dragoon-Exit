using UnityEngine;
using System.Collections;

public class WheelChairCollision : MonoBehaviour
{
    [SerializeField] private WheelChairTrigger wheelChairTrigger;
    private FirstPersonMovementController movementController;
    private FirstPersonCameraController cameraController;
    private Transform cameraTransform;
    private CameraBobbing cameraBobbing;

    private void Start()
    {
        wheelChairTrigger = FindObjectOfType<WheelChairTrigger>();
        movementController = FindObjectOfType<FirstPersonMovementController>();
        cameraController = FindObjectOfType<FirstPersonCameraController>();
        cameraTransform = cameraController.transform;
        cameraBobbing = FindObjectOfType<CameraBobbing>();
    }

    private void OnCollisionEnter(Collision collision)
    {
 
        if (collision.gameObject.CompareTag("Player"))
        {
 
            if (WheelChairTrigger.IsWheelChairMoving)
            {
                if (EnvironmentManager.Instance != null)
                {
                    StartCoroutine(FallAndRestart());
                }
            }
        }
    }

    private IEnumerator FallAndRestart()
    {
 
        movementController.enabled = false;
        cameraController.enabled = false;
        cameraBobbing.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;

        float fallDuration = 1f; 
        float elapsedTime = 0f;
        Vector3 startPosition = cameraTransform.localPosition;
        Vector3 targetPosition = startPosition + new Vector3(0f, -1.5f, -1.7f);

        Quaternion startRotation = cameraTransform.localRotation;
        Vector3 randomRotation = new Vector3(
            Random.Range(-70f, -110f),  
            Random.Range(-45f, 45f),  
            Random.Range(-30f, 30f)   
        );
        Quaternion targetRotation = Quaternion.Euler(randomRotation);

        while (elapsedTime < fallDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fallDuration;

 
            cameraTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            cameraTransform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }
 
        cameraTransform.localPosition = targetPosition;
        cameraTransform.localRotation = targetRotation;

        float waitTime = 0f;
        while (waitTime < 1f)
        {
            waitTime += Time.deltaTime;
            float shake = Mathf.Sin(waitTime * 20f) * 0.02f; 
            cameraTransform.localPosition = targetPosition + new Vector3(0, shake, 0);
            yield return null;
        }

        EnvironmentManager.Instance.Restart();
    }
}