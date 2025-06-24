using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public class Monster_reset : MonsterState_temp
{
    private Vector3 _spawnPoint;
    private NavMeshAgent _agent;
    private Transform _targetPos;
    private MonsterStateMachine_temp _stateMachine;


    public Monster_reset(Monster_temp _monster) : base(_monster)
    {
        _spawnPoint = monster.SpawnPoint.position; // 어디서 스폰할 건지 따로 정하기
        _agent = monster.MonsterAgent;
        _targetPos = monster.TargetPosition;
        _stateMachine = monster._monsterMerchine;
    }

    // 코루틴으로 잠시 정지하고 원 모션으으로 넘어가는데.... 이거 idle 자체가 멈추는 모션으로 지정된거라 굳이 코루틴 안써도?
    // 이거 한번 확인해 보고
    private System.Collections.IEnumerator DelayTransition()
    {
        yield return new WaitForSeconds(0.5f);
        _stateMachine.ChangeState(_stateMachine.StateDic[Estate.Idle]);
    }

    public override void Enter()
    {
        // reset상태에서 혹시라도 플레이어와의 감지를 없애기 위해서 감지 콜라이더 비활성화
        monster.DetectRange.enabled = false;

        // 만약 경로도 있고, 남은 거리도 있는데 속도가 없으면 -> 경로가 꼬인 상태로 가정
        // 그렇다면 경로를 초기화 시키고 다시
        if (_agent.hasPath && _agent.remainingDistance > 0 && _agent.velocity.sqrMagnitude < 0.01f)
        {
            _agent.ResetPath();
            _agent.SetDestination(_targetPos.position);
        }


        //  어젠트가 내비 위에 없거나 내비 경로가 애초에 없는 경우 모든종류의 내비 탐색후 스폰했던 장소로 순간이동.
        if (!_agent.isOnNavMesh || _agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            if (NavMesh.SamplePosition(_spawnPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                _agent.Warp(hit.position);
            }

            // !_agent.isOnNavMesh 상태에 의한 비활성화 상태를 활성화 상태로 전환하고 경로 초기화 후 
            _agent.enabled = true;
            _agent.ResetPath();
        }
        // 바로 idle로 돌아가 탐색 진행을 방지하기 위한 안전성용 코루틴
        monster.StartCoroutine(DelayTransition());
    }

    public override void Exit()
    {
        // reset상태에서 빠져나가는 경우 다시 플레이어를 감지할 수 있도록 콜라이더 켜주기
        monster.DetectRange.enabled = true;
    }
}
