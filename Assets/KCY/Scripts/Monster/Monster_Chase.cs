using UnityEngine;
using UnityEngine.AI;

public class Monster_Chase : MonsterState_temp
{
    public Monster_Chase(Monster_temp _monster) : base(_monster) { }

    private Transform _targetPos;
    private NavMeshAgent _agent;
    protected MonsterStateMachine_temp stateMachine;


    // 실 입력값 초기화
    public void MonsterChaseInit()
    {
        _targetPos = monster.TargetPosition;
        _agent = monster.MonsterAgent;
        stateMachine = monster._monsterMerchine;

        if (!_agent.isOnNavMesh)
        {
            if (NavMesh.SamplePosition(monster.transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                _agent.Warp(hit.position);
            }
            else { return; }
        }

        //// idle모드에서 전환되는 것을 고려하여 새롭게 초기화 상대 추적 시작
        _agent.enabled = true;
        _agent.ResetPath();

        if (_targetPos != null)
        {
            _agent.SetDestination(_targetPos.position);
        }
    }

    public override void Enter()
    {
        MonsterChaseInit();
    }

    // 감지형 콜라이더 필요
    public override void Update()
    {

        if (_agent.enabled && _agent.isOnNavMesh && _targetPos != null)
        {
            // 탐지는 몬스터에게 부착된 새로운 콜라이더에서 업데이트로 지속적으로 확인
            // 인지 되면 해당 스크립트로 전환되어 추적로직 작용
            _agent.SetDestination(_targetPos.position);
        }
       
    }

    public override void Exit()
    {
        if (_agent.enabled && _agent.isOnNavMesh)
        {
            _agent.ResetPath();
            _agent.enabled = false;
        }
        
    }
}
