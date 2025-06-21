/// <summary>
/// 플레이어의 움직임(행동) 상태를 구분하기 위한 열거형입니다.   
/// </summary>
public enum PlayerMovementState
{
    Idle,           // 아무 입력 없음, 대기 상태
    Move,           // WASD 방향 입력
    Jump,           // 점프키 입력 시 위로 상승
    Vault,          // 낮은 장애물을 부드럽게 넘는 상태 (파쿠르 동작)
    Climb,          // 사다리 감지 후 위아래 이동
    Interact,       // 아이템 줍기 등 상호작용
    Run,            // 이동 + 달리기 키
    Crouch,         // 숙이기 키 입력
    Stairs,         // 계단 감지 및 위치 보정
    DoorInteract,   // 문 상호작용
    Sleep,          // 특정 조건에서 잠자기
    Hit,            // 피격 시 애니메이션 및 잠시 제어
    Attack          // 공격 입력 시 실행, Hit 상태에 이벤트 전달 필요?
    // 매달리기 등의 상태 추가?
}

