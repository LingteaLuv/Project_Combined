using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
public class NpcController : MonoBehaviour
{
    private NpcStateMachine _Nfsm;


    #region State
    public NpcIdleState NpcIdleState { get; private set; }
    public NpcPatrolState NpcPatrolState { get; private set; }
    #endregion

    private void Awake() => Init();
    private void Update() => _Nfsm.Update();
    private void FixedUpdate() => _Nfsm.FixedUpdate();

    private void Init() 
    {
        _Nfsm = new NpcStateMachine();
        NpcIdleState = new NpcIdleState(_Nfsm);
        NpcPatrolState = new NpcPatrolState(_Nfsm);
        _Nfsm.Initialize(NpcIdleState);
    }
}