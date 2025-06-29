using UnityEngine;
using UnityEngine.AI;

public class Monster_Reset : MonsterState_temp
{
    public Monster_Reset(Monster_temp _monster) : base(_monster)
    {
        _agent = monster.MonsterAgent;
        _ani = monster.Ani;
        stateMachine = monster._monsterMerchine;
    }

    private NavMeshAgent _agent;
    private Animator _ani;
    private float _resetTimer = 0f;
    private float _resetWaitTime = 1.0f; // Idle로 돌아가기 전 대기시간
    protected MonsterStateMachine_temp stateMachine;

    public override void Enter()
    {
        Debug.Log("[Reset] 상태 진입");

        _resetTimer = 0f;

        // 애니메이션 초기화
        _ani.ResetTrigger("Attack");
        _ani.SetBool("isChasing", false);
        _ani.SetBool("isPatrol", false);
        _ani.SetBool("isDead", false);

        // NavMeshAgent 초기화
        if (_agent != null)
        {
            _agent.ResetPath();
            _agent.isStopped = true;
        }

        // NavMeshAgent가 NavMesh 위에 없으면 보정
        if (_agent != null && !_agent.isOnNavMesh)
        {
            if (NavMesh.SamplePosition(monster.transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                monster.transform.position = hit.position;
                Debug.Log("[Reset] NavMesh 보정 완료");
            }
            else
            {
                Debug.LogError("[Reset] NavMesh 보정 실패");
            }
        }
    }

    public override void Update()
    {
        if (stateMachine.CurState != this) return;

        _resetTimer += Time.deltaTime;

        // 감지 중이면 바로 Chase 복귀
        if (monster.IsDetecting && monster.TargetPosition != null)
        {
            Debug.Log("[Reset] 탐지 유지 중 → Chase 복귀");
            stateMachine.ChangeState(stateMachine.StateDic[Estate.Chase]);
            return;
        }

        // 일정 시간 후 Idle 복귀
        if (_resetTimer >= _resetWaitTime)
        {
            Debug.Log("[Reset] 대기 후 Idle 복귀");
            stateMachine.ChangeState(stateMachine.StateDic[Estate.Idle]);
        }
    }

    public override void Exit()
    {
        Debug.Log("[Reset] 상태 종료");
        // 상태 복귀 시 필요한 정리는 Idle 등에서 다시 수행
    }
}
