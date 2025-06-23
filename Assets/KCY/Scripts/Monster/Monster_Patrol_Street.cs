using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Patrol_Street : MonsterState_temp
{
    private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _targetTransform; // 플레이어의 포지션 갖고와서 확인

    public override void Enter()
    { 

    }

    private void Awake()
    {
       
    }


}
