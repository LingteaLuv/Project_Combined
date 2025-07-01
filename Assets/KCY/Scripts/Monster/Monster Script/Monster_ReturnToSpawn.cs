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
    private Transform _spawnPoint;
    private float _almostRp = 1f;


    public override void Enter()
    {
        Debug.Log("스폰지역으로 다시 돌아가는 중입니다.");

        // 고개는 돌리고 가자
        Vector3 dir = (_spawnPoint.position - monster.transform.position).normalized;
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
            _agent.SetDestination(_spawnPoint.position);
        }

        if (monster.Ani != null)
        {
            monster.Ani.SetBool("isPatrol", true);
            monster.Ani.SetBool("isChasing", false);
        }
    }


    public override void Update()
    {
        if (_agent == null || !_agent.isOnNavMesh)
        {
            Debug.Log(" Return to Respawan 함수 내에서 에이전트 없거나 내비위가 아님을 감지하여 리셋으로 갑니다.");
            monster._monsterMerchine.ChangeState(monster._monsterMerchine.StateDic[Estate.Reset]);
            return;

        } 

        // 길 연산이 끝났고, 남은 거리가 거의 다왔다
        if (!_agent.pathPending && _agent.remainingDistance <= _almostRp)
        {
            Debug.Log("집이다 집이야");
            monster.TempPoint = null; // 초기화 시켜주기
            monster._monsterMerchine.ChangeState(monster._monsterMerchine.StateDic[Estate.Patrol]);
        }
    }

    // 나가면 혹시 담길 길 정보는 초기화 시켜주자
    public override void Exit()
    {
        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.ResetPath();
        }
    }


}
