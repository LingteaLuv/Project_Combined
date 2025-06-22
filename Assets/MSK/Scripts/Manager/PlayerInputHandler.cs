using UnityEngine;

/// <summary>
/// 입력을 감지하고 외부로 전달하는 클래스입니다.
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    public Vector3 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool CrouchHeld { get; private set; }

    private void Update()
    {
        MoveInput = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0f,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        JumpPressed = Input.GetButtonDown("Jump");
        CrouchHeld = Input.GetKey(KeyCode.LeftControl);
    }
}