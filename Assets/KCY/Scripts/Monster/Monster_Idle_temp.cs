using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Idle : MonsterState_temp
{

    private float _idleTimer;
    private float _idleDuration = 2f;
    private NavMeshAgent _navMeshAgent;
    protected MonsterStateMachine_temp stateMachine;

    public override void Enter()
    {
        _idleTimer = 0f;
        _navMeshAgent.enabled = false;  // idle 상태에 진입한 경우 잠시 추적을 종료하고 가만히 있으면서 애니메이션먄 출력 
       // monster.animator.Play()   ... monster클래스는 몬스터의 기본 상태 및 애니메이션 정보가 있다고 가정하고, play()를 통해 애니메이션 불러옴 (이때 애니메이션을 불러오는건 해쉬값을 이용) 
    }

    public override void Update()
    {
        // 쿨타임이 지나면 다시 패트롤 모드로 돌아가기
        _idleTimer += Time.deltaTime;
        if (_idleTimer >= _idleDuration)
        {
            stateMachine.ChangeState(stateMachine.StateDic[Estate.Patrol_Street]);
        }
    }


}
