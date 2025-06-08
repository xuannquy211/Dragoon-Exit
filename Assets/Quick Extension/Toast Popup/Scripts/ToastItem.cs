using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace QuickExtension
{
    public class ToastItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text desTxt;

        public void Init(string des, Color color, Action callback = null, bool activeEffect = true)
        {
            desTxt.text = des;
            desTxt.color = color;
        
            if(activeEffect) StartCoroutine(Effect(callback));
        }

        IEnumerator Effect(Action callback)
        {
            var canvasGroup = gameObject.AddComponent<CanvasGroup>();
        
            canvasGroup.DOFade(1f, 0.5f).From(0f).SetUpdate(true);
            yield return new WaitForSecondsRealtime(2f);
            canvasGroup.DOFade(0f, 0.5f).From(1f).SetUpdate(true).OnComplete(() => callback?.Invoke());
        }
    }
}