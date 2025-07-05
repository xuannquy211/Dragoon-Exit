using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : MonoBehaviour
{
    [SerializeField] private Image panel;
    [SerializeField] private Button resumeButton, settingsButton, restartButton;

    private readonly float _anchorXTarget = 361f;
    private Sequence _sequence;

    private void OnEnable()
    {
        ClearSequence();
        _sequence.Append(panel.DOFade(1f, 0.5f).From(0f));
        _sequence.Append((resumeButton.transform as RectTransform).DOAnchorPosX(_anchorXTarget, 0.25f)
            .From(new Vector3(-_anchorXTarget, 151.9f, 0f)).SetEase(Ease.OutBack));
        _sequence.Join((settingsButton.transform as RectTransform).DOAnchorPosX(_anchorXTarget, 0.35f)
            .From(new Vector3(-_anchorXTarget, 0f, 0f)).SetEase(Ease.OutBack));
        _sequence.Join((restartButton.transform as RectTransform).DOAnchorPosX(_anchorXTarget, 0.45f)
            .From(new Vector3(-_anchorXTarget, -151.9f, 0f)).SetEase(Ease.OutBack));
    }

    private void Start()
    {
        resumeButton.onClick.AddListener(OnClickResume);
        settingsButton.onClick.AddListener(OnClickSettings);
        restartButton.onClick.AddListener(OnClickRestart);
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
        Hide(() => Time.timeScale = 1f);
    }

    private void ClearSequence()
    {
        if (_sequence is { active: true } && _sequence.IsPlaying()) _sequence.Kill();
    }

    private void Hide(Action onComplete = null)
    {
        ClearSequence();

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