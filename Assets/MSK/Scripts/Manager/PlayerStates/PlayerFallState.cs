using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;
using UnityEngine.WSA;

/// <summary>
/// 점프 없이 낙하 중인 상태입니다.
/// </summary>
public class PlayerFallState : PlayerState
{
    private float _airTime;
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
        _airTime = 0;
        _wasGrounded = false;
    }

    public override void Exit()
    {
        _movement.Controller._animator.SetBool("IsFalling", false);
    }

    public override void Tick()
    {
        bool isGrounded = _movement.IsGrounded;

        // 채공 시간 누적
        if (!isGrounded)
        {
            _airTime += Time.deltaTime;
        }

        // 착지한 순간 감지
        if (_wasGrounded == false && isGrounded)
        {
            _currentY = _movement.transform.position.y;
            float fallDistance = _lastYPos - _currentY;
            ApplyFallDamage(fallDistance);

            _airTime = 0;
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

    private void ApplyFallDamage(float distance)
    {
        float safeHeight = 3.0f;
        float dangerHeight = 8.0f;
        float damagePerMeter = 10.0f;

        int damage = 0;

        if (distance > dangerHeight)
            damage = 999;
        else if (distance > safeHeight)
            damage = Mathf.RoundToInt((distance - safeHeight) * damagePerMeter);

        if (damage > 0)
        {
            Debug.Log($"낙하 데미지 발생: {damage}");
            _movement.Controller.HitState.SetDamage(damage);
            _fsm.ChangeState(_movement.Controller.HitState);
        }
    }

}
