using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField] private Button interactButton, pauseButton, testButton;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private Image panel;
    [SerializeField] private GameObject pausePopup;
    [SerializeField] private GameObject testPopup;

    private float _crosshairSize = 1f;
    
    public static GameplayUIManager Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        interactButton.onClick.AddListener(OnClickInteractButton);
        pauseButton.onClick.AddListener(OnClickPause);
        testButton.onClick.AddListener(OnClickTest);
        OpenEye();
    }

    public void OpenEye()
    {
        var effect = DOTween.Sequence();
        panel.gameObject.SetActive(true);
        effect.Append(panel.DOFade(0.9f, 2f).From(1f));
        effect.Append(panel.DOFade(1f, 0.5f));
        effect.Append(panel.DOFade(0.9f, 2f));
        effect.Append(panel.DOFade(1f, 0.5f));
        effect.Append(panel.DOFade(0f, 0.5f).OnComplete(() => panel.gameObject.SetActive(false)));
    }

    public void CloseEye(Action onClose = null)
    {
        var effect = DOTween.Sequence();
        panel.gameObject.SetActive(true);
        effect.Append(panel.DOFade(1f, 2f). OnComplete(() => onClose?.Invoke()));
        effect.Append(panel.DOFade(0f, 0.5f).OnComplete(() => panel.gameObject.SetActive(false)));
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

    private void OnClickPause()
    {
        Time.timeScale = 0f;
        pausePopup.SetActive(true);
    }

    private void OnClickTest()
    {
        testPopup.SetActive(true);
    }

    public void CloseTest()
    {
        testPopup.SetActive(false);
    }
    
    private void Update()
    {
        crosshair.localScale = Vector3.Lerp(crosshair.localScale, Vector3.one * _crosshairSize, Time.deltaTime * 10f);
    }
}