using EPOOutline.Demo;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovements : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody _rigid;
    [SerializeField] private MainCameraController _cameraController;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _acceleration = 15f;
    [SerializeField] private float _deceleration = 20f;

    [Header("Rotation Settings")]
    [SerializeField] private float _mouseSensitivity = 5f;

    public bool IsGrounded { get; private set; }

    private Vector3 _inputDir;

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
            Vector3 moveDir = _cameraController.flatRotation * _inputDir;
            moveDir.Normalize();

            Vector3 targetV = moveDir * _moveSpeed;
            targetV.y = _rigid.velocity.y;

            _rigid.velocity = Vector3.MoveTowards(_rigid.velocity, targetV, _acceleration * Time.fixedDeltaTime);

            // 회전도 방향 기준으로 처리
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, _mouseSensitivity * Time.deltaTime);
        }
        else
        {
            Vector3 targetV = new Vector3(0f, _rigid.velocity.y, 0f);
            _rigid.velocity = Vector3.MoveTowards(_rigid.velocity, targetV, _deceleration * Time.fixedDeltaTime);
        }
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX * _mouseSensitivity);
    }

    private void Init()
    {
        IsGrounded = true;
    }
}