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
       var currentTime = TimeManager.Instance.CurrentTimeOfDay.Value;

        _hasArrive = false;
   
        if (monster.TempPoint != Vector3.zero)
        {
            _eventPos = monster.TempPoint;
        }

        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.autoBraking = true;
            _agent.isStopped = false;
            if (TimeManager.Instance.CurrentTimeOfDay.Value == DayTime.Night || TimeManager.Instance.CurrentTimeOfDay.Value == DayTime.MidNight)
            {
                _agent.speed = monster.Info.NightChaseMoveSpeed;
            }
            else
            {
                _agent.speed = monster.Info.ChaseMoveSpeed;

            }
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

        if (_eventPos != monster.TempPoint && monster.TempPoint != Vector3.zero)
        {
            _eventPos = monster.TempPoint;
            _agent.SetDestination(_eventPos);
        }

        if (_ani != null)
        {
            _ani.SetBool("isChasing", true);
            _ani.SetBool("isPatrol", false);
        }

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
            monster.BasePoint = monster.TempPoint;
            monster.TempPoint = Vector3.zero;
            monster._monsterMerchine.ChangeState(monster._monsterMerchine.StateDic[Estate.Patrol]);
        }
        else
        {
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