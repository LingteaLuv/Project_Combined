using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 아무 입력도 하지 않을 때의 기본 대기 상태입니다.
/// </summary>
public class IdleState : PlayerStateBase
{
    public IdleState(PlayerController controller) : base(controller) { }


    /// <summary>
    /// 매 프레임 Idle 상태를 유지합니다.
    /// </summary>
    public override void OnUpdate()
    {
        Vector3 inputDir = _controller.PlayerInput.GetInputDir();

        if (inputDir != Vector3.zero)
        {
            _controller.StateMachine.ChangeState(new MoveState(_controller));
        }

        _controller.Animator.SetFloat("movementValue", 0f, 0.2f, Time.deltaTime);
    }
}
