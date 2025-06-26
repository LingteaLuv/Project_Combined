using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Idle : MonsterState_temp
{
    public Monster_Idle(Monster_temp _monster) : base(_monster) 
    {
        _navMeshAgent = monster.MonsterAgent;
        stateMachine = monster._monsterMerchine;
    }

    // idle 상태에서 잠시 대기
    private float _idleTimer;
    private float _idleDuration = 3f;

    private NavMeshAgent _navMeshAgent;
    protected MonsterStateMachine_temp stateMachine;
 

    public override void Enter()
    {
        _idleTimer = 0f;
    }

    public override void Update()
    {
        Debug.Log("idle MOde");
        // 쿨타임이 지나면 다시 패트롤 모드로 돌아가기
        _idleTimer += Time.deltaTime;
        Debug.Log($"[Idle] 상태 Update() 호출됨 - 경과시간: {_idleTimer:F2}");
        if (_idleTimer >= _idleDuration)
        {
            Debug.Log("[Idle] 타이머 종료 → Patrol 상태로 전이");
            stateMachine.ChangeState(stateMachine.StateDic[Estate.Patrol]);
        }
    }
    public override void Exit()
    {
        // 내비로 안힌 추적 재활성화
        _navMeshAgent.enabled = true;
    }
}
