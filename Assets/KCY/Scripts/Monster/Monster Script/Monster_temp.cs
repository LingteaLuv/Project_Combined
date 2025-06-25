using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster_temp : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] public NavMeshAgent MonsterAgent;
    [SerializeField] public Transform TargetPosition;
    [SerializeField] public LayerMask PlayerLayerMask;
    [SerializeField] public float WalkSpeed;
    [SerializeField] public float RunningSpeed;
    [SerializeField] public float CrawlSpeed;
    [SerializeField] public Transform[] PatrolPoints;
    [SerializeField] public Collider DetectRange;
    [SerializeField] public Transform SpawnPoint; // 좀비 시작 위치 설정 

    public Animator Ani;
    public Rigidbody Rigid;
    public MonsterStateMachine_temp _monsterMerchine;


    // 애니메이션 실행
   

    private void StateMachineInit()
    {
        _monsterMerchine = new MonsterStateMachine_temp();
        _monsterMerchine.StateDic.Add(Estate.Idle, new Monster_Idle(this));
        _monsterMerchine.StateDic.Add(Estate.Chase, new Monster_Chase(this));
        _monsterMerchine.StateDic.Add(Estate.Patrol, new Monster_Patrol(this));
        _monsterMerchine.StateDic.Add(Estate.Reset, new Monster_Reset(this));

        // 시작은 idle 모드에서 시작
        _monsterMerchine.CurState = _monsterMerchine.StateDic[Estate.Idle];
    }


    void Start()
    {
        Ani = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody>();
        StateMachineInit();
    }

    void Update()
    {
        _monsterMerchine.Update(); 
    }
    void FixedUpdate()
    {
        _monsterMerchine.FixedUpdate();
    }

    // 몬스터에 추가한 콜라이더와 레이어를 활요하여 추적로직
    private void DetectPlayer(Collider other)
    {
        Debug.Log("if문 진입");
        // 설정한 플레이어 레이어 숫자와 부딫힌 오브젝트의 레이어가 겹칠때 추적로직 작동
        if ((PlayerLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log($"[Trigger] 충돌: {other.name}, Layer: {other.gameObject.layer}");

            if ((PlayerLayerMask.value & (1 << other.gameObject.layer)) != 0)
            {
                Debug.Log(" 플레이어 감지됨 → 상태 전이 시도");

                _monsterMerchine.ChangeState(_monsterMerchine.StateDic[Estate.Chase]);
                Debug.Log("상태 전이 → Chase");

                // 체크 되면 몸체 돌리기
                Vector3 dir = TargetPosition.position - transform.position;
                dir.y = 0;
                if (dir.sqrMagnitude > 0.001f)
                {
                    Quaternion lookRot = Quaternion.LookRotation(dir.normalized);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime);
                }
            }
        }
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
        if ((PlayerLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log($"[TriggerExit] {other.name} 플레이어가 감지 범위에서 벗어남 → Idle 상태로 전이");
            _monsterMerchine.ChangeState(_monsterMerchine.StateDic[Estate.Idle]);
        }
    }

}
