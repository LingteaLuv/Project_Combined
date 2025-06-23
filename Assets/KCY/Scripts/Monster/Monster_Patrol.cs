using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Patrol: MonsterState_temp
{
    public Monster_Patrol(Monster_temp _monster) : base(_monster) { }

    private NavMeshAgent _agent;
    private Transform[] _wayPoints;  // 순찰 위치 설정
    private int _curIndex = 0;
    private float _compareDis = 0.5f; // 남은 거리가 다 와 갔을 때 해당 값과 비교하여 index 값 전환

    public void Init()
    {
        _agent = monster.MonsterAgent;
        _wayPoints = monster.PatrolPoints;

        if (_agent == null) { return; }

        //  오류 방지
        if (_wayPoints == null || _wayPoints.Length == 0) { return; }

        // agent를 확성화해 waypoint를 따라가게 한다.
        _curIndex = 0;
        MoveToCurrentPoint();
    }

    private void MoveToCurrentPoint()
    {
        _agent.SetDestination(_wayPoints[_curIndex].position);
    }

    public override void Enter()
    {
        Init();
    }

    public override void Update()
    {
        // 몬스터가 아직 경로를 계산 중이면 대기
        if (_agent.pathPending) return;

        // 몬스터가 현재 경로상에서 도착 한계점 까지의 거리를 얼마나 남았는지 도착 예상 거리를 기준으로 판단 
        // 예상 거리에서 값이 변환되어 도착지점을 거치는 것을 방지하기 위해 속도가 0인 상태를 추가로 제시
        if (_agent.remainingDistance <= _compareDis && _agent.velocity.sqrMagnitude <= 0.01f)
        {
            // 인덱스가 길이와 같아지면 다시 0으로 돌아갈 수 있도록
            _curIndex = (_curIndex + 1) % _wayPoints.Length;
            MoveToCurrentPoint();
        }
    }

    // Chase 씬으로 돌아가는 것은 monster_temp에 있기 때문에 씬 전환은 필요 없다.
    public override void Exit()
    {
        _agent.ResetPath();
    }

}
