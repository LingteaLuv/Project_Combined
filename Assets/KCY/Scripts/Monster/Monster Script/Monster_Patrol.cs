using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Patrol : MonsterState_temp
{
    public Monster_Patrol(Monster_temp _monster) : base(_monster)
    {
        _agent = monster.MonsterAgent;
    }

    private NavMeshAgent _agent;
    private Transform[] _wayPoints;
    private int _curIndex = 0;
    private float _compareDis = 1f;

    // 감지 딜레이 관련
    private float _detectTimer = 0f;
    private float _detectDelay = 0.5f;
    public bool CanDetectNow => _detectTimer >= _detectDelay;

    public void Init()
    {
        _wayPoints = monster.PatrolPoints;

        if (_wayPoints == null || _wayPoints.Length == 0)
        {
            Debug.Log("순찰 지점이 없습니다.");
            return;
        }

        for (int i = 0; i < _wayPoints.Length; i++)
        {
            if (_wayPoints[i] == null)
            {
                Debug.Log("순찰 포인트를 빠뜨렸습니다.");
                return;
            }
        }

        if (_agent == null || !_agent.isOnNavMesh)
        {
            Debug.Log("NavMeshAgent가 NavMesh 위에 없습니다.");
            return;
        }

        _agent.isStopped = false;
        _agent.ResetPath();
        _curIndex = 0;

        MoveToCurrentPoint();
    }

    private void MoveToCurrentPoint()
    {
        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.SetDestination(_wayPoints[_curIndex].position);
            Debug.Log($"{_curIndex}번 포인트로 이동 중");
        }
        else
        {
            Debug.LogWarning("패트롤 중 어젠트가 내비 위에 없다");
        }
    }

    public override void Enter()
    {
        Debug.Log(" 패트롤 상태 진입");

        monster.IsDetecting = false;
        monster.TargetPosition = null;

        if (monster.Ani != null)
        {
            monster.Ani.ResetTrigger("Attack");
            monster.Ani.SetBool("isPatrol", true);
            monster.Ani.SetBool("isChasing", false);
        }

        _detectTimer = 0f; // 감지 딜레이 초기화

        if (_agent != null)
        {
            _agent.speed = monster.WalkSpeed;
        }

        Init();
    }

    public override void Update()
    {
        _detectTimer += Time.deltaTime; // 감지 딜레이 누적

        if (_agent == null || !_agent.isOnNavMesh || _wayPoints == null || _wayPoints.Length == 0)
        {
            Debug.Log("❗ 패트롤 상태 오류 - 구성 요소 누락");
            return;
        }

        if (_agent.pathPending) return;

        if (_agent.remainingDistance <= _compareDis && _agent.velocity.sqrMagnitude <= 0.01f)
        {
            _curIndex = (_curIndex + 1) % _wayPoints.Length;
            MoveToCurrentPoint();
        }
    }

    public override void Exit()
    {
        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.ResetPath();
        }

        if (monster.Ani != null)
        {
            monster.Ani.SetBool("isPatrol", false);
        }
    }
}

