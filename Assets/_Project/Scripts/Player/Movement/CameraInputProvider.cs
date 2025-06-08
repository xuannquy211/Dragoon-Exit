using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraInputProvider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Settings")]
    public float touchSensitivity = 0.01f;
    public RectTransform joystickArea;

    private Vector2 lastTouchPos;
    private Vector2 lookDelta;
    private bool isDragging = false;
    private int fingerId = -1;

    public Vector2 GetLookInput()
    {
        Vector2 delta = lookDelta;
        lookDelta = Vector2.zero;
        return delta * touchSensitivity;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isDragging || IsPointerOverJoystick(eventData)) return;

        isDragging = true;
        lastTouchPos = eventData.position;
        fingerId = eventData.pointerId;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != fingerId || !isDragging) return;

        Vector2 currentPos =  eventData.position;
        lookDelta = currentPos - lastTouchPos;
        lastTouchPos = currentPos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        lookDelta = Vector2.zero;
        fingerId = -1;
    }

    private bool IsPointerOverJoystick(PointerEventData eventData)
    {
        if (joystickArea == null) return false;

        return RectTransformUtility.RectangleContainsScreenPoint(joystickArea, eventData.position, eventData.enterEventCamera);
    }
}