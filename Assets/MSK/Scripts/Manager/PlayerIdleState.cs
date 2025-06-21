using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine fsm, PlayerMovement movement)
        : base(fsm, movement) { }

    public override void Enter()
    {
        Debug.Log("Enter Idle");
    }

    public override void Exit()
    {
        Debug.Log("Exit Idle");
    }

    public override void Tick()
    {
        // 입력이 있으면 이동 상태로 전이
        if (_movement.MoveInput.Value != Vector3.zero)
            _fsm.ChangeState(new PlayerMoveState(_fsm, _movement));

        if (Input.GetButtonDown("Jump") && _movement.IsGrounded)
        {
            _fsm.ChangeState(new PlayerJumpState(_fsm, _movement));
        }
    }
}