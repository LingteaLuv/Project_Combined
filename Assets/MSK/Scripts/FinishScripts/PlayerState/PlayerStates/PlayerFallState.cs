using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;

/// <summary>
/// 점프 없이 낙하 중인 상태입니다.
/// </summary>
public class PlayerFallState : PlayerState
{
    private float _lastYPos;
    private float _currentY;
    private bool _wasGrounded;

    public PlayerFallState(PlayerStateMachine fsm, PlayerMovement movement)
            : base(fsm, movement) { }

    public override void Enter()
    {
        Debug.Log("낙하상태 진입");
        _movement.Controller._animator.SetBool("IsFalling", true);
        _lastYPos = _movement.transform.position.y;
        _wasGrounded = false;
    }

    public override void Exit()
    {
        _movement.Controller._animator.SetBool("IsFalling", false);
    }

    public override void Tick()
    {
        bool isGrounded = _movement.IsGrounded;

        // 착지한 순간 감지
        if (_wasGrounded == false && isGrounded)
        {
            _currentY = _movement.transform.position.y;
            float fallDistance = _lastYPos - _currentY;
            _movement.Property.ApplyFallDamage(fallDistance);
        }

        // 상태 전이 ,Hit 중이면 전이 막기
        if (_movement.IsGrounded && !_movement.Controller.IsInHit) 
        {
            PlayerState nextState = _movement.MoveInput == Vector3.zero
                ? _movement.Controller.IdleState
                : _movement.Controller.MoveState;

            _fsm.ChangeState(nextState);
        }
        _wasGrounded = isGrounded;
    }

    public override void FixedTick()
    {
        _movement.HandleMovement(_movement.MoveInput);
    }
}
