using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerStateMachine fsm, PlayerMovement movement)
        : base(fsm, movement) { }

    public override void Enter() => Debug.Log("Enter Move");
    public override void Exit() => Debug.Log("Exit Move");

    public override void Tick()
    {
        if (_movement.MoveInput.Value == Vector3.zero)
            _fsm.ChangeState(new PlayerIdleState(_fsm, _movement));

        if (Input.GetButtonDown("Jump") && _movement.IsGrounded)
        {
            _fsm.ChangeState(new PlayerJumpState(_fsm, _movement));
        }
        // 실제 이동 처리 함수 호출
        _movement.Move(_movement.MoveInput.Value);
    }
}
