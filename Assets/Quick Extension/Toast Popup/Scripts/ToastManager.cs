using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace QuickExtension
{
    public class ToastManager : MonoBehaviour
    {
        [SerializeField] private ToastItem toastItemPrefab;
        [SerializeField] private Transform containerUp, containerCenter;

        private List<Transform> _toast;

        private readonly Vector2 offsetPosition = new(0f, -100f);

        private static ToastManager ins;

        public static ToastManager Ins
        {
            get
            {
                if (ins == null)
                {
                    var manager = Instantiate(Resources.Load<ToastManager>("Toast Manager"));
                    ins = manager;
                    DontDestroyOnLoad(manager.gameObject);
                }

                return ins;
            }
        }

        public void AddNoti(string notiDes, Color textColor, NotiType type = NotiType.StackUp)
        {
            _toast ??= new();

            var toast = Instantiate(toastItemPrefab, type == NotiType.StackUp ? containerUp : containerCenter);
            ((RectTransform)toast.transform).anchoredPosition = Vector2.zero;

            switch (type)
            {
                case NotiType.StackUp:
                    toast.Init(notiDes, textColor, () =>
                    {
                        _toast.Remove(toast.transform);
                        Destroy(toast.gameObject);
                    });
                    _toast.Add(toast.transform);
                    SyncStackUpPosition();
                    break;
                case NotiType.FlyInCenter:
                    toast.Init(notiDes, textColor, activeEffect: false);
                    var canvasGroup = toast.gameObject.AddComponent<CanvasGroup>();

                    var sequence = DOTween.Sequence();
                    sequence.Append((toast.transform as RectTransform).DOAnchorPosY(200f, 0.5f));
                    sequence.Append(canvasGroup.DOFade(0f, 0.25f).From(1f));
                    sequence.OnComplete(() => Destroy(toast.gameObject));
                    break;
            }
        }

        public void SyncStackUpPosition()
        {
            int totalNoti = _toast.Count;

            for (int i = 0; i < totalNoti; i++)
            {
                if (_toast == null) continue;
                int offset = totalNoti - 1 - i;
                Vector2 anchorPos = offset * offsetPosition;

                (_toast[i] as RectTransform).DOAnchorPos(anchorPos, 0.25f).SetUpdate(true);
            }
        }
    }

    public enum NotiType
    {
        StackUp,
        FlyInCenter
    }
}