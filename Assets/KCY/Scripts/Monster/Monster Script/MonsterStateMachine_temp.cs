using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine_temp
{
    // 상태 전이를 위한 중심 기능
    public Dictionary<Estate, BaseState_temp> StateDic;

    // 현 상태 저장
    public BaseState_temp CurState;

    public MonsterStateMachine_temp()
    {
        StateDic = new Dictionary<Estate, BaseState_temp>();
    }

    // 상태 전이
    public void ChangeState(BaseState_temp changedState)
    {
        Debug.Log($"[상태 전이 시도] 현재 상태: {CurState.GetType().Name}, 변경 대상: {changedState.GetType().Name}");
        if (CurState == changedState) { Debug.Log("현재랑 같은 씬 - 무시");  return; }
        
        // 나간다 - 들어간다 - 들어가서 할 작업 실행
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
