using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FSM 기반 상태 클래스들의 공통 베이스 클래스입니다.
/// 각 상태 클래스는 이 클래스를 상속받아 상태 전환 시 로직(Enter/Update/Exit)을 구현합니다.
/// </summary>
public abstract class PlayerStateBase
{
    protected PlayerStateMachine _fsm;
    protected PlayerController _controller;

    /// <summary>
    /// 상태 생성자: FSM과 PlayerController 참조를 전달받습니다.
    /// </summary>
    /// <param name="fsm">현재 상태 머신</param>
    /// <param name="controller">플레이어 컨트롤러 참조</param>
    public PlayerStateBase(PlayerStateMachine fsm, PlayerController controller)
    {
        _fsm = fsm;
        _controller = controller;
    }

    /// <summary>
    /// 상태 진입 시 호출됩니다. 초기화, 애니메이션 트리거 등을 설정합니다.
    /// </summary>
    public virtual void OnStateEnter() { }

    /// <summary>
    /// 매 프레임 상태 유지 로직을 처리합니다. 이동, 상태 감지 등.
    /// </summary>
    public virtual void OnStateUpdate() { }

    /// <summary>
    /// 상태 종료 시 호출됩니다. 정리, 상태 초기화 등을 수행합니다.
    /// </summary>
    public virtual void OnStateExit() { }
}
