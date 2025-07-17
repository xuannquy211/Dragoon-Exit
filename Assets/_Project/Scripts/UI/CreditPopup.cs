using System;
using DG.Tweening;
using UnityEngine;

public class CreditPopup : MonoBehaviour
{
    [SerializeField] private float yTarget = 1400f;
    [SerializeField] private float duration = 10f;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform contentCredit;

    public void Init(Action onComplete = null)
    {
        var sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1f, 5f).From(0f));
        sequence.Append(contentCredit.DOAnchorPosY(yTarget, duration).From(Vector2.zero));
        sequence.Append(canvasGroup.DOFade(0f, 5f).OnComplete(() =>
        {
            onComplete?.Invoke();
            gameObject.SetActive(false);
        }));
    }

    private void OnDisable()
    {
        contentCredit.anchoredPosition = Vector2.zero;
        canvasGroup.alpha = 0f;
    }
}