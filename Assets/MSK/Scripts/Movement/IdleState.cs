using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 아무 입력도 하지 않을 때의 기본 대기 상태입니다.
/// 일반적으로 Vault 이후 복귀 상태로 사용되며, 필요 시 애니메이션 처리도 가능합니다.
/// </summary>
public class IdleState : PlayerStateBase
{
    public IdleState(PlayerStateMachine fsm, PlayerController controller)
        : base(fsm, controller) { }

    /// <summary>
    /// Idle 상태 진입 시 초기화 작업을 수행합니다. (예: Idle 애니메이션 트리거)
    /// </summary>
    public override void OnStateEnter()
    {
        // _controller.Animator.SetTrigger("Idle"); ← 필요 시 활성화
    }

    /// <summary>
    /// 매 프레임 Idle 상태를 유지합니다.
    /// 입력이 들어올 경우 Move 상태로 전환하는 로직은 추후 확장 예정입니다.
    /// </summary>
    public override void OnStateUpdate()
    {
        // 추후 입력 감지 시 Move 등으로 전환 가능
    }

    /// <summary>
    /// Idle 상태 종료 시 정리 작업을 수행합니다.
    /// </summary>
    public override void OnStateExit()
    {
        // 필요 시 무빙 트리거 등 초기화
    }
}
