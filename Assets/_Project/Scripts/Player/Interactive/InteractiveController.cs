using UnityEngine;

public class InteractiveController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask interactiveLayers;
    
    private void Update()
    {
        var ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out var hit, 2f, interactiveLayers))
        {
            GameplayUIManager.Instance.ActiveInteractButton(hit.collider);
        }
    }
}