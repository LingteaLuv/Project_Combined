using UnityEngine;

/// <summary>
/// 점프 상태: 점프 입력에 의해 진입되고, 공중에 떠 있는 동안 유지됨
/// </summary>
public class PlayerJumpState : PlayerState
{
    private bool _hasJumped = false;

    public PlayerJumpState(PlayerStateMachine fsm, PlayerMovement movement)
        : base(fsm, movement) { }

    public override void Enter()
    {
        Debug.Log("Enter Jump");

        if (_movement.IsGrounded)
        {
            _hasJumped = true;
            Vector3 velocity = _movement.Rigidbody.velocity;
            velocity.y = _movement.JumpForce;
            _movement.Rigidbody.velocity = velocity;
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit Jump");
        _hasJumped = false;
    }

    public override void Tick()
    {
        if (_movement.IsGrounded && _hasJumped)
        {
            _fsm.ChangeState(new PlayerIdleState(_fsm, _movement));
        }

        // 공중에서 수평 이동 허용
        _movement.Move(_movement.MoveInput.Value);
    }
}