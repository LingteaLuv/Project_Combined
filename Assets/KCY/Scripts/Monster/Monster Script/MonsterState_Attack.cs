using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Attack : MonsterState_temp, IAttackable
{
    public Monster_Attack(Monster_temp _monster) : base(_monster)
    {
        _ani = monster.Ani;
        _rb = monster.Rigid;
        _agent = monster.MonsterAgent;
        stateMachine = monster._monsterMerchine;
        _info = monster.Info;
    }
    public MonsterInfo _info;
    public int AtkDamage => _info.AtkDamage;
    private Rigidbody _rb;
    private Animator _ani;
    private NavMeshAgent _agent;
    private IDamageable _curTarget;

    private float _lastAttackTime = -999f;

    private Coroutine _attackCoroutine;
    protected MonsterStateMachine_temp stateMachine;

    public void Attack(IDamageable target)
    {
        if (!IsAttackAvailable() || monster._isDead)
        {
            return;
        }

        // 정면 회전 확인
        Vector3 dirToTarget = ((MonoBehaviour)target).transform.position - monster.transform.position;
        dirToTarget.y = 0f;
        float angle = Vector3.Angle(monster.transform.forward, dirToTarget.normalized);

        if (angle > 10f)
        {
            return;
        }

        // 이미 Attack 상태일 경우 중복 전이 막고 코루틴만 실행
        if (stateMachine.CurState == this)
        {
            if (_attackCoroutine == null)
            {
                _attackCoroutine = monster.StartCoroutine(AttackCoroutine());
            }
            return;
        }

        SetTarget(target);
        stateMachine.ChangeState(this);
    }


    public void SetTarget(IDamageable target)
    {
        _curTarget = target;
    }

    public bool IsAttackAvailable()
    {
        return (Time.time - _lastAttackTime >= monster.CoolDown);
    }


    // 이벤트 함수에서 호출 > 콜라로이드로 감지한 후 > ㅈ
    public void AttackEvent()
    {
        Collider[] hits = Physics.OverlapSphere(monster.HandDetector.transform.position,0.5f, monster.PlayerLayerMask);

        foreach (var hit in hits)
        {
            IDamageable damageTarget = hit.GetComponent<IDamageable>();
            if (damageTarget == null)
                damageTarget = hit.GetComponentInParent<IDamageable>();
            Vector3 distance = hit.transform.position - monster.HandDetector.transform.position;
            
            if (damageTarget != null && hit.gameObject.activeInHierarchy && distance.magnitude < 2.4f)
            {
                try
                {
                    damageTarget.Damaged(AtkDamage);
                    Debug.Log("실제 타격 감지 및 데미지 적용");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($" 데미지 처리 중 예외 발생: {e.Message}");
                }
                return;
            }
        }
    }

    public override void Enter()
    {
        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            _agent.ResetPath();
        }

        if (_ani != null)
        {
            AnimatorStateInfo currentState = _ani.GetCurrentAnimatorStateInfo(0);

            if (!currentState.IsTag("Attack") && !_ani.IsInTransition(0))
            {
                _ani.SetTrigger("Attack");
            }

            _ani.SetBool("isPatrol", false);
            _ani.SetBool("isChasing", false);
        }

        if (monster.HandDetector != null)
        {
            monster.InvokeRepeating(nameof(monster.HandDetector.DetectPlayer), 0f, 0.1f);
        }

        if (_curTarget is MonoBehaviour target)
        {
            Vector3 dir = (target.transform.position - monster.transform.position).normalized;
            dir.y = 0f;
            if (dir != Vector3.zero)
            {
                monster.transform.rotation = Quaternion.LookRotation(dir);
            }
        }
        
        if (_attackCoroutine != null)
        {
            monster.StopCoroutine(_attackCoroutine);
        }
        _attackCoroutine = monster.StartCoroutine(AttackCoroutine());
    }
    private void PleaseStopMonster()
    {
        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            _agent.ResetPath();
        }
    }

    private IEnumerator AttackCoroutine()
    {
        if (Time.time - _lastAttackTime < monster.CoolDown)
        {
            stateMachine.ChangeState(stateMachine.StateDic[Estate.Chase]);
            _attackCoroutine = null;
            yield break;
        }
        _lastAttackTime = Time.time;
        PleaseStopMonster();
        yield return new WaitForSeconds(1.0f); // 애니메이션 길이 + 후딜
        PleaseStopMonster();
        yield return new WaitForSeconds(0.5f); // 정지 유지 시간
        
        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        } 

        if (!monster._isDead)
        {
            stateMachine.ChangeState(stateMachine.StateDic[Estate.Chase]);
            yield break;
        }
        
        Vector3 targetPos = monster.TargetPosition.position;
        float distance = Vector3.Distance(monster.transform.position, targetPos);
        if (distance > monster.AtkRange)
        {
            stateMachine.ChangeState(stateMachine.StateDic[Estate.Chase]);
        }
        _attackCoroutine = null;
    }

    public override void Update()
    {
        if (_ani == null || _agent == null || !_agent.isOnNavMesh) return;

        AnimatorStateInfo stateInfo = _ani.GetCurrentAnimatorStateInfo(0);
        // 공격 애니메이션 중일 땐 멈춤 유지
        if (stateInfo.IsTag("Attack") || _ani.IsInTransition(0))
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            _agent.ResetPath();
        }
        // 애니 끝났으면 Agent 재시작
        if (_agent.isStopped)
        {
            _agent.isStopped = false;
        }
        if (_attackCoroutine == null && IsAttackAvailable() && _curTarget!= null)
        {
            _attackCoroutine = monster.StartCoroutine(AttackCoroutine());
        }
    }


    public override void Exit()
    {
        if (monster.HandDetector != null)
        {
            monster.CancelInvoke(nameof(monster.HandDetector.DetectPlayer));
        }
        _ani.ResetTrigger("Attack");
        _ani.ResetTrigger("Attack");

        if (_attackCoroutine != null)
        {
            monster.StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }

        _curTarget = null;

        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.isStopped = false;
        }
    }
}
