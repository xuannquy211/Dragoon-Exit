using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private Button resumeButton, settingsButton, restartButton;
    [SerializeField] private Button lowButton, mediumButton, highButton;
    [SerializeField] private GameObject[] onGraphicOption, offGraphicOption;
    [SerializeField] private Slider fovSlider;
    [SerializeField] private Toggle soundToggle;

    private readonly float _anchorXTarget = 361f;
    private Sequence _sequence;

    private void OnEnable()
    {
        ClearSequence();
        
        _sequence = DOTween.Sequence();
        _sequence.SetUpdate(true);
        _sequence.Append(panel.DOFade(1f, 0.5f).From(0f));

        UpdateGraphicOption();
        UpdateFOVOption();
        UpdateSoundToggle();
    }

    private void UpdateGraphicOption()
    {
        var currentOption = UserData.GraphicOption;
        
        for (var i = 0; i < onGraphicOption.Length; i++)
        {
            onGraphicOption[i].SetActive(i == currentOption);
            offGraphicOption[i].SetActive(i != currentOption);
        }
    }

    private void UpdateFOVOption()
    {
        var currentFOV = UserData.FOV;
        fovSlider.value = currentFOV;
    }

    private void UpdateSoundToggle()
    {
        var isSoundEnable = UserData.SoundEnabled;
        soundToggle.isOn = isSoundEnable;
    }

    private void Start()
    {
        resumeButton.onClick.AddListener(OnClickResume);
        settingsButton.onClick.AddListener(OnClickSettings);
        restartButton.onClick.AddListener(OnClickRestart);
        
        lowButton.onClick.AddListener(() => OnClickGraphicOption(0));
        mediumButton.onClick.AddListener(() => OnClickGraphicOption(1));
        highButton.onClick.AddListener(() => OnClickGraphicOption(2));

        fovSlider.onValueChanged.AddListener(OnFOVChange);
        soundToggle.onValueChanged.AddListener(OnSoundChange);
    }

    private void OnFOVChange(float value)
    {
        UserData.FOV = value;
    }

    private void OnSoundChange(bool value)
    {
        UserData.SoundEnabled = value;
    }

    private void OnClickGraphicOption(int option)
    {
        UserData.GraphicOption = option;
        UpdateGraphicOption();
    }
    
    private void OnClickResume()
    {
        Hide(() => Time.timeScale = 1f);
    }

    private void OnClickSettings()
    {
        Hide();
    }

    private void OnClickRestart()
    {
        Hide(() =>
        {
            Time.timeScale = 1f;
            EnvironmentManager.Instance.Restart();
        });
    }

    private void ClearSequence()
    {
        if (_sequence is { active: true } && _sequence.IsPlaying()) _sequence.Kill();
    }

    private void Hide(Action onComplete = null)
    {
        ClearSequence();

        _sequence = DOTween.Sequence();
        _sequence.SetUpdate(true);
        _sequence.Append((resumeButton.transform as RectTransform).DOAnchorPosX(-_anchorXTarget, 0.45f)
            .SetEase(Ease.InBack));
        _sequence.Join((settingsButton.transform as RectTransform).DOAnchorPosX(-_anchorXTarget, 0.35f)
            .SetEase(Ease.InBack));
        _sequence.Join((restartButton.transform as RectTransform).DOAnchorPosX(-_anchorXTarget, 0.25f)
            .SetEase(Ease.InBack));
        _sequence.Append(panel.DOFade(0f, 0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            onComplete?.Invoke();
        }));
    }
}