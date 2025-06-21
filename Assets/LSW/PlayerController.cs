using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rigidbody 기반의 간단한 플레이어 이동 컨트롤러입니다.
/// WASD 입력으로 이동하고 마우스 입력으로 회전합니다.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("References")]
    [SerializeField] private Animator animator;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Rotate();
        Animate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(h, 0f, v).normalized;

        Vector3 velocity = transform.TransformDirection(moveDir) * moveSpeed;
        velocity.y = _rb.velocity.y;

        _rb.velocity = velocity;
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX * rotationSpeed);
    }

    private void Animate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float movement = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        animator.SetFloat("movementValue", movement, 0.1f, Time.deltaTime);
    }
}