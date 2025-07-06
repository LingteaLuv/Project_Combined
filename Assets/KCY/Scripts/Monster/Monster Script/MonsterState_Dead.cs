using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Monster_Dead : MonsterState_temp
{
    public Monster_Dead(Monster_temp _monster) : base(_monster)
    {
        _ani = monster.Ani;
        _agent = monster.MonsterAgent;
        stateMerchine = monster._monsterMerchine;
        _obj = monster.MonObject;
        _lootable = monster.lootable;
    }

    private int _hp;
    private NavMeshAgent _agent;
    private Animator _ani;
    private GameObject _obj;
    protected MonsterStateMachine_temp stateMerchine;
    private Lootable _lootable;


    public override void Enter()
    {
        Debug.Log("몬스터 죽음  몬스터 죽음 몬스터 죽음 몬스터 죽음 몬스터 죽음");

        // 죽으면 멈춰서 애니 실행


        // 루터블 트루
        _lootable.IsLootable = true;
        // 루터블 트루



        _ani.SetBool("isPatrol", false);
        _ani.SetBool("isChasing", false);
        _agent.velocity = Vector3.zero;
        _agent.isStopped = true;
        _ani.SetTrigger("Dead");
        monster.SightCol.enabled = false;
        monster.HearingCol.enabled = false;
        _agent.enabled = false;

        //Object.Destroy(_obj,15f);      몬스터 소멸을 원하시는 경우에만 쓰고 아니면 해당 코드는 지우기
    }

}
