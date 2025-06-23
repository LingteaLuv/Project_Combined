using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine_temp 
{
    // 상태 전이용
    public Dictionary<Estate, BaseState_temp> StateDic;

    public BaseState_temp CurState;

    public void ChangeState(BaseState_temp changedState)
    {
        if (CurState == changedState) { return; }

        CurState.Exit();
        CurState = changedState;
        CurState.Enter();
    }
    public void Update()
    {
        CurState.Update();
    }
    public void FixedUpdate()
    {
        if (CurState.HasPhysics == true)
        {
            CurState.FixedUpdate();
        }
    }

}
