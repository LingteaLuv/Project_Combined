using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 플레이어 상태의 추상 기반 클래스입니다.
/// </summary>
public abstract class PlayerState
{
    protected PlayerStateMachine _fsm;
    protected PlayerMovement _movement;
    protected float _fallTimer;

    /// <summary>
    /// 상태 생성자
    /// </summary>
    /// <param name="fsm">상태머신 인스턴스</param>
    /// <param name="movement">이동 관련 기능을 가진 객체</param>
    protected PlayerState(PlayerStateMachine fsm, PlayerMovement movement)
    {
        _fsm = fsm;
        _movement = movement;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Tick();
    public abstract void FixedTick();
    /// <summary>
    /// 점프 입력이 가능하면 점프 처리 후 true 반환
    /// </summary>
    protected bool HandleJumpTransition()
    {
        if (_movement.CanJump())
        {
            _fsm.ChangeState(_movement.Controller.JumpState);
            return true;
        }
        return false;
    }
    protected bool EnterFallState(float delay = 0.3f)
    {
        if (!_movement.IsGrounded && _movement.Rigidbody.velocity.y < -0.1f)
        {
            _fallTimer += Time.deltaTime;
            return _fallTimer >= delay;
        }

        _fallTimer = 0f;
        return false;
    }
}

