using UnityEngine;

/// <summary>
/// 플레이어가 웅크리고 정지한 상태입니다.
/// </summary>
public class PlayerIdleCrouchState : PlayerState
{
    public PlayerIdleCrouchState(PlayerStateMachine fsm, PlayerMovement movement)
        : base(fsm, movement) { }

    public override void Enter()
    {
        Debug.Log("Enter IdleCrouch");
        _movement.Controller._animator.SetTrigger("CrouchDown");
        _movement.Controller._animator.SetBool("IsCrouch", true);
        _movement.SetCrouch(true);
    }

    public override void Exit()
    {
        Debug.Log("Exit IdleCrouch");
        _movement.Controller._animator.SetTrigger("CrouchUp");
        _movement.Controller._animator.SetBool("IsCrouch", false);
        _movement.SetCrouch(false);
    }

    public override void Tick()
    {
        if (!_movement.CrouchHeld)
        {
            _fsm.ChangeState(_movement.Controller.IdleState);
            return;
        }

        if (_movement.MoveInput != Vector3.zero)
        {
            _fsm.ChangeState(_movement.Controller.CrouchState);
            return;
        }

        if (HandleJumpTransition()) return;
    }

    public override void FixedTick()
    {
        // 정지 상태라 이동 없음
    }
}
