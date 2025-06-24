using System;
using UnityEngine;

/// <summary>
/// 플레이어가 가만히 서 있는 상태입니다.
/// </summary>
public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine fsm, PlayerMovement movement)
        : base(fsm, movement) { }

    public override void Enter() 
    { 
        Debug.Log("Enter Idle");
        if (_movement.Controller.IsCrouch)
        {
            _movement.Controller._animator.SetTrigger("CrouchUp");
            _movement.Controller.IsCrouch = false;
        }
    }
    public override void Exit() { Debug.Log("Exit Idle"); }
    public override void FixedTick() { }
    public override void Tick()
    {
        if (_movement.MoveInput != Vector3.zero)
        {
            _fsm.ChangeState(_movement.Controller.MoveState);
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
        if (_movement.InteractPressed)
        {
            _fsm.ChangeState(_movement.Controller.InteractState);
            return;
        }

        if (HandleJumpTransition()) return;
    }
}

