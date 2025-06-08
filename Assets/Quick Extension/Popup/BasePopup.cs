using UnityEngine;

public class BasePopup : MonoBehaviour
{
    protected static T Show<T>() where T : PopupUI<T>
    {
        UIManager.PopupShowing = typeof(T).ToString();
        return PopupManager.Instance.ShowPopup<T>();
    }
}