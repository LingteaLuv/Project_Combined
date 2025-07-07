using System;
using UnityEngine;
using UnityEngine.AI;

public class Monster_temp : MonoBehaviour, IAttackable, IDamageable
{
    [Header("Elements")]
    [SerializeField] private MonsterInfo _info;
    public MonsterInfo Info => _info;
    public NavMeshAgent MonsterAgent; // 몬수터 어젠트 ,  추적용
    public Transform TargetPosition; // 플레이어 확인을 위한 포지션인데 굳이 퍼블릭으로 안해도 될 것 같다. 혹시 모르니까
    public LayerMask PlayerLayerMask;
    public LayerMask SoundLayerMask;
    public LayerMask BuildingLayerMask;
    public float MoveSpeed => _info.MoveSpeed; //  기본속도 : WalkSpeed
    public float ChaseMoveSpeed => _info.ChaseMoveSpeed; // 달리는 속도 : RunningSpeed
    //[SerializeField] public float NightMoveSpeed;

    public SphereCollider HearingCol;  //  사운드 디텍트용
    public SphereCollider SightCol; // 시야 디텍트용

   

    public Transform SpawnPointLink; // 수정 필요 >> 스폰포인트를 다 벡터로 받았는데 연결이 트랜스폼이다. 바꿔
    public Vector3 SpawnPoint;
    public Vector3 TempPoint;
    public Vector3 BasePoint;

    // 필요한 레이어는 3개(현재) : 플레이어, 사운드 아이템, 건물 

    [Header("status")]
    public Animator Ani;
    public float MaxHp => _info.MaxHP;
    public float AtkRange => _info.AtkRange;
    public bool _isDead = false;
    public IAttackable target;  // 몬스터, 캐릭터 (데미지 계산 위함)
   
    public Rigidbody Rigid;
    public MonsterStateMachine_temp _monsterMerchine;
    public GameObject MonObject;
    public MonsterHandDetector HandDetector; //  손 감지기 연결용

    public float SightRange => _info.SightRange; // 시야 거리 _ 이걸 콜라로이드로 제어하기 때문에 어떻게 할 수 없음
    public float SightAngle => _info.SightAngle;// 시야각
    public bool IsSightDetecting = false;

    public float HearingRange => _info.HearingRange; // 청각 범위 _ 이걸 콜라로이드로 제어하기 때문에 어떻게 할 수 없음.


    // 감지 우선순위 시간
    private float sightDetectTime = 0f;
    private float sightDetectDur = 1f;


    public BaseState_temp PrevState { get; private set; }
    public bool IsDetecting = false;
    public bool IsEventActive = true;


    private Vector3 _buildingSite = Vector3.positiveInfinity;
    private float _retryDis = 0.5f;

    // 벽 탈출을 위한 탐지 off 쿨타임
    // 해당 불값이 디텍트 정지시키고, dur값에 다다를 때까지 지속
    private bool _canDetect = false;
    private float _detectCoolTime = 0f;
    private float _detectDur = 3f;
    private int _wallHitCount = 0;
    private int _wallLimitedCount = 5;


    [SerializeField] public Lootable lootable;


    public float NightChaseMoveSpeed => _info.NightChaseMoveSpeed;
    public int EnemyID => _info.EnemyID;
    public string Name => _info.Name;
    //public string AtkType => _info.AtkType;
    //public float AtkSpeed => _info.AtkSpeed;
    public float NightMoveSpeed => _info.NightMoveSpeed;
    public string EnemyLootId => _info.EnemyLootID;
    public string EnemyLootGridChanceID => _info.EnemyLootGridChanceID;
    public AudioClip IdleSfx1 => _info.IdleSfx1;
    public AudioClip IdleSfx2 => _info.IdleSfx2;
    public float IdleSfxRange => _info.IdleSfxRange; 
    public AudioClip AtkSfx => _info.AtkSfx; 
    public AudioClip HitSfx => _info.HitSfx; 
    public AudioClip DieSfx => _info.DieSfx;
    public AudioClip ChaseSfx1 => _info.ChaseSfx1;
    public AudioClip ChaseSfx2 => _info.ChaseSfx2;





    public float CoolDown;

    public float AtkCoolDown => _info.AtkCoolDown;
    public float NightAtkCoolDownRate => _info.NightAtkCoolDown;

    private void OnTimeOfDayChanged(DayTime timeOfDay)
    {

        if (timeOfDay == DayTime.Night || timeOfDay == DayTime.MidNight)
        {
            CoolDown = AtkCoolDown / NightAtkCoolDownRate;
           // Debug.Log($"[쿨다운 변경] 시간대: {timeOfDay} → NightRate 적용됨, 최종 쿨다운: {CoolDown:F2}");
        }
        else
        {
            CoolDown = AtkCoolDown;
           // Debug.Log($"[쿨다운 변경] 시간대: {timeOfDay} → 기본 쿨다운 유지: {CoolDown:F2}");
        }
    }


    private void Awake()
    {

        PlayerLayerMask = LayerMask.GetMask("Player");
        Ani = GetComponentInChildren<Animator>();
        HandDetector = GetComponentInChildren<MonsterHandDetector>();
        _canDetect = true;
        if (SpawnPointLink != null)
        {
            SpawnPoint = SpawnPointLink.position;
        }

        // target = this as IAttackable; 피격 실험용으로 사용한 코드입니다 나중에 사용할 때 활성화 시켜주면 됩니다.
    }

    private void Start()
    {
        //Debug.Log("Monster Start()");
        StateMachineInit();

        if (SightCol != null)
        {
            SightCol.radius = SightRange;
            HearingCol.radius = HearingRange;
        }
        else
        {
            //Debug.LogError(" SightCol이 할당되지 않았습니다. Monster_temp에 연결하세요!");
        }

        TimeManager.Instance.CurrentTimeOfDay.OnChanged += OnTimeOfDayChanged;
        OnTimeOfDayChanged(TimeManager.Instance.CurrentTimeOfDay.Value);
    }

    private void Update()
    {
        if (!MonsterAgent.isOnNavMesh)
        {
            Debug.LogError("NavMeshAgent가 NavMesh 위에 없음!");
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {

            _monsterMerchine.ChangeState(_monsterMerchine.StateDic[Estate.Dead]);
        }

        // Debug.Log($" Update /// 객체 이름: {this.name}, _canDetect = {_canDetect}, 쿨타임 = {_detectCoolTime}");

        //Debug.Log($"[Patrol] 시야 감지 여부: {IsSightDetecting}");
   

        if (IsSightDetecting)
        {
            sightDetectTime += Time.deltaTime;

            if (sightDetectTime > sightDetectDur)
            {
                IsSightDetecting = false;
                sightDetectTime = 0f;
            }
        }



        // 상태머신 업데이트
        _monsterMerchine?.Update();

        // 벽에 맞았을 때 토글시켜 탐지를 방어하고, 시간초를 업데이트에서 누적시켜 자동 토글로 디텍트를 다시 온으로 만든다/ 오프로 만드는 스위치는 벽에 닿는 것 
        if (!_canDetect)
        {
            //Debug.Log($"쿨타임 진행 중 {_detectCoolTime:F2} / {_detectDur}");
            _detectCoolTime += Time.deltaTime;
            if (_detectCoolTime >= _detectDur)
            {
                _canDetect = true;
                _detectCoolTime = 0f;
            }
        }
    }

    private void StateMachineInit()
    {
        _monsterMerchine = new MonsterStateMachine_temp();
        _monsterMerchine.StateDic.Add(Estate.Idle, new Monster_Idle(this));
        _monsterMerchine.StateDic.Add(Estate.Patrol, new Monster_Patrol(this));
        _monsterMerchine.StateDic.Add(Estate.Chase, new Monster_Chase(this));
        _monsterMerchine.StateDic.Add(Estate.Attack, new Monster_Attack(this));
        _monsterMerchine.StateDic.Add(Estate.Reset, new Monster_Reset(this));
        _monsterMerchine.StateDic.Add(Estate.Hit, new Monster_Hit(this));
        _monsterMerchine.StateDic.Add(Estate.Dead, new Monster_Dead(this));
        _monsterMerchine.StateDic.Add(Estate.ReturnToSpawn, new Monster_ReturnToSpawn(this));
        _monsterMerchine.StateDic.Add(Estate.GoToEvent, new Monster_GoToEvent(this));

        // 처음 시작은 아이들 모드로 시작
        _monsterMerchine.ChangeState(_monsterMerchine.StateDic[Estate.Idle]);
    }


    // 외부에서 호출: 몬스터가 플레이어를 공격하도록 설정
    public void Attack(IDamageable target)
    {
        if (_monsterMerchine.StateDic[Estate.Attack] is Monster_Attack attackState)
        {
            attackState.SetTarget(target); // 타겟 저장
            _monsterMerchine.ChangeState(attackState); // 상태 전이
        }
    }
    public void Damaged(float damage)
    {
        if (_isDead) return;
        PrevState = _monsterMerchine.CurState;
        // 데미지를 Hit 상태로 위임 (중계)
        if (_monsterMerchine.StateDic[Estate.Hit] is Monster_Hit hitState)
        {
            hitState.Damaged(damage);
            _monsterMerchine.ChangeState(hitState);  // Hit 상태로 전이
        }
    }


    public void SightDetectPlayer(Collider other)
    {
       // Debug.Log($"[SightDetectPlayer 진입] 객체 이름: {this.name}");
        //Debug.Log(" SightDetectPlayer 함수 진입함");

        // 레이어를 통한 플레이어 확인 방어 코드 , 플레이어 캐릭터가 아니면 리턴  >>> 건물을 포함하지 않으면 건물을 캐치할 수 없어요
        if (((1 << other.gameObject.layer) & (PlayerLayerMask | BuildingLayerMask)) == 0)
        {
            return;
        }

        IsSightDetecting = true;

        // 빌딩에서 탈출하기 위한 패트롤용
        if (!_canDetect)
        {
            return;
        }

       // Debug.Log("1. State 체크 시작");
       //Debug.Log($"State 현재 상태: {_monsterMerchine.CurState}, 필요 상태: {_monsterMerchine.StateDic[Estate.Patrol]}");


        // 레어어가 현 상태가 패트롤이 아닌경우 감지 무시 (패트롤의 경우만 감지하고, 공격상태일땐 거리로 측정함, 아이들 모드 역시 무시중)
        if (_monsterMerchine.CurState != _monsterMerchine.StateDic[Estate.Patrol])
        {
            return;
        }

        // 빌딩에 때문에 막혔을 때 ->  해당 지역에 대한 감지 실패 지역을 기억하고 다시 감지하는 것을 무시하는 것//
        // 벽에의한 무한루프 방지? - 추적 중 탈출을 위한 준비
        // 위치 비교를 위한 값을 설정해 비교하고, 사이의 거리값이 비교 값보다 작으면 감지 종료

        if (_buildingSite != Vector3.positiveInfinity)
        {
            float distance = Vector3.Distance(transform.position, _buildingSite);
            if (distance < _retryDis)
            {
                return;
            }
        }

        Vector3 dirToPlayer = (other.transform.position - transform.position).normalized;
        // dirToPlayer.y = 0f; // 상황 보고 계단에서 적용이 안되는 경우 y값도 같이 사용한다. -> 이거 y값 안들어가면 경사로 확인 불가

        // 몬스터 정면을 중심으로  플레이어어 위치까지의 각도 대해 
        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > SightAngle)
        {

            return;
        }

        //Debug.Log("레이ㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣㅣ");


        // 좀비 머리 위에서 좀비와 플레이어를 가리키는 방향으로 레이를 쏴서 맞춘 레이어가 빌딩이면 아이들 모드로 가고 - 자연스럽게 패트롤 모드로 갈 수 있도록 유도

        Vector3 CapsuleStart = transform.position + Vector3.up * 0.5f;
        Vector3 CapsuleEnd = transform.position + Vector3.up * 1.5f;
        float capsuleRadius = 0.5f;
        float capDis = 5f;
        Vector3 forward = transform.forward;

        if (Physics.CapsuleCast(CapsuleStart, CapsuleEnd, capsuleRadius, forward, out RaycastHit hit, capDis))
        {
            //Debug.Log("맞음"); //  이 로그가 찍혀야 레이가 맞은 것
           // Debug.Log($"Raycast 충돌 오브젝트: {hit.collider.name}, 레이어: {hit.collider.gameObject.layer}");

            if (((1 << hit.collider.gameObject.layer) & BuildingLayerMask) != 0)
            {
                // 벽에 부딫힌 횟수 감지
                // 벽에 5번 부딫히면 그냥 집으로 와라
                _wallHitCount++;
    

                if (_wallHitCount >= _wallLimitedCount)
                {
                    //_monsterMerchine.ChangeState(HitState); >>>  확인
                    _wallHitCount = 0;
                    return;
                }

                // 해당 위치를 저장한다.
                _buildingSite = transform.position;
                TargetPosition = null;
                IsDetecting = false;

                // 디텍트 방어 중
                _canDetect = false;
                _detectCoolTime = 0f;

                _monsterMerchine.ChangeState(_monsterMerchine.StateDic[Estate.Idle]);
                return;
            }
        }

        // 중복 전이 방지
        if (_monsterMerchine.CurState == _monsterMerchine.StateDic[Estate.Chase])
            return;

        // 감지 성공 시 기록을 초기화 합니다.
        _buildingSite = Vector3.positiveInfinity;

        // 플레이어 위치 찾고, 감지 하고 

        if (((1 << other.gameObject.layer) & PlayerLayerMask) != 0)
        {
            TargetPosition = other.transform;
            IsDetecting = true;

            //// 돌리는 것 같은데 .////  일단 이거 확인  >>>> 그냥 사운드에 걸리는 거였음...... 시야는 잘 되고 있었다.///
            Vector3 LookDir = (TargetPosition.position - transform.position);
            // LookDir.y = 0; -> 이거 위에 못본다 
            if (LookDir != Vector3.zero)
            {
                // 감지하면 돌린다.
                transform.rotation = Quaternion.LookRotation(LookDir);
            }
            _monsterMerchine.ChangeState(_monsterMerchine.StateDic[Estate.Chase]);

        }
        else
        {
            Debug.Log($"플레이어가 아님 무시");
        }


    }

    public void SoundDetectPlayer(Collider other)
    {
        //플레이어나 사운드가 아니면 리턴
        if (((1 << other.gameObject.layer) & PlayerLayerMask) == 0 && ((1 << other.gameObject.layer) & SoundLayerMask) == 0) return;

        // 소리로 플레이어를 탐색하였을 경우
        if (((1 << other.gameObject.layer) & PlayerLayerMask) != 0)
        {
            Vector3 dirToPlayer = (other.transform.position - transform.position).normalized;
            if (dirToPlayer != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(dirToPlayer);
            }
            return;
        }

        if (IsSightDetecting)
        {
            return;
        }

        // 소리로 소리 아이템을 확인한 경우
        if (((1 << other.gameObject.layer) & SoundLayerMask) != 0)
        {
            Vector3 dirToSound = (other.transform.position - transform.position).normalized;
            if (dirToSound != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(dirToSound);
            }

            TempPoint = other.transform.position;
            IsEventActive = true;

            if (_monsterMerchine.CurState != _monsterMerchine.StateDic[Estate.GoToEvent])
            {
                _monsterMerchine.ChangeState(_monsterMerchine.StateDic[Estate.GoToEvent]);
            }
        }

    }

    public void AttackEvent()
    {
        if (_monsterMerchine.CurState is Monster_Attack attackState)
        {
            attackState.AttackEvent();
        }
    }

}
