using UnityEngine;

/// <summary>
/// 입력을 감지하고 외부로 전달하는 클래스입니다.
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    public Vector3 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }

    // 점프 입력 → 내부 플래그로 변경

    private void Update()
    {
        MoveInput = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0f,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        JumpPressed = Input.GetButtonDown("Jump");
    }
}