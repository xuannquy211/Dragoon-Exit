using System;
using System.Linq;
using DG.Tweening;
using MyBox;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class PopupUI<T> : BasePopup where T : PopupUI<T>
{
    [SerializeField] protected RectTransform mainUI;

    [ConditionalField(nameof(mainUI), false)] [SerializeField]
    protected float offsetY = 200;

    [SerializeField] protected Transform[] uiEffects;

    protected CanvasGroup canvasGroup;
    protected Sequence effect;
    protected Action finallyCallback;

    public Action FinallyCallback
    {
        set => finallyCallback = value;
    }

    protected void Awake()
    {
        PopEffect();
        ActiveUIScaleEffect();
    }

    protected void ActiveUIScaleEffect()
    {
        foreach (var ui in uiEffects) ScaleEffectRandom(ui);
    }

    protected virtual void PopEffect()
    {
        canvasGroup = transform.GetOrAddComponent<CanvasGroup>();
        effect = DOTween.Sequence().SetUpdate(true);
        effect.Append(canvasGroup.DOFade(1f, 0.35f).From(0f));

        if (mainUI == null) return;
        var toPos = mainUI.anchoredPosition;
        var fromPos = toPos;
        fromPos.y += offsetY;

        effect.Join(mainUI.DOAnchorPos(toPos, 0.35f).SetEase(Ease.OutBack).From(fromPos));
    }

    public void Hide(Action callback = null, bool offStateShowUI = true)
    {
        UIManager.BlockClick = true;

        ClearEffect();

        effect = DOTween.Sequence().SetUpdate(true);
        effect.Append(canvasGroup.DOFade(0f, 0.35f).SetUpdate(true).OnComplete(() =>
        {
            UIManager.BlockClick = false;
            PopupManager.HidePopup();
            callback?.Invoke();
        }));
        if (mainUI != null)
        {
            var fromPos = mainUI.anchoredPosition;
            var toPos = fromPos;
            toPos.y -= offsetY;
            effect.Join(mainUI.DOAnchorPos(toPos, 0.35f).SetEase(Ease.InBack).From(fromPos)).SetUpdate(true);
        }
    }

    protected void ClearEffect()
    {
        if (effect != null) effect.Kill();
    }

    public static T Show()
    {
        return Show<T>();
    }

    protected virtual void OnDestroy()
    {
        ClearEffect();
    }

    void ScaleEffectRandom(Transform ui)
    {
    }
    
    public void ActiveHideEffect(RectTransform target, Action callback = null)
    {
        effect = DOTween.Sequence().SetUpdate(true);
        effect.Append(canvasGroup.DOFade(0f, 0.35f).SetUpdate(true).OnComplete(() =>
        {
            callback?.Invoke();
        }));
        var fromPos = target.anchoredPosition;
        var toPos = fromPos;
        toPos.y -= offsetY;
        effect.Join(target.DOAnchorPos(toPos, 0.35f).SetEase(Ease.InBack).From(fromPos)).SetUpdate(true);
    }
    
    public void ActiveShowEffect(RectTransform target)
    {
        effect = DOTween.Sequence().SetUpdate(true);
        effect.Append(canvasGroup.DOFade(1f, 0.35f).From(0f));

        var toPos = target.anchoredPosition;
        var fromPos = toPos;
        fromPos.y += offsetY;

        effect.Join(target.DOAnchorPos(toPos, 0.35f).SetEase(Ease.OutBack).From(fromPos));
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (UnityEditor.PrefabUtility.IsPartOfPrefabAsset(gameObject))
        {
            var container = Resources.Load<PopupContainer>("PopupContainer");
            var isContainer = container.IsContains(GetInstanceID());
            if (isContainer) return;
        
            container.datas.Add(this);
            container.ClearMissingAsset();
            UnityEditor.EditorUtility.SetDirty(container);
        }
    }
    #endif
}