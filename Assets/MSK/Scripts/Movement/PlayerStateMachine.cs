using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// 플레이어의 현재 상태를 관리하고, 상태 전환 및 상태 업데이트를 제어하는 상태 머신입니다.
/// 각 상태는 PlayerStateBase를 상속받아 구현되며, 상태 전환 시 자동으로 Enter/Exit을 호출합니다.
/// </summary>
public class PlayerStateMachine
{
    private PlayerStateBase _currentState;
    public PlayerMovementState CurrentStateType { get; private set; }

    private readonly PlayerController _controller;

    public PlayerStateMachine(PlayerController controller)
    {
        _controller = controller;
    }

    /// <summary>
    /// 상태를 변경하고, 기존 상태를 종료(Exit)한 뒤 새 상태의 진입(Enter)을 호출합니다.
    /// 필요한 상태별 추가 정보(vaultTarget 등)는 조건에 따라 전달합니다.
    /// </summary>
    public void ChangeState(PlayerMovementState newState, Transform vaultTarget = null)
    {
        _currentState?.OnStateExit();

        switch (newState)
        {
            case PlayerMovementState.Vault:
                _currentState = new VaultState(this, _controller, vaultTarget);
                break;

            case PlayerMovementState.Idle:
                _currentState = new IdleState(this, _controller);
                break;

                // ...
        }

        CurrentStateType = newState;
        _currentState.OnStateEnter();
    }

    /// <summary>
    /// 현재 상태의 Update 로직을 호출합니다. 일반적으로 MonoBehaviour의 Update()에서 사용됩니다.
    /// </summary>
    public void Update()
    {
        _currentState?.OnStateUpdate();
    }
}
