using UnityEngine;

/// <summary>
/// 플레이어의 이동과 점프를 처리합니다.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private PlayerProperty _property;

    public PlayerController Controller { get; set; }

    private Rigidbody _rb;
    private bool _jumpConsumedThisFrame;

    [Header("Settings")]
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _groundCheckDistance = 0.05f;

    public Vector3 MoveInput => _inputHandler.MoveInput;
    public bool JumpPressed => _inputHandler.JumpPressed;
    public bool IsGrounded => Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, _groundCheckDistance + 0.1f);



    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _jumpConsumedThisFrame = false;
    }
    /// <summary>
    /// 입력 방향에 따라 관성 없이 이동합니다.
    /// </summary>
    public void Move(Vector3 inputDir)
    {
        if (inputDir == Vector3.zero) return;

        Camera cam = Camera.main;
        if (cam == null) return;

        Vector3 moveDir = cam.transform.forward * inputDir.z + cam.transform.right * inputDir.x;
        moveDir.y = 0f;
        moveDir.Normalize();

        transform.position += moveDir * _property.MoveSpeed.Value * Time.deltaTime;

        Quaternion targetRot = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
    }
    public bool CanJump()
    {
        return !_jumpConsumedThisFrame && JumpPressed && IsGrounded;
    }

    /// <summary>
    /// Rigidbody를 이용해 점프를 실행합니다.
    /// </summary>
    public void Jump()
    {
        _jumpConsumedThisFrame = true;
        Vector3 velocity = _rb.velocity;
        velocity.y = _jumpForce;
        _rb.velocity = velocity;
    }
}