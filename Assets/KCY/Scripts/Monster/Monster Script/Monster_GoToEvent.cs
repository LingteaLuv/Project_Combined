using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster_GoToEvent : MonsterState_temp
{
    public Monster_GoToEvent(Monster_temp _monster) : base(_monster)
    {
        _agent = monster.MonsterAgent;
        _ani = monster.Ani;
    }

    private NavMeshAgent _agent;
    private Animator _ani;
    private Vector3 _eventPos;
    private float _compareDis = 1.5f;
    private bool _hasArrive = false;

    public override void Enter()
    {
        _hasArrive = false;
        Debug.Log($"[GoToEvent] Enter 직전 TempPoint: {monster.TempPoint}");
        Debug.Log("사운드 감지 :  해당 지역으로 이동합니다.");

        if (monster.TempPoint != Vector3.zero)
        {
            _eventPos = monster.TempPoint;
        }

        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.autoBraking = true;
            _agent.isStopped = false;
            _agent.speed = monster.ChaseMoveSpeed;

            bool result = _agent.SetDestination(_eventPos);
            if (!result)
            {
                Debug.LogWarning("SetDestination 실패");
            }
        }

        if (_ani != null)
        {
            _ani.SetBool("isChasing", true);
            _ani.SetBool("isPatrol", false);
        }
    }

    public override void Update()
    {
        if (_agent == null || !_agent.isOnNavMesh || _hasArrive)
            return;

        float distance = _agent.remainingDistance;
        float speed = _agent.velocity.sqrMagnitude;

        if (!_agent.pathPending && distance <= _compareDis)
        {
            _hasArrive = true;

            _agent.isStopped = true;
            _agent.ResetPath();
            monster.Rigid.velocity = Vector3.zero;

            monster.StartCoroutine(WaitForSmoothAni());
        }
    }

    private IEnumerator WaitForSmoothAni()
    {
        if (_ani != null)
        {
            _ani.SetBool("isChasing", false);
            _ani.SetBool("isPatrol", false);
        }

        yield return new WaitForSeconds(2f);
        if (!monster.IsEventActive)
        {
            Debug.Log("감지 종료 확인 :Patrol로 전환 + TempPoint를 순찰 기준으로 사용");

            monster.BasePoint = monster.TempPoint;
            monster.TempPoint = Vector3.zero;
            monster._monsterMerchine.ChangeState(monster._monsterMerchine.StateDic[Estate.Patrol]);
        }
        else
        {
            Debug.Log("감지 유지 중 : 계속 정지 상태");
            _hasArrive = false; // 대기 연장
        }
    }

    public override void Exit()
    {
        _hasArrive = false;
        monster.IsEventActive = true;

        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.ResetPath();
            _agent.isStopped = false;
        }

        if (_ani != null)
        {
            _ani.SetBool("isChasing", false);
            _ani.SetBool("isPatrol", true);
            _ani.SetBool("isHeadTurn", false);
            _ani.ResetTrigger("Turn");
        }
    }
}