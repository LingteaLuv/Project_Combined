using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbState : PlayerState
{
    public PlayerClimbState(PlayerStateMachine fsm, PlayerMovement movement) : base(fsm, movement) { }

    /// <summary>
    /// 상태 진입 시 사다리 타기 애니메이션 실행, 중력 적용x
    /// </summary>
    public override void Enter()
    {
        Debug.Log("Enter Climb");
        _movement.SetGravity(false);
        _movement.Controller.PlayClimbAnimation();
        _movement.Rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
    }

    /// <summary>
    /// 상태 종료시 중력 적용o
    /// </summary>
    public override void Exit()
    {
        Debug.Log("Exit Climb");
        _movement.SetGravity(true);
        _movement.Controller.StopClimbAnimation();
        _movement.Rigidbody.constraints = RigidbodyConstraints.None;
        _movement.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        
    }
    public override void FixedTick() { }

    /// <summary>
    /// 매 프레임마다 호출
    /// </summary>
    public override void Tick()
    {
        if (_movement.IsOnLadder)
        {
            _movement.PlayerClimbHandler.Climb(_movement.IsGrounded);
            _movement.Controller.SetAnimatorSpeed();
        }
        else
        {
            _fsm.ChangeState(_movement.MoveInput == Vector3.zero
                ? _movement.Controller.IdleState
                : _movement.Controller.MoveState);
        }
    }
}
