using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine _fsm;
    protected PlayerMovement _movement;

    public PlayerState(PlayerStateMachine fsm, PlayerMovement movement)
    {
        _fsm = fsm;
        _movement = movement;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Tick(); // 또는 FixedTick 등
}
