using EPOOutline.Demo;
using System.Collections;
using UnityEngine;

/// <summary>
/// Rigidbody 기반의 3인칭 캐릭터 이동 및 회전을 처리하는 클래스입니다.
/// 카메라 방향을 기준으로 입력을 변환하고 가속/감속 및 회전을 제어합니다.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovements : MonoBehaviour
{
    #region Serialized Fields

    [Header("References")]

    [SerializeField] private Rigidbody _rigid;
    [SerializeField] private MainCameraController _cameraController;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _acceleration = 15f;
    [SerializeField] private float _deceleration = 20f;

    [Header("Rotation Settings")]
    [SerializeField] private float _mouseSensitivity = 5f;

    #endregion
    #region Properties

    /// <summary> 현재 플레이어가 지면에 접촉해 있는지 여부입니다. </summary>
    public bool IsGrounded { get; private set; }

    #endregion

    #region Private Fields

    private Vector3 _inputDir;

    #endregion
    #region Unity Methods

    private void Awake()
    {
        Init();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// 입력을 받아 방향 벡터를 계산하고 회전을 처리합니다.
    /// Update 루프에서 호출되어야 합니다.
    /// </summary>
    public void InputUpdate()
    {
        InputDirection();
        Rotate();
    }

    /// <summary>
    /// 물리 이동을 처리합니다.
    /// FixedUpdate 루프에서 호출되어야 합니다.
    /// </summary>
    public void MoveUpdate()
    {
        Movement();
    }

    /// <summary>
    /// 지면에 있는 상태를 외부에서 지정합니다.
    /// </summary>
    /// <param name="value">지면 접촉 여부</param>
    public void GetIsGrounded(bool value)
    {
        IsGrounded = value;
    }

    #endregion
    #region Private Methods

    /// <summary>
    /// 키보드 입력을 기반으로 방향 벡터를 계산합니다.
    /// </summary>
    private void InputDirection()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        _inputDir = new Vector3(x, 0, z).normalized;
    }

    /// <summary>
    /// 입력 방향을 기준으로 물리 이동 및 회전을 처리합니다.
    /// </summary>
    private void Movement()
    {
        if (_inputDir != Vector3.zero)
        {
            Vector3 moveDir = _cameraController.flatRotation * _inputDir;
            moveDir.Normalize();

            Vector3 targetV = moveDir * _moveSpeed;
            targetV.y = _rigid.velocity.y;

            _rigid.velocity = Vector3.MoveTowards(_rigid.velocity, targetV, _acceleration * Time.fixedDeltaTime);

            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, _mouseSensitivity * Time.deltaTime);
        }
        else
        {
            Vector3 targetV = new Vector3(0f, _rigid.velocity.y, 0f);
            _rigid.velocity = Vector3.MoveTowards(_rigid.velocity, targetV, _deceleration * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// 마우스 입력을 기반으로 Y축 회전을 수행합니다.
    /// </summary>
    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX * _mouseSensitivity);
    }

    private void Init()
    {
        IsGrounded = true;
    }

    #endregion
}