using UnityEngine;

/// <summary>
/// 플레이어가 웅크리는 상태입니다.
/// Ctrl 키를 누르면 진입하며, 떼면 일어납니다.
/// </summary>
public class PlayerCrouchState : PlayerState
{
    public PlayerCrouchState(PlayerStateMachine fsm, PlayerMovement movement)
        : base(fsm, movement) { }

    public override void Enter()
    {
        Debug.Log("Enter Crouch");

        _movement.Controller._animator.SetTrigger("CrouchDown");
        _movement.Controller._animator.SetBool("IsCrouch", true);
        _movement.SetCrouch(true);

    }

    public override void Exit()
    {
        Debug.Log("Exit Crouch");

        _movement.Controller._animator.SetTrigger("CrouchUp");
        _movement.Controller._animator.SetBool("IsCrouch", false);
        _movement.SetCrouch(false);
    }

    public override void Tick()
    {
        if (!_movement.CrouchHeld)
        {
            _fsm.ChangeState(_movement.MoveInput == Vector3.zero
                ? _movement.Controller.IdleState
                : _movement.Controller.MoveState);
            return;
        }
        if (_movement.MoveInput == Vector3.zero)
        {
            _fsm.ChangeState(_movement.Controller.IdleCrouchState);
            return;
        }
        if (HandleJumpTransition()) return;
    }

    public override void FixedTick()
    {
        _movement.Move(_movement.MoveInput);
    }
}
