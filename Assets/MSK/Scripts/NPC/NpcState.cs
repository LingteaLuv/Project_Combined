using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NpcState
{
    protected NpcStateMachine _Nfsm;
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Tick();
    public abstract void FixedTick();
}
