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
    [SerializeField] public float MoveSpeed;
    [SerializeField] public float RunningSpeed;
    [SerializeField] public float CrawlSpeed;
    [SerializeField] public Transform[] PatrolPoints;
    [SerializeField] public Collider DetectRange;

    public Animator Ani;
    public Rigidbody Rigid; 
    public MonsterStateMachine_temp _monsterMerchine;

    private void StateMachineInit()
    {
        _monsterMerchine = new MonsterStateMachine_temp();
        _monsterMerchine.StateDic.Add(Estate.Idle, new Monster_Idle(this));
        _monsterMerchine.StateDic.Add(Estate.Chase, new Monster_Chase(this));
        _monsterMerchine.StateDic.Add(Estate.Patrol, new Monster_Patrol(this));


        // 시작은 idle 모드에서 시작
        _monsterMerchine.CurState = _monsterMerchine.StateDic[Estate.Idle];
    }


    // 디버그용 추후 삭제
    private void OnDrawGizmosSelected()
    {
        if (DetectRange != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(DetectRange.bounds.center, DetectRange.bounds.extents.magnitude);
        }

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
    private void OnTriggerEnter(Collider other)
    {
        // 설정한 플레이어 레이어 숫자와 부딫힌 오브젝트의 레이어가 겹칠때 추적로직 작동
        if ((PlayerLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            _monsterMerchine.ChangeState(_monsterMerchine.StateDic[Estate.Chase]);

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

    private void OnTriggerExit(Collider other)
    {
        if ((PlayerLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            _monsterMerchine.ChangeState(_monsterMerchine.StateDic[Estate.Idle]);
        }
    }

}
