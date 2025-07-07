using System.Collections;
using UnityEngine;

/// <summary>
/// 플레이어의 이동과 점프를 처리합니다.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] public CapsuleCollider CrouchCollider;
    [SerializeField] private SphereCollider _stateSphereCollider;

    [Header("Settings")]
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _groundCheckDistance = 0.05f;

    private float StepCheckHeight = 0.1f;
    private float StepCheckMid = 0.15f;
    private float StepCheckUp = 0.3f;
    private float StepCheckDistance = 0.2f;
    private float SlopeMaxAngle = 45f;
    private float SlopeForce = 10f;
    private float WaterSpeedMultiplier = 0.6f;
    private float FallMultiplier = 5f;
    private float _lastYPos = 0f;
    private float _yStableTime = 0f;
    private float _yStableThreshold = 0.02f;    
    private float _kneeHeight = 0.5f;        
    private float _stepUpAmount = 0.15f;   
    private float _stableTimeRequired = 1f;  

    public PlayerClimb PlayerClimbHandler { get; private set; }
    public PlayerController Controller { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public PlayerProperty Property { get; private set; }
    public SphereCollider StateSphereCollider => _stateSphereCollider;

    public bool IsRunning => _inputHandler.RunPressed && _inputHandler.MoveInput.z > 0.1f && CanRun;
    public bool CanRun => Property.Stamina.Value >= 5f && !Property.IsDied;

    public Vector3 MoveInput => _inputHandler.MoveInput;
    public bool JumpPressed => _inputHandler.JumpPressed;
    public bool CrouchHeld => _inputHandler.CrouchHeld;
    public bool InteractPressed => _inputHandler.InteractPressed;

    public bool IsOnLadder { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsWater { get; private set; }
    public bool IsConsumingStamina { get; private set; }
    public bool CanMove { get; private set; } = true;

    private bool _jumpConsumedThisFrame;
    private bool _isCrouching;
    private Coroutine _hitInWaterCoroutine;
    private WaitForSeconds _delay;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        PlayerClimbHandler = GetComponent<PlayerClimb>();
        Controller = GetComponent<PlayerController>();
        Property = GetComponent<PlayerProperty>();
        _delay = new WaitForSeconds(1f);
    }
    private void Update()
    {
        if (Property.IsDied) return;

        _jumpConsumedThisFrame = false;
        UpdateGroundAndLadderState();
        TryStartStaminaCoroutine();
    }
    private void UpdateGroundAndLadderState()
    {
        IsGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, _groundCheckDistance + 0.1f);
        IsOnLadder = _inputHandler.IsOnLadder;
    }
    private void TryStartStaminaCoroutine()
    {
        if (IsRunning && !IsConsumingStamina)
            StartCoroutine(StaminaConsumePerSecond());
    }
    private IEnumerator StaminaConsumePerSecond()
    {
        IsConsumingStamina = true;
        while (IsRunning)
        {
            if (Property.Stamina.Value < 5f)
                break;
            Property.StaminaConsume(Property.StaminaCostRun);
            yield return _delay;
        }
        IsConsumingStamina = false;
    }
    private void FixedUpdate()
    {
        if (Property.IsDied) return;

        if (!IsOnLadder && IsGrounded)
            HandleMovement(MoveInput);

        if (!IsOnLadder)
            HandleGravity();
    }
    public void HandleMovement(Vector3 inputDir)
    {
        if (!CanMove) return;
        var cam = Camera.main;
        if (cam == null) return;

        Vector3 moveDir = cam.transform.forward * inputDir.z + cam.transform.right * inputDir.x;
        moveDir.y = 0f;
        moveDir.Normalize();

        if (inputDir.sqrMagnitude < 0.01f)
        {
            Rigidbody.velocity = Vector3.MoveTowards(Rigidbody.velocity, new Vector3(0, Rigidbody.velocity.y, 0), 100 * Time.fixedDeltaTime);
            return;
        }

        // 경사면 보정
        moveDir = GetSlopeAdjustedMoveDirection(moveDir);

        // 속도 계산 공식
        float speed = Property.MoveSpeed.Value
                      * (_isCrouching ? Property.CrouchSpeed : 1f)
                      * (IsWater ? WaterSpeedMultiplier : 1f)
                      * (IsRunning ? Property.RunSpeed : 1f);

        Vector3 targetVelocity = moveDir * speed;
        targetVelocity.y = Rigidbody.velocity.y;
        Rigidbody.velocity = Vector3.MoveTowards(Rigidbody.velocity, targetVelocity, 100 * Time.fixedDeltaTime);

        // 이동 방향 회전
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }

        // 턱 오르기
        if (CheckRay(transform.position, transform.forward, StepCheckDistance, StepCheckHeight)
            && CheckRay(transform.position, transform.forward, StepCheckDistance, StepCheckMid)
            && !CheckRay(transform.position, transform.forward, StepCheckDistance, StepCheckUp))
        {
            Rigidbody.position += Vector3.up * 0.1f;
        }



        // 경사면에서 미끄러짐 방지
        if (IsGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, _groundCheckDistance + 0.3f))
        {
            if (slopeHit.normal != Vector3.up && Vector3.Angle(slopeHit.normal, Vector3.up) <= SlopeMaxAngle)
            {
                Rigidbody.AddForce(-slopeHit.normal * SlopeForce, ForceMode.Force);
            }
        }
    }

    private bool CheckRay(Vector3 origin, Vector3 direction, float distance, float heightOffset = 0f)
    {
        return Physics.Raycast(origin + Vector3.up * heightOffset, direction, distance);
    }
    private void HandleGravity()
    {
        if (!IsGrounded && Rigidbody.velocity.y < 0.1f)
        {
            float wallCheckDistance = 0.5f;
            Vector3 origin = transform.position + Vector3.up * 0.5f;
            Vector3[] directions = { transform.forward, -transform.forward, transform.right, -transform.right };

            foreach (var dir in directions)
            {
                if (Physics.Raycast(origin, dir, out RaycastHit hit, wallCheckDistance, LayerMask.GetMask("Default")))
                {
                    Rigidbody.velocity = new Vector3(0f, Rigidbody.velocity.y, 0f) + hit.normal * 4f;
                    return;
                }
            }

            foreach (var dir in directions)
            {
                if (Physics.Raycast(origin, dir, out RaycastHit hit, wallCheckDistance, LayerMask.GetMask("Default")))
                {
                    Vector3 gravityDownWall = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
                    Rigidbody.velocity += gravityDownWall * Physics.gravity.y * (FallMultiplier - 1f) * Time.fixedDeltaTime;
                    return;
                }
            }
            Rigidbody.velocity += Vector3.up * Physics.gravity.y * (FallMultiplier - 1f) * Time.fixedDeltaTime;
        }

        // ------ y 고정 & 무릎 Ray 보정 (떨어지는 중에만) ------
        float currentY = transform.position.y;

        // 떨어지는 중에 y 변화가 없으면 누적, 아니면 리셋
        bool isFalling = !IsGrounded && Rigidbody.velocity.y < 0.1f;
        if (isFalling && Mathf.Abs(currentY - _lastYPos) < _yStableThreshold)
        {
            _yStableTime += Time.fixedDeltaTime;
            if (_yStableTime >= _stableTimeRequired)
            {
                Vector3 kneeOrigin = transform.position + Vector3.up * _kneeHeight;
                bool frontClear = !Physics.Raycast(kneeOrigin, transform.forward, 0.25f);
                if (frontClear)
                {
                    Rigidbody.position += Vector3.up * _stepUpAmount;
                    _yStableTime = 0f;
                }
            }
        }
        else
        {
            _yStableTime = 0f;
        }
        _lastYPos = currentY;
    }

    private Vector3 GetSlopeAdjustedMoveDirection(Vector3 moveDir)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _groundCheckDistance + 0.3f))
        {
            Vector3 normal = hit.normal;
            if (normal != Vector3.up)
                return Vector3.ProjectOnPlane(moveDir, normal).normalized;
        }
        return moveDir;
    }
    public bool CanJump()
    {
        if (Property.Stamina.Value < 10f)
            return false;
        return CanMove && !_jumpConsumedThisFrame && JumpPressed && IsGrounded && !IsJumpAnimationPlaying();
    }

    public void Jump()
    {
        if (Property.Stamina.Value < 10f) return;
        _jumpConsumedThisFrame = true;
        Property.StaminaConsume(Property.StaminaCostJump);
        Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    public void SetCrouch(bool crouch) => _isCrouching = crouch;
    public void SetWater(bool water)
    {
        IsWater = water;
        if (water)
        {
            _hitInWaterCoroutine = StartCoroutine(Property.HitInWater(20));
        }
        else
        {
            if (_hitInWaterCoroutine != null)
            {
                StopCoroutine(_hitInWaterCoroutine);
                _hitInWaterCoroutine = null;
            }
        }
    }
    public void SetRotation(float offset) => transform.rotation = Quaternion.Euler(0f, offset, 0f);

    public bool IsJumpAnimationPlaying() => Controller._animator.GetCurrentAnimatorStateInfo(0).IsName("Jump");
    public void SetStateColliderRadius(float radius)
    {
        if (StateSphereCollider != null)
            StateSphereCollider.radius = radius;
    }
    public void SetGravity(bool enabled) => Rigidbody.useGravity = enabled;
    public void MoveLock() => CanMove = !CanMove;
}
