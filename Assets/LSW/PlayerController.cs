using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerCollision _playerCollision;
    private PlayerClimb _playerClimb;
    private PlayerInput _playerInput;
    private PlayerMovement _playerMovement;

    private Vector3 _moveDir;
    
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        _playerCollision.IsGrounded.OnChanged += _playerMovement.GetIsGrounded;
        _playerCollision.IsGrounded.OnChanged += _playerMovement.GetIsJumped;
    }

    private void Update()
    {
        if (!_playerClimb.IsOnClimbed)
        {
            _playerMovement.Rotate();
            _moveDir = _playerInput.GetInputDir();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _playerMovement.Jump();
            }
        }
        _playerClimb.ClimbUpdate(_playerMovement.IsGrounded);
    }
    
    private void FixedUpdate()
    {
        if (!_playerClimb.IsOnClimbed)
        {
            _playerMovement.SetMove(_moveDir);
        }
    }

    private void Init()
    {
        _playerClimb = GetComponent<PlayerClimb>();
        _playerCollision = GetComponentInChildren<PlayerCollision>();
        _playerInput = GetComponent<PlayerInput>();
        _playerMovement = GetComponent<PlayerMovement>();
    }
}
