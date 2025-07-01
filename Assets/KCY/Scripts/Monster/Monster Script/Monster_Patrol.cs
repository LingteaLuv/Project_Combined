using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Patrol : MonsterState_temp
{
    public Monster_Patrol(Monster_temp _monster) : base(_monster)
    {
        _agent = monster.MonsterAgent;
        SpawnPoint = monster.SpawnPoint;

    }


    public Transform SpawnPoint; // 제일 먼저 스폰하는 곳에서의 장소(해당 장소는 불변)
    public Transform TempPoint; // 이벤트 발생 - 해당 지역으로 가서 해당 장소에 그 이벤트를 발견해 처음 멈춰선 곳의, 상태 전환전에 현재의 포지션 값
    public Transform BasePoint; // 스폰과 템프의 중계역할을 할 예정, 처음은 스폰으로, 추적 후에는 템프로, 다시 돌아와서는 스폰으로
    public float PatrolRadius; // 패트롤 반경 
    private float _sightAngle; // 시야각 (패트롤 중에는 해당 시야각을 늘려줘야 한다.)
    public float LimitTryCount = 10f;

    // 60초 후엔 스폰 자리로 돌아가야 한다.
    public float SearchTime = 0f; // SpawnPoint로의 회귀 시간
    public float SearchDuration = 60f;//

    // 15초 간 머물러야 한다.
    public float StayTimer = 0f;
    public float StayDuration = 15f;

    private bool isHeadRot = false;

    private NavMeshAgent _agent;
    private float _compareDis = 1f;






    public void Init()
    {
        // 처음 시작은 스폰 포인트 부터 시작한다.  // chase를 나가는 경우 BasePoint에  TempPoint를 입력한다.
        // 템프포인트가 존재하면 템프포인에서 진행하고 없으면 스폰포인트에서 진행 (Rtp에서 템프를 null로 비울 예정- 그럼 다시 돌아옴)
        BasePoint = TempPoint != null ? TempPoint : SpawnPoint;

        // 패트롤 모드에서는 몬스터의 시야각이 상승한다. 기획서엔 2배라고 되어있는데 지금 각 60도 이기 때문에 2배면 240도다
        // 나중에 피드백 받고 수정할 것
        _sightAngle = monster.SightAngle * 1.5f;

        // 유효성 검사(에이전트 보유, 내비 이탈 - 다음을 리턴)
        if (_agent == null || !_agent.isOnNavMesh)
        {
            Debug.Log("NavMeshAgent가 NavMesh 위에 없습니다.");
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
            Vector3 destination = BasePoint.position + new Vector3(randomPatrolpoint.x, 0, randomPatrolpoint.y);

            // destination 이치 근처 내비메쉬 확인하고 해당 내비의 위치를 인자로 반환 
            if (NavMesh.SamplePosition(destination, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                // 해당 점이 내비위에 있어도 갈 수 없으면 고장난다 따라서 해당 점으로 진짜 갈 수 있는지 유효성 검사
                if (_agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    result = hit.position;
                    return true;
                }
            }
        }
        // 근대 해당 점의 위치에서 2f 거리에 내비 없을 경우 스폰 지점으로 복귀하고 해당 복귀 점을 인자로 반환
        result = BasePoint.position;
        return false;
    }


    public override void Enter()
    {
        Debug.Log(" 패트롤 상태 진입");

        SearchTime = 0f;
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

        if (_agent != null)
        {
            _agent.speed = monster.MoveSpeed;
        }

        Init();
    }

    public override void Update()
    {
        SearchTime += Time.deltaTime; // 회귀 시간 누적
        StayTimer += Time.deltaTime; // 머무는 시간 누적

        // 고장방지 유효성 검사
        // 에이전트 있는지, 내비매쉬 위에 있는지 확인하기 -> 현 상태에선 둘 중 하나 없으면 멈춤
        if (_agent == null || !_agent.isOnNavMesh)
        {
            Debug.Log("패트롤 상태 오류 : 어젠트가 없거나 내비 위 존재 안함 -> 리셋 상태로 이동");
            monster._monsterMerchine.ChangeState(monster._monsterMerchine.StateDic[Estate.Reset]);
            return;
        }

        // 경로가 아직 생성되지 않았을 경우 리턴한다.
        if (_agent.pathPending) return;

        // 1분 순찰 종료
        // 이게 우선순위 이게 먼저 실행되야 더 안돈다 (<< 버그 확인)
        if (SearchTime >= SearchDuration)
        {
            Debug.Log("순찰시간이 초과되었으니 리스폰 지역으로 복귀합니다.");
            monster.TempPoint = null;
            monster._monsterMerchine.ChangeState(monster._monsterMerchine.StateDic[Estate.ReturnToSpawn]);
            return;
        }

        // 15초 순회
        // 목적지에 거의 다 왔을 경우 다음 목적지로
        if (_agent.remainingDistance <= _compareDis && _agent.velocity.sqrMagnitude <= 0.01f)
        {
            // 고개 돌리기 (10초 마다)_ 대충 해당 시간 안에 고개 돌리고 봐야함
            if (StayTimer >= 5f && StayTimer <= 15f && !isHeadRot)
            {
                monster.Ani.SetBool("isHeadTurn", true);
                monster.Ani.SetTrigger("Turn");
                monster.transform.rotation = Quaternion.Euler(0, monster.transform.eulerAngles.y + 180f, 0);
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
                    Debug.DrawLine(BasePoint.position, nextPos, Color.green, 1f);

                    isHeadRot = false;
                }

                StayTimer = 0f;
            }
        }
    }
    // 애니메이션 전용 코루틴, 고개 돌려야 해요/.
    private IEnumerator ResetHeadTurn()
    {
        // 잠깐 쉬었다 바꾸기 - 안전성 검사
        yield return new WaitForSeconds(0.1f);
        monster.Ani.SetBool("isHeadTurn", false);
    }


    public override void Exit()
    {
        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.ResetPath();
        }

        if (monster.Ani != null)
        {
            monster.Ani.SetBool("isPatrol", false);
        }
    }
}

