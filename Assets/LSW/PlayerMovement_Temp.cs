using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Temp : MonoBehaviour
{
    [Header("Drag&Drop")] 
    [SerializeField] private Rigidbody _rigid;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;
    
    private Vector3 _inputDir;
    private float _moveSpeed;
    
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        InputDirection();
        Rotate();
    }
    private void FixedUpdate()
    {
        Movement();
    }

    private void InputDirection()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        _inputDir = new Vector3(x, 0, z).normalized;
    }
    
    private void Movement()
    {
        if (_inputDir != Vector3.zero)
        {
            Camera _playerFollowCam = Camera.main;
            Vector3 camForward = _playerFollowCam.transform.forward;
            Vector3 camRight = _playerFollowCam.transform.right;

            Vector3 moveDir = camForward * _inputDir.z + camRight * _inputDir.x;
            
            Vector3 targetV = moveDir * _moveSpeed;
            _rigid.velocity = Vector3.MoveTowards(_rigid.velocity, targetV, _acceleration * Time.fixedDeltaTime);
        }
        else
        {
            _rigid.velocity = Vector3.MoveTowards(_rigid.velocity, Vector3.zero, _deceleration * Time.fixedDeltaTime);
        }
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX * 5f);
    }
    
    private void Init()
    {
        _moveSpeed = 5f;
    }
}
