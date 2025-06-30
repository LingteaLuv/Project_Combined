using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcIdleState : NpcState
{
    public NpcIdleState(NpcStateMachine nsm) { _Nsm = nsm; }

    public override void Enter()
    {
        Debug.Log("NPC Idle 상태 진입");
    }

    public override void Exit() { }
    public override void Tick() { }
    public override void FixedTick() { }
}
