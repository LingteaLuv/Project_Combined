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
        _movement.SetStateColliderRadius(2f);
        if (_movement.Controller.IsCrouch)
        {
            _movement.Controller.IsCrouch = false;
        }
    }
    public override void Exit() { }
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
            _movement.SetCrouch(true);
            return;
        }
        if (_movement.CrouchHeld && _movement.MoveInput != Vector3.zero)
        {
            _fsm.ChangeState(_movement.Controller.IdleCrouchState);
            _movement.SetCrouch(true);
            return;
        }
        if (_movement.InteractPressed)
        {
            _fsm.ChangeState(_movement.Controller.InteractState);
            return;
        }
        if (EnterFallState())
        {
            _fsm.ChangeState(_movement.Controller.FallState);
            return;
        }
        if (HandleJumpTransition()) return;
    }
}

