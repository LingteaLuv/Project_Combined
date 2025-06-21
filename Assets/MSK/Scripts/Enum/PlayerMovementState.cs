/// <summary>
/// 플레이어의 움직임(행동) 상태를 구분하기 위한 열거형입니다.   
/// </summary>
public enum PlayerMovementState
{
    Idle,           // 기본 대기 상태
    Move,           // 카메라 기준으로 이동
    Jump,           // 점프 중
    Vault,          // 낮은 장애물을 부드럽게 넘는 상태 (파쿠르 동작)
    Climb,          // 사다리 등반 중
    Interact,        // 문 열기, 아이템 줍기 등 상호작용

    // 매달리기 등의 상태 추가?
}

