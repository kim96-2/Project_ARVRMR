using UnityEngine;
using UnityEngine.InputSystem;

public class DisableJoystickInput : MonoBehaviour
{
    public InputActionProperty moveAction; // 조이스틱 이동 액션

    public void DisableJoystick()
    {
        moveAction.action.Disable(); // 액션 비활성화
    }

    public void EnableJoystick()
    {
        moveAction.action.Enable(); // 액션 활성화
    }
}
