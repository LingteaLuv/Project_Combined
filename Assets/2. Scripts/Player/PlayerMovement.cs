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

    public PlayerClimb PlayerClimbHandler { get; private set; }
    public PlayerController Controller { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public PlayerProperty Property { get; private set; }
    #region Public
    public bool IsOnLadder { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsWater { get; private set; }
    public bool IsRunning { get; private set; }
    public bool CanMove { get; private set; }
    public bool CanRun { get; private set; }
    public bool IsConsumingStamina { get; private set; }
    public Vector3 MoveInput => _inputHandler.MoveInput;
    public bool JumpPressed => _inputHandler.JumpPressed;
    public bool CrouchHeld => _inputHandler.CrouchHeld;
    public bool InteractPressed => _inputHandler.InteractPressed;

    public SphereCollider StateSphereCollider => _stateSphereCollider;
    #endregion

    private bool _jumpConsumedThisFrame;
    private bool _isCrouching;
    private WaitForSeconds _delay;
    private Vector3 _currentRotation;

    private float _waterSpeedMultiplier = 0.6f;
    private float _fallMultiplier = 5f;

    private void Awake() => Init();

    private void Init()
    {
        Rigidbody = GetComponent<Rigidbody>();
        PlayerClimbHandler = GetComponent<PlayerClimb>();
        Controller = GetComponent<PlayerController>();
        Property = GetComponent<PlayerProperty>();

        CanMove = true;
        _delay = new WaitForSeconds(1f);
    }

    private void Update()
    {
        _jumpConsumedThisFrame = false;

        // Raycast로 지면 체크
        IsGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, _groundCheckDistance + 0.1f);
        IsOnLadder = _inputHandler.IsOnLadder;
        IsRunning = _inputHandler.RunPressed;

        if (Property.Stamina.Value < 5f)
            CanRun = false;
        else
            CanRun = true;

        if (IsRunning && !IsConsumingStamina && (Property.Stamina.Value > 5f))
            StartCoroutine(StaminaConsumePerSecond());
    }

    private IEnumerator StaminaConsumePerSecond()
    {
        IsConsumingStamina = true;
        while (true)
        {
            if (!IsRunning) break;
            Property.StaminaConsume(Property.StaminaCostRun);
            yield return _delay;
        }
        IsConsumingStamina = false;
    }

    private void FixedUpdate()
    {
        if (!IsOnLadder || IsGrounded)
        {
            HandleMovement(MoveInput); // 이동 처리
            HandleGravity();
        }
    }

    public void HandleMovement(Vector3 inputDir)
    {
        Camera cam = Camera.main;
        if (!CanMove)   //  CanMove가 False면 이동 제한
            return;
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
  
            //  속도 계산 공식 
            float speed = Property.MoveSpeed.Value
                * (_isCrouching ? Property.CrouchSpeed : 1f)
                * (IsWater ? _waterSpeedMultiplier : 1f)
                * (IsRunning && (Property.Stamina.Value > 5f) ? Property.RunSpeed : 1f);

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
            bool downRay = Physics.Raycast(transform.position + Vector3.up * 0.1f, transform.forward, 0.2f);
            bool middleRay = Physics.Raycast(transform.position + Vector3.up * 0.15f, transform.forward, 0.2f);
            bool upRay = Physics.Raycast(transform.position + Vector3.up * 0.3f, transform.forward, 0.2f);

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

    //  중력 관련 
    private void HandleGravity()
    {
        if (!IsGrounded && Rigidbody.velocity.y < 0.1f)
        {
            float wallCheckDistance = 0.5f;
            Vector3 origin = transform.position + Vector3.up * 0.5f;
            Vector3[] directions = {
            transform.forward, -transform.forward,
            transform.right, -transform.right
        };

            foreach (var dir in directions)
            {
                if (Physics.Raycast(origin, dir, out RaycastHit hit, wallCheckDistance, LayerMask.GetMask("Default")))
                {
                    float pushStrength = 4f;
                    Rigidbody.velocity = new Vector3(0f, Rigidbody.velocity.y, 0f) + hit.normal * pushStrength;
                    return;
                }
            }
            Rigidbody.velocity += Vector3.up * Physics.gravity.y * (_fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }


    //  경사면 미끄러짐 방지
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


    //  점프 가능 여부 반환
    public bool CanJump()
    {
        if (Property.Stamina.Value < 10f)
            return false;
        else
            return CanMove && !_jumpConsumedThisFrame && JumpPressed && IsGrounded && !IsJumpAnimationPlaying();
    }

    //  점프 
    public void Jump()
    {
        if(Property.Stamina.Value < 10f)
            return;
        _jumpConsumedThisFrame = true;
        Property.StaminaConsume(Property.StaminaCostJump);
        Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    // 웅크렸는지 체크
    public void SetCrouch(bool crouch)
    {
        _isCrouching = crouch;
    }

    //  물에 들어갔는제 체크
    public void SetWater(bool water)
    {
        IsWater = water;
    }

    //  방향 설정
    public void SetRotation(float offset)
    {
        transform.rotation = Quaternion.Euler(0f, offset, 0f);
    }

    // 점프 애니메이션이 끝났는지 체크 
    public bool IsJumpAnimationPlaying()
    {
        return Controller._animator.GetCurrentAnimatorStateInfo(0).IsName("Jump");
    }

    //  소음 콜라이더
    public void SetStateColliderRadius(float radius)
    {
        if (StateSphereCollider != null)
            StateSphereCollider.radius = radius;
    }
    //  중력 ON/OFF
    public void SetGravity(bool enabled)
    {
        Rigidbody.useGravity = enabled;
    }
    //  움직임 On/Off
    public void MoveLock()
    {
        CanMove = !CanMove;
    }
}