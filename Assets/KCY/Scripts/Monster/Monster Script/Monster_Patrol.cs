using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Monster_Patrol : MonsterState_temp
{
    public Monster_Patrol(Monster_temp _monster) : base(_monster)
    {
        _agent = monster.MonsterAgent;
        SpawnPoint = monster.SpawnPoint;
        _info = monster.Info;
    }
    private MonsterInfo _info;
    public Vector3 SpawnPoint; // 제일 먼저 스폰하는 곳에서의 장소(해당 장소는 불변)
    // public Vector3 TempPoint; // 이벤트 발생 - 해당 지역으로 가서 해당 장소에 그 이벤트를 발견해 처음 멈춰선 곳의, 상태 전환전에 현재의 포지션 값
    public Vector3 BasePoint; // 스폰과 템프의 중계역할을 할 예정, 처음은 스폰으로, 추적 후에는 템프로, 다시 돌아와서는 스폰으로
    public float PatrolRadius => _info.PatrolRadius; // 패트롤 반경 
    private float _sightAngle; // 시야각 (패트롤 중에는 해당 시야각을 늘려줘야 한다.)
    public float LimitTryCount = 10f;
    // 60초 후엔 스폰 자리로 돌아가야 한다.
    public float StartSearchTime = 0f; // SpawnPoint로의 회귀 시간
    public float SearchTime => _info.SearchTime;//PatrolRadiu
    // 15초 간 머물러야 한다.
    public float StayTimer = 0f;
    public float StayDuration = 15f;
    private bool isHeadRot = false;
    private NavMeshAgent _agent;
    private float _compareDis = 1.0f;
    private float PatrolSight => _info.PatrolSight;

    public void Init()
    {
        // 처음 시작은 스폰 포인트 부터 시작한다.  // chase를 나가는 경우 BasePoint에  TempPoint를 입력한다.
        // 템프포인트가 존재하면 템프포인에서 진행하고 없으면 스폰포인트에서 진행 (Rtp에서 템프를 null로 비울 예정- 그럼 다시 돌아옴)
        BasePoint = monster.TempPoint != Vector3.zero ? monster.TempPoint : SpawnPoint;
        // 패트롤 모드에서는 몬스터의 시야각이 상승한다. 기획서엔 2배라고 되어있는데 지금 각 60도 이기 때문에 2배면 240도다
        // 나중에 피드백 받고 수정할 것
        // 유효성 검사(에이전트 보유, 내비 이탈 - 다음을 리턴)
        if (_agent == null || !_agent.isOnNavMesh)
        {
            return;
        }
        // 아이들 모드에서 -> 패트롤 모드로 전환시 에이전트 움직임 재개 및 경로 재탐색
        _agent.isStopped = false;
        _agent.ResetPath();
        // 이동 로직
        if (RandomPatrolPoint(out Vector3 startPos))
        {
            _agent.SetDestination(startPos);
        }
        else
        {
            Debug.LogWarning("Patrol 위치 생성 실패 → 다음 프레임에 재시도");
        }
    }
    /// <summary>
    /// ((랜덤 패트롤 로직))
    /// 
    /// 1. 반지름에 대해 임의의 점을 생성한다 - 2. 해당 점에대해 유효성을 검사하고 가장 가까운 내비위의 점을 반환시킨다.
    /// 3. 해당 점에 대해 내비가 직접 갈수 있는지 다시 유효성 검사를 진행한다.
    /// 4. 유효성 검사에 해당하는 점으로 이동 / false의 경우 고장을 대비해 다시 스폰 포인트로 돌아온다. 
    ///
    /// </summary>
    public bool RandomPatrolPoint(out Vector3 result)
    {
        // path에 해당 경로에 대한 모든 정보를 기입(처음 시작 / 업데이트에서 진행해서 계속 새롭게 생기는 점에 대한 정보 저장 )
        NavMeshPath path = new NavMeshPath();
        for (int i = 0; i < LimitTryCount; i++)
        {
            // 반지름이 PartolRadius 인 원에 임의의 점을 반환하고  해당 점의 좌표를  xz좌표로 반환한다.
            Vector2 randomPatrolpoint = Random.insideUnitCircle * PatrolRadius;
            Vector3 basePos = BasePoint;
            basePos.y = 0f;
            Vector3 destination = BasePoint + new Vector3(randomPatrolpoint.x, 0, randomPatrolpoint.y);
  
            // destination 이치 근처 내비메쉬 확인하고 해당 내비의 위치를 인자로 반환 
            if (NavMesh.SamplePosition(destination, out NavMeshHit hit, 4f, NavMesh.AllAreas))
            {
                // 해당 점이 내비위에 있어도 갈 수 없으면 고장난다 따라서 해당 점으로 진짜 갈 수 있는지 유효성 검사
                if (_agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    result = hit.position;
                    result.y = monster.transform.position.y;
                    return true;
                }
                else
                {
                    Debug.Log("경로 계산 실패");
                }
            }
        }
        // 근대 해당 점의 위치에서 2f 거리에 내비 없을 경우 스폰 지점으로 복귀하고 해당 복귀 점을 인자로 반환
        result = BasePoint;
        return false;
    }
    public override void Enter()
    {
        // 몬스터 시야 반지름 길이는 몬스터 시야 거리 *  패트롤 사이트 배율
        monster.SightCol.radius = monster.SightRange * PatrolSight;


        if (TimeManager1.Instance.CurrentTimeOfDay.Value == DayTime.Night ||
      TimeManager1.Instance.CurrentTimeOfDay.Value == DayTime.MidNight)
        {
            _agent.speed = monster.Info.NightMoveSpeed;
        }
        else
        {
            _agent.speed = monster.Info.ChaseMoveSpeed;
        }


        StartSearchTime = 0f;
        StayTimer = 0f;
        monster.IsDetecting = false;
        monster.TargetPosition = null;
        if (monster.Ani != null)
        {
            monster.Ani.ResetTrigger("Attack");
            monster.Ani.SetBool("isPatrol", true);
            monster.Ani.SetBool("isChasing", false);
        }
        // 감지 딜레이 초기화
       /* if (_agent != null)
        {
            _agent.speed = monster.MoveSpeed;
        }*/
        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.isStopped = false;
            _agent.ResetPath();
        }
        Init();
    }
    public override void Update()
    {
        StartSearchTime += Time.deltaTime; // 회귀 시간 누적
        StayTimer += Time.deltaTime; // 머무는 시간 누적
        // 고장방지 유효성 검사
        // 에이전트 있는지, 내비매쉬 위에 있는지 확인하기 -> 현 상태에선 둘 중 하나 없으면 멈춤
        if (_agent == null || !_agent.isOnNavMesh)
        {
            monster._monsterMerchine.ChangeState(monster._monsterMerchine.StateDic[Estate.Reset]);
            return;
        }
        // 경로가 아직 생성되지 않았을 경우 리턴한다.
        if (_agent.pathPending)
        {
            return;
        }
        // 시야 우선순위로 인한 감지 불가 회귀 >> 시야와 청각이 겹치는 경우에는 레이로 쏴서 플레이어가 사라진 뒤에 남겨진 사운드 오브젝트를 따라간다.
        if (!monster.IsSightDetecting)
        {
            Collider[] hits = Physics.OverlapSphere(monster.transform.position, 3f, monster.SoundLayerMask);
            if (hits.Length > 0)
            {
                foreach (Collider hit in hits)
                {
                    Vector3 dir = (hit.transform.position - monster.transform.position).normalized;
                    if (dir != Vector3.zero)
                        monster.transform.rotation = Quaternion.LookRotation(dir);
                    monster.TempPoint = hit.transform.position;
                    monster._monsterMerchine.ChangeState(monster._monsterMerchine.StateDic[Estate.GoToEvent]);
                    break;
                }
            }
            float stopDetectTimer = 0f;
            if (!_agent.pathPending && _agent.velocity.magnitude < 0.01f)
            {
                stopDetectTimer += Time.deltaTime;
                if (stopDetectTimer > 2f)  // 2초 이상 멈춰 있을 때만 강제 리셋
                {
                    _agent.ResetPath();
                    _agent.isStopped = false;
                    if (RandomPatrolPoint(out Vector3 nextPos))
                    {
                        _agent.SetDestination(nextPos);
                        monster.Ani.SetBool("isPatrol", true);
                        StayTimer = 0f;
                    }
                    else
                    {
                        Debug.LogWarning("‼ 다음 경로 재설정 실패");
                    }
                    stopDetectTimer = 0f; // 리셋 후 타이머 초기화
                }
            }
            else
            {
                stopDetectTimer = 0f; // 
            }
        }
        // 1분 순찰 종료
        // 이게 우선순위 이게 먼저 실행되야 더 안돈다 (<< 버그 확인)
        if (StartSearchTime >= SearchTime)
        {
            monster.TempPoint = Vector3.zero;
            monster._monsterMerchine.ChangeState(monster._monsterMerchine.StateDic[Estate.ReturnToSpawn]);
            return;
        }
        // 15초 순회
        // 목적지에 거의 다 왔을 경우 다음 목적지로
        if (_agent.remainingDistance <= _compareDis && _agent.velocity.sqrMagnitude <= 0.1f)
        {
            // 목적지에 다왔을 때 정지해 있는 모션
            monster.Ani.SetBool("isPatrol", false);
            // 고개 돌리기 (10초 마다)_ 대충 해당 시간 안에 고개 돌리고 봐야함
            if (StayTimer >= 5f && StayTimer <= 15f && !isHeadRot)
            {
                monster.Ani.SetBool("isHeadTurn", true);
                monster.Ani.SetTrigger("Turn");
                monster.StartCoroutine(SmoothTurn(1f));
                monster.transform.rotation = Quaternion.Euler(0,180f, 0);
                monster.StartCoroutine(ResetHeadTurn());
                isHeadRot = true;
            }
            // 15초가 되면 이동
            if (StayTimer >= StayDuration)
            {
                if (RandomPatrolPoint(out Vector3 nextPos))
                {
                    // 해당 지점으로 고개 돌리기
                    Vector3 dir = (nextPos - monster.transform.position).normalized;
                    if (dir != Vector3.zero)
                    {
                        monster.transform.rotation = Quaternion.LookRotation(dir);
                    }
                    _agent.SetDestination(nextPos);
                    monster.Ani.ResetTrigger("Attack");
                    monster.Ani.SetBool("isPatrol", true);
                    isHeadRot = false;
                }
                monster.Ani.SetBool("isPatrol", true);
                StayTimer = 0f;
            }
        }
    }
    private IEnumerator SmoothTurn(float duration = 1f)
    {
        Quaternion startRot = monster.transform.rotation;
        Quaternion targetRot = startRot * Quaternion.Euler(0, 180f, 0);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            monster.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }
        monster.transform.rotation = targetRot; // 정확히 맞춰 마무리
    }
    // 애니메이션 전용 코루틴, 고개 돌려야 해요/.
    private IEnumerator ResetHeadTurn()
    {
        // 잠깐 쉬었다 바꾸기 - 안전성 검사
        yield return new WaitForSeconds(0.5f);
        monster.Ani.SetBool("isHeadTurn", false);
    }
    public override void Exit()
    {
        monster.SightCol.radius = monster.SightRange;

        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.ResetPath();
        }
        if (monster.Ani != null)
        {
            //monster.Ani.SetBool("isPatrol", false);
        }
    }
}