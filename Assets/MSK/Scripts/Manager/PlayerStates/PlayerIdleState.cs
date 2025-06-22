using UnityEngine;

/// <summary>
/// 플레이어가 가만히 서 있는 상태입니다.
/// </summary>
public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine fsm, PlayerMovement movement)
        : base(fsm, movement) { }

    public override void Enter() { Debug.Log("Enter Idle"); }
    public override void Exit() { Debug.Log("Exit Idle"); }
    public override void FixedTick() { }
    public override void Tick()
    {
        if (_movement.MoveInput != Vector3.zero)
        {
            _fsm.ChangeState(_movement.Controller.MoveState);
            return;
        }
        if (_movement.Input.CrouchHeld)
        {
            _fsm.ChangeState(_movement.Controller.CrouchState);
            return;
        }
        if (HandleJumpTransition()) return;
    }
}

