using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField] private Button interactButton;
    [SerializeField] private RectTransform crosshair;

    private float _crosshairSize = 1f;
    
    public static GameplayUIManager Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        interactButton.onClick.AddListener(OnClickInteractButton);
    }

    public void ActiveInteractButton(bool isActive = true)
    {
        if (interactButton.gameObject.activeSelf != isActive)
        {
            interactButton.gameObject.SetActive(isActive);
            _crosshairSize = isActive ? 2f : 1f;
        }
    }

    private void OnClickInteractButton()
    {
        InteractiveController.Instance.Active = true;
    }

    private void Update()
    {
        crosshair.localScale = Vector3.Lerp(crosshair.localScale, Vector3.one * _crosshairSize, Time.deltaTime * 10f);
    }
}