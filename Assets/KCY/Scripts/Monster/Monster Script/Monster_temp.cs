using UnityEngine;
using UnityEngine.AI;

public class Monster_temp : MonoBehaviour, IAttackable, IDamageable
{
    [Header("Elements")]
    [SerializeField] public NavMeshAgent MonsterAgent;
    [SerializeField] public Transform TargetPosition;
    [SerializeField] public LayerMask PlayerLayerMask;
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

        // 데미지를 Hit 상태로 위임 (중계)sp rm
        if (_monsterMerchine.StateDic[Estate.Hit] is Monster_Hit hitState)
        {
            hitState.Damaged(damage);
            _monsterMerchine.ChangeState(hitState);  // Hit 상태로 전이
        }
    }
    public void DetectPlayer(Collider other)
    {
        if (((1 << other.gameObject.layer) & PlayerLayerMask) == 0) return; // 플레이어가 아님

        if (_monsterMerchine.CurState != _monsterMerchine.StateDic[Estate.Patrol])
        {
            Debug.Log("▶ Patrol 상태가 아니라 감지 무시");
            return;
        }

        TargetPosition = other.transform;
        IsDetecting = true;
        Debug.Log($" 플레이어 감지됨 ({other.name}) Chase 상태로 전이");
        _monsterMerchine.ChangeState(_monsterMerchine.StateDic[Estate.Chase]);
    }
}
