using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 Ground 충돌 여부를 감지합니다.
/// </summary>
public class PlayerCollision : MonoBehaviour
{
    public Property<bool> IsGrounded { get; private set; }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        IsGrounded = new Property<bool>(true);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            IsGrounded.Value = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            IsGrounded.Value = false;
        }
    }
}
