using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopupManager : SingletonDontDestroy<PopupManager>
{
    static readonly Stack<GameObject> popups = new();
    private PopupContainer popupContainer;

    private void Awake()
    {
        popupContainer ??= Resources.Load<PopupContainer>("PopupContainer");
        DontDestroyOnLoad(gameObject);
    }

    public T ShowPopup<T>(Transform parent = null) where T : PopupUI<T>
    {
        popupContainer ??= Resources.Load<PopupContainer>("PopupContainer");
        var prefab =
            popupContainer.datas.FirstOrDefault(popup => popup != null && popup.TryGetComponent<T>(out var com));

        if (prefab == null)
        {
            throw new NullReferenceException("Ko tim thay popup");
        }
        
        var popup = Instantiate(prefab, parent);
        popups.Push(popup.gameObject);

        return popup.GetComponent<T>();
    }

    public static void HidePopup(Action callback = null)
    {
        if (popups.Count > 0)
        {
            var popup = popups.Pop();
            if (popup == null)
            {
                HidePopup();
                return;
            }

            callback?.Invoke();
            Destroy(popup);
        }

        UIManager.IsShowingPopup = popups.Count > 0;
    }

    public static bool IsShowingPopup()
    {
        foreach (var popup in popups)
        {
            if (popup != null) return true;
        }

        return false;
    }
}