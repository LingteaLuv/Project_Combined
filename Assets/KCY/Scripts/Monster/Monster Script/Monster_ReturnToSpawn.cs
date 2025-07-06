using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_ReturnToSpawn : MonsterState_temp
{
    public Monster_ReturnToSpawn(Monster_temp _monster) : base(_monster)
    {
        _agent = monster.MonsterAgent;
        _spawnPoint = monster.SpawnPoint;

    }

    private NavMeshAgent _agent;
    private Vector3 _spawnPoint;
    private float _almostRp = 0.5f;


    public override void Enter()
    {
        //Debug.Log("스폰지역으로 다시 돌아가는 중입니다.");
        //Debug.Log($"[ReturnToSpawn:Enter] 현재 SpawnPoint 위치: {monster.SpawnPoint}");

        // 고개는 돌리고 가자
        Vector3 dir = (_spawnPoint - monster.transform.position).normalized;
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            monster.transform.rotation = rot;
        }

        // 패트롤 애니 그대로 송출
        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.isStopped = false;
            _agent.ResetPath();
            _agent.SetDestination(monster.SpawnPoint);
        }

        if (monster.Ani != null)
        {
            monster.Ani.SetBool("isPatrol", true);
            monster.Ani.SetBool("isChasing", false);
        }

        monster.TempPoint = Vector3.zero;
    }


    public override void Update()
    {
        //Debug.Log("호출 확인용,호출 확인용,호출 확인용,호출 확인용,호출 확인용,호출 확인용,호출 확인용");

        if (_agent == null || !_agent.isOnNavMesh)
        {
            //Debug.Log(" Return to Respawan 함수 내에서 에이전트 없거나 내비위가 아님을 감지하여 리셋으로 갑니다.");
            monster._monsterMerchine.ChangeState(monster._monsterMerchine.StateDic[Estate.Reset]);
            return;

        } 

        // 길 연산이 끝났고, 남은 거리가 거의 다왔다 , + 속도 offset값 추가
        if (!_agent.pathPending && _agent.remainingDistance <= _almostRp && _agent.velocity.sqrMagnitude < 0.5f)
        {
            //Debug.Log("집이다 집이야");
            //Debug.Log($"[도착체크] 거리: {_agent.remainingDistance}, 속도: {_agent.velocity.sqrMagnitude}");
            monster.TempPoint = Vector3.zero; // 초기화 시켜주기
            monster._monsterMerchine.ChangeState(monster._monsterMerchine.StateDic[Estate.Idle]);
        }
    }

    // 나가면 혹시 담길 길 정보는 초기화 시켜주자
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
