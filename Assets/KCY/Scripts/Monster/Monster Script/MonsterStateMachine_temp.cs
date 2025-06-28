using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MonsterStateMachine_temp
{
    public Dictionary<Estate, BaseState_temp> StateDic;
    public BaseState_temp CurState;
    public BaseState_temp PrevState;

    public MonsterStateMachine_temp()
    {
        StateDic = new Dictionary<Estate, BaseState_temp>();
    }

    public void ChangeState(BaseState_temp changedState)
    {
        UnityEngine.Debug.Log($"[상태 전이 시도] 현재: {CurState?.GetType().Name}, 변경 대상: {changedState?.GetType().Name}");
        UnityEngine.Debug.Log($"[상태 전이 시도] 현재: {CurState?.GetType().Name}, 변경 대상: {changedState?.GetType().Name}");
        UnityEngine.Debug.Log($"[상태 전이 완료] 현재: {CurState?.GetType().Name}, 이전: {PrevState?.GetType().Name}");


        if (CurState == changedState)
        {
            UnityEngine.Debug.LogWarning("[상태 전이 무시] 동일 상태");
            return;
        }

        if (CurState != null)
        {
            PrevState = CurState;
            CurState.Exit();
        }

        CurState = changedState;
        CurState?.Enter();

        UnityEngine.Debug.Log($"[상태 전이 완료] 현재: {CurState?.GetType().Name}, 이전: {PrevState?.GetType().Name}");
    }

    public void Update()
    {
        CurState?.Update();
    }

    public void FixedUpdate()
    {
        if (CurState?.HasPhysics == true)
        {
            CurState.FixedUpdate();
        }
    }
}
