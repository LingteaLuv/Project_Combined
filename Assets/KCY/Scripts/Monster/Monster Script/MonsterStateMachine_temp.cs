using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine_temp
{
    // 상태 전이용
    public Dictionary<Estate, BaseState_temp> StateDic;

    public BaseState_temp CurState;

    public MonsterStateMachine_temp()
    {
        StateDic = new Dictionary<Estate, BaseState_temp>();
    }

    public void ChangeState(BaseState_temp changedState)
    {
        Debug.Log($"[상태 전이 시도] 현재 상태: {CurState.GetType().Name}, 변경 대상: {changedState.GetType().Name}");
        if (CurState == changedState) { Debug.Log("[상태 전이 무시] 현재 상태와 동일한 상태로 전이 시도됨");  return; }

        CurState.Exit();
        CurState = changedState;
        Debug.Log($"[상태 전이 완료] 새로운 상태: {CurState.GetType().Name}");
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
