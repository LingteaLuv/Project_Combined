using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 입력 방향으로 이동하는 상태입니다.
/// </summary>
public class MoveState : PlayerStateBase
{
    public MoveState(PlayerController controller) : base(controller) { }

    public override void OnUpdate()
    {
        Vector3 inputDir = _controller.PlayerInput.GetInputDir();

        if (inputDir == Vector3.zero)
        {
            _controller.StateMachine.ChangeState(new IdleState(_controller));
            return;
        }

        _controller.PlayerMovement.Rotate();
        _controller.PlayerMovement.SetMove(inputDir);

        float movementAmount = Mathf.Clamp01(inputDir.magnitude);
        _controller.Animator.SetFloat("movementValue", movementAmount, 0.2f, Time.deltaTime);
    }
}