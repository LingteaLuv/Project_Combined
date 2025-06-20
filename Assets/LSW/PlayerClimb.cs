using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    [Header("Drag&Drop")] 
    [SerializeField] private float _climbSpeed;

    [SerializeField] private Rigidbody _rigid;
    [SerializeField] private Animator _animator;
    
    public bool IsOnClimbed
    {
        get;
        private set;
    }
    
    private void Awake()
    {
        Init();
    }

    public void ClimbUpdate(bool isOnGround)
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 0.6f))
        {
            if(hit.collider.CompareTag("Ladder"))
            {
                IsOnClimbed = true;
                _rigid.useGravity = false;
                _animator.SetBool("IsClimb", true);
                Climb(isOnGround);
            }
            else
            {
                IsOnClimbed = false; 
                _rigid.useGravity = true;
                _animator.SetBool("IsClimb", false);
            }
        }
        else
        {
            IsOnClimbed = false; 
            _rigid.useGravity = true;
            _animator.SetBool("IsClimb", false);
        }
    }

    private void Climb(bool isOnGround)
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * (_climbSpeed * Time.deltaTime);
            
        }
        _animator.speed = Mathf.Abs(Input.GetAxis("Vertical"));
        if (Input.GetKey(KeyCode.S))
        {
            if (isOnGround)
            {
                transform.position -= transform.forward * (_climbSpeed * Time.deltaTime);
            }
            else
            {
                transform.position += Vector3.down * (_climbSpeed * Time.deltaTime);
            }
        }
    }
    
    private void Init()
    {
        IsOnClimbed = false;
    }
}
