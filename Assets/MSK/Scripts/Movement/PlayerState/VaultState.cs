using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vault 상태: 낮은 장애물을 넘는 파쿠르 상태입니다.
/// vaultTarget까지 이동하며 애니메이션을 재생하고, 도달하면 Idle 상태로 전환됩니다.
/// </summary>
public class VaultState : PlayerStateBase
{
    private readonly int _vaultHash = Animator.StringToHash("isVault");
    private readonly float _duration;
    private float _elapsed;

    public VaultState(PlayerController controller, float duration = 1.0f) : base(controller)
    {
        _duration = duration;
    }

    /// <summary>
    /// Vault 상태 진입 시 호출됩니다. 애니메이션 재생과 중력 비활성화 처리.
    /// </summary>
    public override void OnEnter()
    {
        _elapsed = 0f;
        _controller.Animator.SetBool(_vaultHash, true);
    }

    /// <summary>
    /// Vault 상태 유지 중 호출됩니다. vaultTarget까지의 이동을 처리합니다.
    /// </summary>
    public override void OnUpdate()
    {
        _elapsed += Time.deltaTime;

        if (_elapsed >= _duration)
        {
            _controller.Animator.SetBool(_vaultHash, false);
            _controller.StateMachine.ChangeState(new IdleState(_controller));
        }
    }

    /// <summary>
    /// Vault 상태 종료 시 호출됩니다. 중력을 다시 활성화합니다.
    /// </summary>
    public override void OnFixedUpdate()
    {
        // 이 상태에선 움직임 차단
        _controller.PlayerMovement.SetMove(Vector3.zero);
    }
}