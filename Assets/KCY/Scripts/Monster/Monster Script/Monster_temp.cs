using UnityEngine;
using UnityEngine.AI;

public class Monster_temp : MonoBehaviour, IAttackable, IDamageable
{
    [Header("Elements")]
    [SerializeField] public NavMeshAgent MonsterAgent;
    [SerializeField] public Transform TargetPosition;
    [SerializeField] public LayerMask PlayerLayerMask;
    [SerializeField] public LayerMask BuildingLayerMask;
    [SerializeField] public float WalkSpeed;
    [SerializeField] public float RunningSpeed;
    [SerializeField] public float CrawlSpeed;
    [SerializeField] public Transform[] PatrolPoints;
    [SerializeField] public Collider DetectCol;
    [SerializeField] public Transform SpawnPoint;

    [Header("status")]
    [SerializeField] public int MonsterHp = 10;
    [SerializeField] public float AttackRange = 1.5f;
    public bool _isDead = false;
    public IAttackable target;  // 몬스터, 캐릭터 (데미지 계산 위함)
    public Animator Ani;
    public Rigidbody Rigid;
    public MonsterStateMachine_temp _monsterMerchine;
    public GameObject MonObject;
    public MonsterHandDetector HandDetector; //  손 감지기 연결용

    public BaseState_temp PrevState { get; private set; }
    public bool IsDetecting = false;

    /// <summary>
    ///  피격 확인용
    /// </summary>
    /// <param name="damage"></param>
    /*public void Damaged(int damage)
    {
        // 죽어 있으면 데미지 받지 말고
        if (_isDead) return;

        // 좀비 체력 깍기
        MonsterHp -= damage;
        Debug.Log($"🩸 피격! 체력: {MonsterHp}");
        //  피격 모션 발동
        Ani.SetTrigger("IsHit");

        if (MonsterHp <= 0 && !_isDead)
        {
            _isDead = true;
            Debug.Log("사망");
            
            //어젠트로 움직임 제어하므로 어젠트를 정지 시켜주고 속도를 0으로 만들어 준다
            MonsterAgent.isStopped = true;
            MonsterAgent.velocity = Vector3.zero;
            Ani.SetTrigger("Dead");
        }
    }*/
    /// <summary>
    ///  피격 확인용
    /// </summary>
    /// 
    private void Awake()
    {
        PlayerLayerMask = LayerMask.GetMask("Player");
        Ani = GetComponentInChildren<Animator>();
        HandDetector = GetComponentInChildren<MonsterHandDetector>();
        // target = this as IAttackable; 피격 실험용으로 사용한 코드입니다 나중에 사용할 때 활성화 시켜주면 됩니다.

    }

    private void Start()
    {
        Debug.Log("Monster Start()");
        StateMachineInit();

    }

    private void Update()
    {
        _monsterMerchine?.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("스페이스 피격 테스트");
            (this as IDamageable)?.Damaged(1);
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

        _monsterMerchine.ChangeState(_monsterMerchine.StateDic[Estate.Idle]);
    }

    private void OnTriggerEnter(Collider other)
    {
        DetectPlayer(other);

    }

    private void OnTriggerStay(Collider other)
    {
        DetectPlayer(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & PlayerLayerMask) == 0) return;

        if (IsDetecting && TargetPosition == other.transform)
        {
            Debug.Log(" 플레이어 감지 해제 → Idle 상태 복귀");

            TargetPosition = null;
            IsDetecting = false;
            _monsterMerchine.ChangeState(_monsterMerchine.StateDic[Estate.Idle]);
        }
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
    public void Damaged(int damage)
    {
        if (_isDead) return;

        // 데미지를 Hit 상태로 위임 (중계)
        if (_monsterMerchine.StateDic[Estate.Hit] is Monster_Hit hitState)
        {
            hitState.Damaged(damage);
            _monsterMerchine.ChangeState(hitState);  // Hit 상태로 전이
        }
    }
    public void DetectPlayer(Collider other)
    {
        // 레이어를 통한 플레이어 확인 방어 코드 , 플레이어 캐릭터가 아니면 리턴
        if (((1 << other.gameObject.layer) & PlayerLayerMask) == 0) return; // 플레이어가 아님

        // 레어어가 현 상태가 패트롤이 아닌경우 감지 무시 (패트롤의 경우만 감지하고, 공격상태일땐 거리로 측정함, 아이들 모드 역시 무시중)
        if (_monsterMerchine.CurState != _monsterMerchine.StateDic[Estate.Patrol])
        {
            Debug.Log("Patrol 상태가 아니라 감지 무시");
            return;
        }

        Vector3 dirToPlayer = (other.transform.position - transform.position).normalized;
        dirToPlayer.y = 0f; // 상황 보고 계단에서 적용이 안되는 경우 y값도 같이 사용한다.

        // 몬스터 정면을 중심으로  플레이어어 위치까지의 각도 대해 
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle > 75f)
        {
            Debug.Log("좀비 시야에서 벗어났으니까 추격하지 않습니다.");
            return;
        }

        // 좀비 머리 위에서 좀비와 플레이어를 가리키는 방향으로 레이를 쏴서 맞춘 레이어가 빌딩이면 아이들 모드로 가고 - 자연스럽게 패트롤 모드로 갈 수 있도록 유도
        Ray ray = new Ray(transform.position + Vector3.up * 1f, dirToPlayer);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            if (((1 << other.gameObject.layer) & BuildingLayerMask) != 0)
            {
                Debug.Log("건물이랑 붙었다 다시 탐지 하면 안되요");
                TargetPosition = null;
                IsDetecting = false;

                _monsterMerchine.ChangeState(_monsterMerchine.StateDic[Estate.Idle]);
                return;
            }
        }

        TargetPosition = other.transform;
        IsDetecting = true;
        Debug.Log($" 플레이어 감지됨 ({other.name}) Chase 상태로 전이");
        _monsterMerchine.ChangeState(_monsterMerchine.StateDic[Estate.Chase]);
    }

    public void AttackEvent()
    {
        Debug.Log("<color=lime>[Monster_temp] AttackEvent 호출됨 </color>");
        if (_monsterMerchine.CurState is Monster_Attack attackState)
        {
            attackState.AttackEvent();
        }
    }

}
