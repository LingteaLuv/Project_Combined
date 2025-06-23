using UnityEngine;
using UnityEngine.AI;

public class Monster_Chase : MonsterState_temp
{
    private Transform _targetPos;
    private NavMeshAgent _agent;
    private float _detectRadius = 10f;
    private float _maxDistnace = 10f; // 처음 false 시작 오류 방지
    private LayerMask _playerLayer;
    protected MonsterStateMachine_temp stateMachine;


    public override void Enter()
    {
        // idle모드에서 전환되는 것을 고려하여 새롭게 초기화 상대 추적 시작
        _agent.enabled = true;
        _agent.SetDestination(_targetPos.position);
    }
    public override void Update()
    {
        // 프레임 마다 직속 탐색
        _agent.SetDestination(_targetPos.position);
        Collider[] hits = Physics.OverlapSphere(_agent.transform.position, _detectRadius, _playerLayer);

        bool playerDetect = false;
        foreach (Collider hit in hits)
        {
            if (hit.transform == _targetPos)
            {
                playerDetect = true;
                break;
            }
        }

        // 감지 실패
        float distance = Vector3.Distance(_agent.transform.position, _targetPos.position);
        if (!playerDetect && distance > _maxDistnace)
        {
            _agent.enabled = false;
            stateMachine.ChangeState(stateMachine.StateDic[Estate.Idle]);
        }
    }

    public override void Exit()
    {
        _agent.ResetPath();
        _agent.enabled = false;
    }
}
