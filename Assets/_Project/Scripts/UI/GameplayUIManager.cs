using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField] private Button interactButton;

    public static GameplayUIManager Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    public void ActiveInteractButton(bool isActive = true)
    {
        if(interactButton.gameObject.activeSelf != isActive) interactButton.gameObject.SetActive(isActive);
    }
}