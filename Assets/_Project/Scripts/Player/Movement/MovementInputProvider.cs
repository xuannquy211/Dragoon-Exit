using UnityEngine;

public class MovementInputProvider : MonoBehaviour
{
    public FloatingJoystick joystick;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    public Vector2 GetMoveInput()
    {
/*#if UNITY_EDITOR
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        return new Vector2(x, y);
#elif UNITY_ANDROID*/
        return joystick.Direction;
//#endif
    }

    public bool IsRunning()
    {
/*#if UNITY_EDITOR
        return Input.GetKey(KeyCode.LeftShift);
#elif UNITY_ANDROID*/
        return joystick.Direction.y > 0.9f;
//#endif
    }
}