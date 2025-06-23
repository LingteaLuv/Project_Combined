using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster_temp : MonoBehaviour
{
    [Header("Chase Element")]
    [SerializeField] public NavMeshAgent MonsterAgent;
    [SerializeField] public Transform TargetPosition;
    [SerializeField] public LayerMask PlayerLayerMask;

    // public Animator Ani;
    // public Rigidbody Rigid; 
    private MonsterStateMachine_temp _monsterMerchine;




    void Start()
    {



        _monsterMerchine.StateDic = new Dictionary<Estate, BaseState_temp> { { Estate.Idle, idle }, { Estate.Chase, chase } };
    }



    void Update()
    {
        _monsterMerchine.Update();
    }
    void FixedUpdate()
    {
        _monsterMerchine.FixedUpdate();
    }
}
