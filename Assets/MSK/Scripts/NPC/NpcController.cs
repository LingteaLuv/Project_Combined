using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
public class NpcController : MonoBehaviour
{
    private NpcStateMachine _Nsm;
    public NpcIdleState NpcIdleState { get; private set; }

    private void Awake()
    {

    }

    private void Update() => _Nsm.Update();
    private void FixedUpdate() => _Nsm.FixedUpdate();

    private void Init() 
    {
        _Nsm = new NpcStateMachine();
        NpcIdleState = new NpcIdleState(_Nsm);
        _Nsm.Initialize(NpcIdleState);
    }
}