using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcIdleState : NpcState
{
    public NpcIdleState(NpcStateMachine nfsm) { _Nfsm = nfsm; }

    public override void Enter()
    {
        Debug.Log("NPC Idle 상태 진입");
    }

    public override void Exit() { }
    public override void Tick() { }
    public override void FixedTick() { }
}
