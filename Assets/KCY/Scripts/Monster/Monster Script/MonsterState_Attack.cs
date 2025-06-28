using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Monster_Attack : MonsterState_temp, IAttackable
{
    public Monster_Attack(Monster_temp _monster) : base(_monster)
    {
        _ani = monster.Ani;
        _rb = monster.Rigid;
        _agent = monster.MonsterAgent;
        stateMachine = monster._monsterMerchine;
    }

    private int _attackDamage = 1;
    private Rigidbody _rb;
    private Animator _ani;
    private NavMeshAgent _agent;
    private IDamageable _curTarget;

    private float _lastAttackTime = -999f;
    private float _attackCoolTime = 5f;

    private Coroutine _attackCoroutine;
    protected MonsterStateMachine_temp stateMachine;

    // IAttackable 인터페이스 구현
    public void Attack(IDamageable target)
    {
        SetTarget(target);
        stateMachine.ChangeState(this); // 상태 전이까지 직접 수행
    }

    public void SetTarget(IDamageable target)
    {
        _curTarget = target;
    }

    public override void Enter()
    {
        Debug.Log("[Attack 상태 진입]");

        if (_ani != null)
        {
            _ani.SetTrigger("Attack");
            _ani.SetBool("isPatrol", false);
            _ani.SetBool("isChasing", false);
        }

        if (_agent != null)
        {
            _agent.isStopped = true;
        }

        // 기존 코루틴 중단 후 재시작
        if (_attackCoroutine != null)
            monster.StopCoroutine(_attackCoroutine);

        _attackCoroutine = monster.StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        // 대상이 지정될 때까지 대기
        yield return new WaitUntil(() => _curTarget != null);

        if (Time.time - _lastAttackTime < _attackCoolTime)
        {
            Debug.Log("공격 쿨타임 미도래 → 상태 전환");
            yield break;
        }

        _ani.SetTrigger("Attack");
        _lastAttackTime = Time.time;

        // 공격 타이밍 대기
        yield return new WaitForSeconds(0.5f);

        if (_curTarget != null)
        {
            _curTarget.Damaged(_attackDamage);
            Debug.Log("공격 성공 - 데미지 적용");
        }

        // 후딜
        yield return new WaitForSeconds(0.4f);

        // 체이스로 복귀
        if (!monster._isDead)
        {
            monster.Ani.ResetTrigger("Attack");
            Debug.Log("Attack → Chase 상태로 복귀");
            stateMachine.ChangeState(stateMachine.StateDic[Estate.Chase]);
        }
    }

    public override void Update()
    {
        if (_curTarget is MonoBehaviour target)
        {
            float distance = Vector3.Distance(monster.transform.position, target.transform.position);
            if (distance > monster.AttackRange)
            {
                _ani.ResetTrigger("Attack");
                Debug.Log("공격 중 타겟 이탈 → 체이스 전환");
                stateMachine.ChangeState(stateMachine.StateDic[Estate.Chase]);
            }
        }
    }

    public override void Exit()
    {
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

        _ani.ResetTrigger("Attack");
    }
}
