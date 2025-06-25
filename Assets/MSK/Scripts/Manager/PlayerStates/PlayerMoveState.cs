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
        if (_movement.Controller.IsCrouch)
        {
            _movement.Controller._animator.SetTrigger("CrouchUp");
            _movement.Controller.IsCrouch = false;
        }
    }

    public override void Exit() { Debug.Log("Exit Move"); }
    public override void Tick()
    {
        if (_movement.MoveInput == Vector3.zero)
        {
            _fsm.ChangeState(_movement.Controller.IdleState);
            return;
        }
        if (_movement.CrouchHeld)
        {
            _fsm.ChangeState(_movement.Controller.CrouchState);
            return;
        }
        if (_movement.CrouchHeld && _movement.MoveInput != Vector3.zero)
        {
            _fsm.ChangeState(_movement.Controller.IdleCrouchState);
            return;
        }
        if (_movement.IsOnLadder)
        {
            _fsm.ChangeState(_movement.Controller.ClimbState);
            return;
        }
        if (HandleJumpTransition()) return;
    }
    public override void FixedTick()
    {
        _movement.Move(_movement.MoveInput);
    }
}