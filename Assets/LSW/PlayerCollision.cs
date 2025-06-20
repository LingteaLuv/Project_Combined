using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public Property<bool> IsGrounded;

    private void Awake()
    {
        Init();
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Ground"))
        {
            IsGrounded.Value = true;
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        if(other.collider.CompareTag("Ground"))
        {
            IsGrounded.Value = false;
        }
    }

    private void Init()
    {
        IsGrounded = new Property<bool>(true);
    }
}
