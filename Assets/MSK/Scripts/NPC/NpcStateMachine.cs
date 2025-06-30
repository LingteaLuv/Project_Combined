using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStateMachine
{
    private NpcState _currentState;

    public void Initialize(NpcState initialState)
    {
        _currentState = initialState;
        _currentState.Enter();
    }

    public void ChangeState(NpcState newState)
    {
        if (_currentState == newState)
            return;
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Update() => _currentState?.Tick();
    public void FixedUpdate() => _currentState?.FixedTick();
}