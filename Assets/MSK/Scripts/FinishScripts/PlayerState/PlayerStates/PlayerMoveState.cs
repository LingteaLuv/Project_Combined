using UnityEngine;

/// <summary>
/// 플레이어가 이동 중인 상태입니다.
/// </summary>
public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerStateMachine fsm, PlayerMovement movement)
        : base(fsm, movement) { }

    public override void Enter()
    {
        _movement.SetStateColliderRadius(12f);
        if (_movement.Controller.IsCrouch)
        {
            _movement.Controller.IsCrouch = false;
        }

    }

    public override void Exit() { }
    public override void Tick()
    {
        if (_movement.IsRunning && _movement.CanRun)
            _movement.Controller._animator.SetBool("IsRunning", true);
        else
            _movement.Controller._animator.SetBool("IsRunning", false);

        if (_movement.MoveInput == Vector3.zero)
        {
            _fsm.ChangeState(_movement.Controller.IdleState);
            return;
        }
        if (_movement.CrouchHeld)
        {
            _fsm.ChangeState(_movement.Controller.CrouchState);
            _movement.SetCrouch(true);
            return;
        }
        if (_movement.CrouchHeld && _movement.MoveInput != Vector3.zero)
        {
            _fsm.ChangeState(_movement.Controller.IdleCrouchState);
            _movement.SetCrouch(true);
            return;
        }
        if (_movement.IsOnLadder)
        {
            _fsm.ChangeState(_movement.Controller.ClimbState);
            return;
        }
        if (EnterFallState())
        {
            _fsm.ChangeState(_movement.Controller.FallState);
            return;
        }
        if (HandleJumpTransition()) return;
    }
    public override void FixedTick()
    {
        _movement.HandleMovement(_movement.MoveInput);
    }
}