using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Temp : MonoBehaviour
{
    [Header("Drag&Drop")] 
    [SerializeField] private Rigidbody _rigid;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;

    [SerializeField] public List<Recipe> recipes
    {
        get;
        private set;
    }
    
    
    
    private Vector3 _inputDir;
    private float _moveSpeed;
    public bool IsGrounded { get; private set; }
    
    private void Awake()
    {
        Init();
    }

    public void InputUpdate()
    {
        InputDirection();
        Rotate();
    }
    public void MoveUpdate()
    {
        Movement();
    }

    public void GetIsGrounded(bool value)
    {
        IsGrounded = value;
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
            targetV.y = _rigid.velocity.y;
            _rigid.velocity = Vector3.MoveTowards(_rigid.velocity, targetV, _acceleration * Time.fixedDeltaTime);
        }
        else
        {
            Vector3 targetV = Vector3.zero;
            targetV.y = _rigid.velocity.y;
            _rigid.velocity = Vector3.MoveTowards(_rigid.velocity, targetV, _deceleration * Time.fixedDeltaTime);
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
        IsGrounded = true;
    }



    public void OnClick()
    {
        // 오전
        // Recipe recipe = GiveRecipe();
        // 제작(recipe)
    }

    public void Craft(Recipe recipe)
    {
        
    }
    
    
}
