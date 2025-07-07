using UnityEngine;
using UnityEngine.AI;

public class Monster_Chase : MonsterState_temp
{
    public Monster_Chase(Monster_temp _monster) : base(_monster)
    {
        _agent = monster.MonsterAgent;
        stateMachine = monster._monsterMerchine;
        _info = monster.Info;
    }
    private MonsterInfo _info;
    private Transform _targetPos;
    private NavMeshAgent _agent;
    private float _missingTime = 0f;
    protected MonsterStateMachine_temp stateMachine;

    private float ChaseSight => _info.ChaseSight;

    public void MonsterChaseInit()
    {
        _targetPos = monster.TargetPosition;

        if (_targetPos == null)
        {
            Debug.LogWarning("타겟이 존재하지 않아 추적 불가");
            return;
        }

        if (_agent == null || !_agent.isOnNavMesh)
        {
            if (NavMesh.SamplePosition(monster.transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                _agent.enabled = true;
                _agent.Warp(hit.position);
            }
            else
            {
                Debug.LogWarning("NavMesh 위에 없음 → 이동 불가");
                return;
            }
        }

        _agent.isStopped = false;
        _agent.stoppingDistance = 0.8f;
        _agent.ResetPath();
        _agent.SetDestination(_targetPos.position);
        Debug.Log($" 추적 시작: {_targetPos.position}");
    }

    public override void Enter()
    {

        monster.SightCol.radius = monster.SightRange * ChaseSight;
        var currentTime = TimeManager.Instance.CurrentTimeOfDay.Value;
        if (TimeManager.Instance.CurrentTimeOfDay.Value == DayTime.Night || TimeManager.Instance.CurrentTimeOfDay.Value == DayTime.MidNight)
        {
            _agent.speed = monster.Info.NightChaseMoveSpeed;
            //Debug.Log(" 밤 >> 추격 시간 적용");
            //Debug.Log($"[Chase] 시간대: {currentTime} → NightChase 속도 적용: {_agent.speed:F2}");
        }
        else
        {
            _agent.speed = monster.Info.ChaseMoveSpeed;
            //Debug.Log(" 낮, 아침 >> 일반 추적 속도");
            //Debug.Log($"[Chase] 시간대: {currentTime} → 기본 Chase 속도 적용: {_agent.speed:F2}");
        }


        //Debug.Log("체이싱 상태 진입");
        //Debug.Log($"[Chase] 상태 진입 완료 / 추적 대상: {monster.TargetPosition?.name}");
        _missingTime = 0f;

        // 몬스터 애니메이션 용 방어코드
        if (monster.Ani != null)
        {
            monster.Ani.ResetTrigger("Attack");
            monster.Ani.SetBool("isChasing", true);
            monster.Ani.SetBool("isPatrol", false);
            
        }

        // 어젠트 방어 - 어젠트 속도 조절/ 공격을 위해 몬스터가 목적지와의 멈출 간격/ 몬스터 이동
        if (_agent != null)
        {
            //_agent.speed = monster.ChaseMoveSpeed;
            _agent.stoppingDistance = 0.8f; // ← 적당한 거리 유지 (AttackRange보다 작거나 비슷하게)
            _agent.isStopped = false;
        }

        MonsterChaseInit();
    }

    public override void Update()
    {
        if (_agent == null || !_agent.isOnNavMesh || monster.TargetPosition == null || monster._isDead)
            return;

        // 대상이 파괴되거나 소실되는 경우 해당 위치를 tempPoint로 설정 - 자연스럽게 아이들로 들어가서 다시 템프 중심의 패트롤
        if (monster.TargetPosition == null)
        {
            if (monster.TempPoint == Vector3.zero)
            {
               monster.TempPoint = monster.transform.position;
            }

            monster.TargetPosition = null;
            monster.IsDetecting = false;

            monster._monsterMerchine.ChangeState(monster._monsterMerchine.StateDic[Estate.Idle]);
          
        }

        Vector3 targetPos = monster.TargetPosition.position;
        float distance = Vector3.Distance(monster.transform.position, targetPos);
        // 1. 공격 사거리 도달
        if (distance <= monster.AtkRange)
        {
            Debug.Log(" 공격 범위 도달 > Attack 상태 전이");
            stateMachine.ChangeState(stateMachine.StateDic[Estate.Attack]);
            return;
        }

        // 2. 경로 유효성 확인
        if (_agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Debug.LogWarning("경로 생성 실패 > Reset 상태로 전이");
            stateMachine.ChangeState(stateMachine.StateDic[Estate.Reset]);
            return;
        }

        // 3. 추적 중 이동 불능 상태 감지
        if (_agent.hasPath && !_agent.pathPending && _agent.remainingDistance > _agent.stoppingDistance + 0.1f &&
            _agent.desiredVelocity.sqrMagnitude < 0.01f)
        {
            _missingTime += Time.deltaTime;
            if (_missingTime >= 1.5f)
            {
                //Debug.LogWarning("경로 멈춤 감지 > Reset 상태 전이");
                stateMachine.ChangeState(stateMachine.StateDic[Estate.Reset]);
                return;
            }
        }
        else
        {
            _missingTime = 0f;
        }

        // 4. 계속 목표 위치 업데이트
        _agent.SetDestination(targetPos);
    }

    public override void Exit()
    {

        monster.SightCol.radius = monster.SightRange;
        

        if (monster.TargetPosition == null)
        {
            monster.IsSightDetecting = false;
        }

        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.ResetPath();
            _agent.isStopped = false;
        }

        if (monster.Ani != null)
        {
            monster.Ani.SetBool("isChasing", false);
        }
    }
}
