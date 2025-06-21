using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Components

    private PlayerInput _playerInput;
    private PlayerMovement _playerMovement;
    private Animator _animator;
    private Rigidbody _rigid;

    #endregion

    private Vector3 _moveDir;
    public PlayerInput PlayerInput { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }

    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }


    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        StateMachine = new PlayerStateMachine();
        StateMachine.ChangeState(new IdleState(this)); // 초기 상태: Idle
    }

    private void Update()
    {
        StateMachine?.FixedUpdate();
    }
    
    private void FixedUpdate()
    {
        StateMachine?.FixedUpdate();
    }


    #region Initialization
    private void Init()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerMovement = GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody>();
    }
    #endregion
}
