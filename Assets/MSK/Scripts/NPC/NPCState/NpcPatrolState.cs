using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPatrolState : NpcState
{
    public NpcPatrolState(NpcStateMachine nfsm) { _Nfsm = nfsm; }

    public override void Enter() { /* ... */ }
    public override void Exit() { /* ... */ }
    public override void Tick() { /* ... */ }
    public override void FixedTick() { /* ... */ }
}