using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 점프 없이 낙하 중인 상태입니다.
/// </summary>
public class PlayerFallState : PlayerState
{
    public PlayerFallState(PlayerStateMachine fsm, PlayerMovement movement)
        : base(fsm, movement) { }

    public override void Enter()
    {
        Debug.Log("Enter Fall");
        _movement.Controller._animator.SetBool("IsFalling", true);
    }

    public override void Exit()
    {
        Debug.Log("Exit Fall");
        _movement.Controller._animator.SetBool("IsFalling", false);
    }

    public override void Tick()
    {
        // 착지하면 상태 전이
        if (_movement.IsGrounded)
        {
            PlayerState nextState = _movement.MoveInput == Vector3.zero
                ? _movement.Controller.IdleState
                : _movement.Controller.MoveState;

            _fsm.ChangeState(nextState);
        }
    }

    public override void FixedTick()
    {
        _movement.Move(_movement.MoveInput); // 공중 이동 가능
    }
}