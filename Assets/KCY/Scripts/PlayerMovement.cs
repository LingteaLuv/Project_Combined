using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 플레이어의 이동과 점프를 처리합니다.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private PlayerProperty _property;
    
    public PlayerClimb PlayerClimbHandler { get; private set; } 
    public PlayerController Controller { get; set; }
    public Rigidbody Rigidbody { get; private set; }
    public bool IsOnLadder { get; private set; }
    public bool IsGrounded { get; private set; }

    private bool _jumpConsumedThisFrame;
    private bool _isCrouching;
    private Vector3 _currentRotation;

    [Header("Settings")]
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _groundCheckDistance = 0.05f;
    [SerializeField] private float _crouchSpeedMultiplier = 0.5f;
    [SerializeField] private float _fallMultiplier = 5f;

    public Vector3 MoveInput => _inputHandler.MoveInput;
    public bool JumpPressed => _inputHandler.JumpPressed;
    public bool CrouchHeld => _inputHandler.CrouchHeld;
    public bool InteractPressed => _inputHandler.InteractPressed;

    private void Awake() => Init();

    private void Init()
    {
        Rigidbody = GetComponent<Rigidbody>();
        PlayerClimbHandler = GetComponent<PlayerClimb>();
    }

    private void Update()
    {
        _jumpConsumedThisFrame = false;

        // Raycast로 지면 체크
        IsGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, _groundCheckDistance + 0.1f);
        IsOnLadder = _inputHandler.IsOnLadder;
    }

    private void FixedUpdate()
    {
        HandleMovement(MoveInput); // 이동 처리
        HandleGravity(); // 중력 처리
    }

    public void HandleMovement(Vector3 inputDir)
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // 카메라 기준 이동 벡터
        Vector3 moveDir = cam.transform.forward * inputDir.z + cam.transform.right * inputDir.x;
        moveDir.y = 0f;
        moveDir.Normalize();

        if (inputDir.sqrMagnitude < 0.01f)
        {
            // 입력이 거의 없을 경우 멈춤 처리
            Vector3 stopVelocity = new Vector3(0, Rigidbody.velocity.y, 0);
            Rigidbody.velocity = Vector3.MoveTowards(Rigidbody.velocity, stopVelocity, 100 * Time.fixedDeltaTime);
        }
        else
        {
            // 경사면 보정 이동
            moveDir = GetSlopeAdjustedMoveDirection(moveDir);

            float speed = _property.MoveSpeed.Value * (_isCrouching ? _crouchSpeedMultiplier : 1f);
            Vector3 targetVelocity = moveDir * speed;
            targetVelocity.y = Rigidbody.velocity.y; // 수직 속도 유지
            Rigidbody.velocity = Vector3.MoveTowards(Rigidbody.velocity, targetVelocity, 100 * Time.fixedDeltaTime);

            // 이동 방향 회전
            if (moveDir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
            }

            // 작은 턱 오르기 처리
            bool downRay = Physics.Raycast(transform.position + Vector3.up * 0.01f, transform.forward, 0.5f);
            bool middleRay = Physics.Raycast(transform.position + Vector3.up * 0.1f, transform.forward, 0.5f);
            bool upRay = Physics.Raycast(transform.position + Vector3.up * 0.3f, transform.forward, 0.5f);

            if (downRay && middleRay && !upRay)
            {
                Rigidbody.position += Vector3.up * 0.1f;
            }

            // 경사면에 붙도록 아래 방향 힘 가함
            if (IsGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, _groundCheckDistance + 0.3f))
            {
                if (slopeHit.normal != Vector3.up && Vector3.Angle(slopeHit.normal, Vector3.up) <= 45f)
                {
                    Rigidbody.AddForce(-slopeHit.normal * 10f, ForceMode.Force);
                }
            }
        }
    }

    private void HandleGravity()
    {
        if (!IsGrounded && Rigidbody.velocity.y < 0f)
        {
            Rigidbody.velocity += Vector3.up * Physics.gravity.y * (_fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }

    private Vector3 GetSlopeAdjustedMoveDirection(Vector3 moveDir)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _groundCheckDistance + 0.3f))
        {
            Vector3 normal = hit.normal;
            if (normal != Vector3.up)
            {
                return Vector3.ProjectOnPlane(moveDir, normal).normalized;
            }
        }
        return moveDir;
    }

    public bool CanJump()
    {
        return !_jumpConsumedThisFrame && JumpPressed && IsGrounded;
    }

    public void Jump()
    {
        _jumpConsumedThisFrame = true;
        Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    public void SetCrouch(bool crouch)
    {
        _isCrouching = crouch;
    }

    public void SetRotation(float offset)
    {
        transform.rotation = Quaternion.Euler(0f, offset, 0f);
    }

    public float GetAnimatorSpeedMultiplier()
    {
        return Mathf.Clamp01(MoveInput.magnitude) * (_property?.MoveSpeed?.Value ?? 0f) + 1;
    }

    public void SetGravity(bool enabled)
    {
        Rigidbody.useGravity = enabled;
    }
}