using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 상태 전이를 관리하는 상태 머신입니다.
/// </summary>
public class PlayerStateMachine
{
    private PlayerState _currentState;

    public void Initialize(PlayerState initialState)
    {
        _currentState = initialState;
        _currentState.Enter();
    }

    /// <summary>
    /// 상태를 변경합니다.
    /// </summary>
    /// <param name="newState">전이할 새로운 상태</param>
    public void ChangeState(PlayerState newState)
    {
        if (_currentState == newState)
            return;
        _currentState?.Exit();  // null-safe
        _currentState = newState;
        _currentState.Enter();
    }

    /// <summary>
    /// 현재 상태의 Tick을 호출합니다.
    /// </summary>
    public void Update() => _currentState?.Tick();
    public void FixedUpdate() => _currentState?.FixedTick();
}