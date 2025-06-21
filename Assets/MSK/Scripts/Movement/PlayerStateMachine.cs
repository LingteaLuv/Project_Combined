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
    public PlayerStateBase CurrentState { get; private set; }

    /// <summary>
    /// 상태를 새로운 상태로 전환합니다.
    /// </summary>
    /// <param name="newState">전환할 상태 인스턴스</param>
    public void ChangeState(PlayerStateBase newState)
    {
        CurrentState?.OnExit();
        CurrentState = newState;
        CurrentState?.OnEnter();
    }

    /// <summary>
    /// 현재 상태의 Update 루프를 호출합니다.
    /// </summary>
    public void Update()
    {
        CurrentState?.OnUpdate();
    }

    /// <summary>
    /// 현재 상태의 FixedUpdate 루프를 호출합니다.
    /// </summary>
    public void FixedUpdate()
    {
        CurrentState?.OnFixedUpdate();
    }
}
