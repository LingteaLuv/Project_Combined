using UnityEngine;

/// <summary>
/// 플레이어가 피격되었을 때 실행되는 상태입니다.
/// 데미지 적용, 피격 애니메이션 재생, 사망 판정까지 수행합니다.
/// </summary>
public class PlayerHitState : PlayerState
{
    private float _timer;
    public PlayerHitState(PlayerStateMachine fsm, PlayerMovement movement)
        : base(fsm, movement) { }

    public override void Enter() {   }

    public override void Exit()   {    }

    public override void FixedTick() { }

    public override void Tick()
    {
        var controller = _movement?.Controller;
        if (_timer <= 0f)
        {
            _fsm.ChangeState(_movement.MoveInput == Vector3.zero
                ? controller.IdleState
                : controller.MoveState);
        }
    }
}