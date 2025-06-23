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
    [SerializeField] private Transform _aim;

    public PlayerController Controller { get; set; }
    private Rigidbody _rb;
    private bool _jumpConsumedThisFrame;

    private Vector2 _currentRotation;
    private bool _isCrouching;

    [Header("Settings")]
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _groundCheckDistance = 0.05f;
    [SerializeField] private float _crouchSpeedMultiplier = 0.5f;

    public Vector3 MoveInput => _inputHandler.MoveInput;
    public bool JumpPressed => _inputHandler.JumpPressed;
    public bool CrouchHeld => _inputHandler.CrouchHeld;
    public bool IsGrounded => Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, _groundCheckDistance + 0.1f);


    private void Init()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Awake() => Init();

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

        if (_property.MoveSpeed.Value == 0)
        {
            _property.MoveSpeed.Value = 3;
        }

        Camera cam = Camera.main;
        if (cam == null) return;

        Vector3 moveDir = cam.transform.forward * inputDir.z + cam.transform.right * inputDir.x;
        moveDir.y = 0f;
        moveDir.Normalize();

        float speed = _property.MoveSpeed.Value * (_isCrouching ? _crouchSpeedMultiplier : 1f);
        transform.position += moveDir * speed * Time.deltaTime;

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
    public void SetCrouch(bool crouch)
    {
        _isCrouching = crouch;
    }

    public void SetRotation(Vector2 rotation)
    {
        _currentRotation = rotation;

        transform.rotation = Quaternion.Euler(0f, rotation.x, 0f);

        if (_aim != null)
        {
            Vector3 aimEuler = _aim.localEulerAngles;
            _aim.localEulerAngles = new Vector3(rotation.y, aimEuler.y, aimEuler.z);
        }
    }
    //TODO : 애니메이션 스피드 조정용 코드 추가 예정
    public float GetAnimatorSpeedMultiplier()
    {
        return Mathf.Clamp01(MoveInput.magnitude) * (_property?.MoveSpeed?.Value ?? 0f)+1;
    }
}