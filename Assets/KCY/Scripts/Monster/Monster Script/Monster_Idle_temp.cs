using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Idle : MonsterState_temp
{
    public Monster_Idle(Monster_temp _monster) : base(_monster)
    {
        //HearingRange = monster.SoundCol.radius;
        _navMeshAgent = monster.MonsterAgent;
        stateMachine = monster._monsterMerchine;
        _ani = monster.Ani;
        _info = monster.Info;
    }

    private MonsterInfo _info;
    private Animator _ani;
    private float _idleTimer;
    private float _idleDuration = 1.5f;

    // 배율값
    private float DeactivateHearing => _info.DeactivateHearing;
    private NavMeshAgent _navMeshAgent;
    protected MonsterStateMachine_temp stateMachine;


    public override void Enter()
    {
        monster.HearingCol.radius = monster.HearingRange * DeactivateHearing;

        monster.MonsterAgent.autoBraking = true;
        _ani.ResetTrigger("Attack");
        _idleTimer = 0f;

        if (_navMeshAgent != null && _navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.ResetPath();
            _navMeshAgent.isStopped = true;
            _navMeshAgent.velocity = Vector3.zero;

            monster.IsDetecting = false;
            monster.TargetPosition = null;

            //Debug.Log("아이들 모드 정지");
        }
        else
        {
            //Debug.LogWarning("[Idle] NavMeshAgent가 NavMesh 위에 없거나 null입니다!");
        }
        
    }


    public override void Update()
    {
        //Debug.Log("idle MOde");
        _idleTimer += Time.deltaTime;
        //Debug.Log($"[Idle] 상태 Update() 호출됨 - 경과시간: {_idleTimer:F2}");
        if (_idleTimer >= _idleDuration)
        {
            //Debug.Log("[Idle] 타이머 종료 → Patrol 상태로 전이");
            stateMachine.ChangeState(stateMachine.StateDic[Estate.Patrol]);
        }
    }

    public override void Exit()
    {

        monster.HearingCol.radius = monster.HearingRange;

        if (_navMeshAgent != null && _navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.ResetPath();
            //Debug.Log("idle 나감");
            monster.Ani.SetBool("isPatrol", true);
            _ani.ResetTrigger("Attack");
        }
        else
        {
            //Debug.Log("idle 나갈때 뭔가 잘못됬다 확인해라");
        }

        //monster.HearingCol.radius = _info.HearingRange;
    }
}