using UnityEngine;


/// <summary>
/// 입력을 감지해 외부로 전달
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    // 이동 입력 값
    public Property<Vector3> MoveInput { get; private set; } = new(Vector3.zero);

    /// <summary>
    /// 프레임마다 입력 감지 및 값 업데이트
    /// </summary
    private void Update()
    {
        Vector3 input = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0f,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        MoveInput.Value = input;
    }
}