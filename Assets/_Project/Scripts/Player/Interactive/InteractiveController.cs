using System;
using UnityEngine;

public class InteractiveController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask interactiveLayers;
    
    public bool Active { get; set; }
    public static InteractiveController Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        var ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out var hit, 3f, interactiveLayers))
        {
            GameplayUIManager.Instance.ActiveInteractButton();
            if (Active)
            {
                InteractiveObject.InteractiveObjects[hit.collider].Activate();
                Active = false;
            }
        }
        else GameplayUIManager.Instance.ActiveInteractButton(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * 3f);
    }
}