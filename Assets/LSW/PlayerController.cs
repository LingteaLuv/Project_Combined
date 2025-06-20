using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerCollision _playerCollision;
    private PlayerClimb _playerClimb;
    private PlayerMovement_Temp _playerMovementTemp;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        _playerCollision.IsGrounded.OnChanged += _playerMovementTemp.GetIsGrounded;
    }

    private void Update()
    {
        if (!_playerClimb.IsOnClimbed)
        {
            _playerMovementTemp.InputUpdate();
        }
        _playerClimb.ClimbUpdate(_playerMovementTemp.IsGrounded);
    }
    
    private void FixedUpdate()
    {
        if (!_playerClimb.IsOnClimbed)
        {
            _playerMovementTemp.MoveUpdate();
        }
    }

    private void Init()
    {
        _playerClimb = GetComponent<PlayerClimb>();
        _playerMovementTemp = GetComponent<PlayerMovement_Temp>();
        _playerCollision = GetComponentInChildren<PlayerCollision>();
    }
}
