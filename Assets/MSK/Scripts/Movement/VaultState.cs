using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vault 상태: 낮은 장애물을 넘는 파쿠르 상태입니다.
/// vaultTarget까지 이동하며 애니메이션을 재생하고, 도달하면 Idle 상태로 전환됩니다.
/// </summary>
public class VaultState : PlayerStateBase
{
    private Animator _animator;
    private Transform _playerTransform;
    private Transform _vaultTarget;

    public VaultState(PlayerStateMachine fsm, PlayerController controller, Transform vaultTarget) : base(fsm, controller)
    {
        _animator = controller.Animator;
        _playerTransform = controller.transform;
        _vaultTarget = vaultTarget;
    }

    /// <summary>
    /// Vault 상태 진입 시 호출됩니다. 애니메이션 재생과 중력 비활성화 처리.
    /// </summary>
    public override void OnStateEnter()
    {
        _animator.SetTrigger("Vault");
        _controller.Rigidbody.useGravity = false;
        _controller.Rigidbody.velocity = Vector3.zero;
    }

    /// <summary>
    /// Vault 상태 유지 중 호출됩니다. vaultTarget까지의 이동을 처리합니다.
    /// </summary>
    public override void OnStateUpdate()
    {
        _playerTransform.position = Vector3.MoveTowards(
            _playerTransform.position,
            _vaultTarget.position,
            3.5f * Time.deltaTime
        );

        float distance = Vector3.Distance(_playerTransform.position, _vaultTarget.position);
        if (distance < 0.1f)
        {
            _fsm.ChangeState(PlayerMovementState.Idle);
        }
    }

    /// <summary>
    /// Vault 상태 종료 시 호출됩니다. 중력을 다시 활성화합니다.
    /// </summary>
    public override void OnStateExit()
    {
        _controller.Rigidbody.useGravity = true;
    }
}