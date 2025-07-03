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
    public float AtkCoolDown => _info.AtkCoolDown;

    private Coroutine _attackCoroutine;
    protected MonsterStateMachine_temp stateMachine;

    public void Attack(IDamageable target)
    {
        if (!IsAttackAvailable())
        {
            Debug.Log("⏳ 공격 쿨타임 중");
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
        return (Time.time - _lastAttackTime >= AtkCoolDown);
    }

    public void AttackEvent()
    {
        Debug.Log("[어택이벤트 호출] - 애니메이션 타이밍 맞춰 데미지 적용");

        Collider[] hits = Physics.OverlapSphere(monster.HandDetector.transform.position, 3f, monster.PlayerLayerMask);
        foreach (var hit in hits)
        {
            IDamageable damageTarget = hit.GetComponent<IDamageable>();
            if (damageTarget == null)
                damageTarget = hit.GetComponentInParent<IDamageable>();

            if (damageTarget != null && hit.gameObject.activeInHierarchy)
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

        Debug.Log(" 타격 실패: 플레이어 없음 또는 IDamageable 아님");
    }

    public override void Enter()
    {
        Debug.Log("Attack 상태 들어옴");

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
                Debug.Log("공격 시작 시 타겟 방향으로 회전");
            }
        }

      
        if (_attackCoroutine != null)
        {
            monster.StopCoroutine(_attackCoroutine);
        }
        monster.StartCoroutine(PleaseStopMonster());
        _attackCoroutine = monster.StartCoroutine(AttackCoroutine());
    }
    private IEnumerator PleaseStopMonster()
    {
        yield return null; // 다음 프레임까지 대기
        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            _agent.ResetPath();
            Debug.Log("다음 프레임에서 강제로 NavMeshAgent 정지");
        }
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitUntil(() => _curTarget != null);

        if (Time.time - _lastAttackTime < AtkCoolDown)
        {
            Debug.Log("공격 쿨타임 끝, 상태 전환");
            stateMachine.ChangeState(stateMachine.StateDic[Estate.Chase]);
            _attackCoroutine = null;
            yield break;
        }

        _lastAttackTime = Time.time;

        yield return new WaitForSeconds(1.0f); // 애니메이션 길이 + 후딜

        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
            Debug.Log(" 공격 후 어젠트 일시 정지");
        }

        yield return new WaitForSeconds(0.8f); // 정지 유지 시간

        if (!monster._isDead && stateMachine.CurState == this)
        {
            Debug.Log("Attack에서 Chase 상태로 복귀");
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
            return;
        }

        // 애니 끝났으면 Agent 재시작
        if (_agent.isStopped)
        {
            _agent.isStopped = false;
            Debug.Log(" 공격 애니 끝 어젠트 재시작");
        }

        // 타겟 추적/공격 처리
        if (stateMachine.CurState == this && _curTarget is MonoBehaviour target)
        {
            float distance = Vector3.Distance(monster.transform.position, target.transform.position);

            if (distance <= monster.AtkRange + 0.3f)
            {
                Vector3 dir = (target.transform.position - monster.transform.position).normalized;
                dir.y = 0f;
                float angle = Vector3.Angle(monster.transform.forward, dir);

                if (angle > 10f)
                {
                    // 회전 먼저 수행, 공격은 보류
                    monster.transform.rotation = Quaternion.Slerp(
                        monster.transform.rotation,
                        Quaternion.LookRotation(dir),
                        10f * Time.deltaTime
                    );
                    Debug.Log($"공격 전 회전 중... (angle={angle:F1})");
                    return;
                }

                //  회전 완료 ,쿨타임 OK 공격 시도
                if (IsAttackAvailable())
                {
                    Debug.Log("정면 회전 완료 + 쿨타임 완료 Attack()");
                    Attack(_curTarget);
                }
                else
                {
                    Debug.Log("쿨타임 대기 중");
                }
            }
            else
            {
                Debug.Log($"사거리 벗어남 → Chase 전환 (거리={distance:F2})");
                stateMachine.ChangeState(stateMachine.StateDic[Estate.Chase]);
            }
        }
        else if (_curTarget == null)
        {
            Debug.Log("타겟 없음 → Chase 전환");
            stateMachine.ChangeState(stateMachine.StateDic[Estate.Chase]);
        }
    }


    public override void Exit()
    {
        if (monster.HandDetector != null)
        {
            monster.CancelInvoke(nameof(monster.HandDetector.DetectPlayer));
        }

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
