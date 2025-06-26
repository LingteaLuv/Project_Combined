using UnityEngine;

/// <summary>
/// 플레이어가 점프 중일 때의 상태입니다.
/// 공중에 있는 동안 상태를 유지하며, 착지 시 Idle 또는 Move 상태로 전이됩니다.
/// </summary>
public class PlayerJumpState : PlayerState
{
    // 공중 유지 시간 (짧은 착지 방지용 지연)
    private float _hangTime = 0.95f;
    private float _hangTimer;

    public PlayerJumpState(PlayerStateMachine fsm, PlayerMovement movement)
        : base(fsm, movement) { }


    /// <summary>
    /// 상태 진입 시 점프 애니메이션 실행 및 실제 점프 수행
    /// </summary>
    public override void Enter()
    {
        _hangTimer = _hangTime;

        _movement.Controller.PlayJumpAnimation();
        _movement.Jump();
    }

    /// <summary>
    /// 상태 종료 시 호출됩니다.
    /// 현재는 점프 상태 종료 후 특별한 처리는 없음
    /// </summary>
    public override void Exit() { }
    public override void FixedTick()
    {
        _movement.HandleMovement(_movement.MoveInput);
    }

    /// <summary>
    /// 매 프레임마다 이동 및 착지 판정을 수행합니다.
    /// </summary>
    public override void Tick()
    {
        if (_hangTimer > 0f)
        {
            _hangTimer -= Time.deltaTime;
            return;
        }

        if (_movement.IsGrounded)
        {
            _fsm.ChangeState(_movement.MoveInput == Vector3.zero
                ? _movement.Controller.IdleState
                : _movement.Controller.MoveState);
        }
        if (EnterFallState())
        {
            _fsm.ChangeState(_movement.Controller.FallState);
            return;
        }
    }
}

