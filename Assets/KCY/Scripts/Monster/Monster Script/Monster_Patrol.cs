using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Patrol: MonsterState_temp
{
    public Monster_Patrol(Monster_temp _monster) : base(_monster)
    {
        _agent = monster.MonsterAgent;
    }

    private NavMeshAgent _agent;
    private Transform[] _wayPoints;  // 순찰 위치 설정
    private int _curIndex = 0;
    private float _compareDis = 1f; // 남은 거리가 다 와 갔을 때 해당 값과 비교하여 index 값 전환

    public void Init()
    {
        _wayPoints = monster.PatrolPoints;

        Debug.Log("여긴 들어 왔나");

        for (int i = 0; i < _wayPoints.Length; i++)
        {
            if (_wayPoints[i] == null)
            {
                Debug.Log(" 순찰 포인트를 빠뜨렸습니다. 채워 넣어주세요! ");
                return;
            }
        }
        //  오류 방지
        if (_agent == null || _wayPoints == null || _wayPoints.Length == 0) 
        {
            Debug.Log("뭔가 빼먹었다. - 어젠트, 패트롤 위치, 패트롤 아예 등록을 안했다.");
            return; 
        }

        // 내비 위에 없으면 
        if (!_agent.isOnNavMesh)
        {
            Debug.Log("어젠트가 위애 없다. 없다 어젠트가 내비 위에 없어요 없어 없다고요");
        }

        // 못찾을 경우 경로 초기화/ 다시 처음의 곳으로 돌아가
        _agent.isStopped = false;
        _agent.ResetPath();
        _curIndex = 0;

        MoveToCurrentPoint();
    }




    private void MoveToCurrentPoint()
    {
        if (_agent != null  && _agent.isOnNavMesh)
        {
            _agent.SetDestination(_wayPoints[_curIndex].position);
            Debug.Log($"{_curIndex}번 포인트로 이동 중");
        }
        else
        {
            Debug.LogWarning("[Patrol] 에이전트가 NavMesh 위에 없어 이동 실패");
        }
    }

    public override void Enter()
    {
        Debug.Log("패트롤 모드 진입 성공");
        monster.Ani.SetBool("isPatrol", true);
        Debug.Log(" 애니 문제인가");
        _agent.speed = monster.WalkSpeed;
        Debug.Log("스피드 문제인가");
        Init();
    }

    public override void Update()
    {

        if (!_agent.enabled || !_agent.isOnNavMesh || _wayPoints == null || _wayPoints.Length == 0) return;
        Debug.Log("뭔가 오류는 없다.");
        // 몬스터가 아직 경로를 계산 중이면 대기
        if (_agent.pathPending) return;
        Debug.Log("길 계산 완료");
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

        if (_agent.enabled && _agent.isOnNavMesh)
        {
            _agent.ResetPath();
            monster.Ani.SetBool("isPatrol", false);
        }
    }

}
