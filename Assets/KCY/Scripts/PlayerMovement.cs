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

    public PlayerClimb PlayerClimbHandler { get; private set; }

    public PlayerController Controller { get; set; }
    public Rigidbody Rigidbody { get; private set; }
    private bool _jumpConsumedThisFrame;
    private float _airTime;
    private bool _wasGrounded;

    private Vector3 _currentRotation;
    private bool _isCrouching;

    [Header("Settings")]
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _groundCheckDistance = 0.05f;
    [SerializeField] private float _crouchSpeedMultiplier = 0.5f;
    [SerializeField] private float fallMultiplier = 5f;

    public Vector3 MoveInput => _inputHandler.MoveInput;
    public bool JumpPressed => _inputHandler.JumpPressed;
    public bool CrouchHeld => _inputHandler.CrouchHeld;
    public bool InteractPressed => _inputHandler.InteractPressed;

    public bool IsOnLadder { get; private set; }
    public bool IsGrounded { get; private set; }


    private void Init()
    {
        Rigidbody = GetComponent<Rigidbody>();
        PlayerClimbHandler = GetComponent<PlayerClimb>();
    }

    private void Awake() => Init();

    private void Update()
    {
        _jumpConsumedThisFrame = false;
        _wasGrounded = IsGrounded;
        IsGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, _groundCheckDistance + 0.1f);
        IsOnLadder = _inputHandler.IsOnLadder;
        
        // 채공 시간 누적
        if (!IsGrounded)
        {
            TrackAirTime(); 
        }
        //  착지 여부 판단
        if (! _wasGrounded && IsGrounded)
        {
            ApplyFallDamage();
            _airTime = 0;   // 채공 시간 초기화
        }
    }

    private void ApplyFallDamage()
    {
        //  TODO : 데미지 입히는 로직
    }
    private void TrackAirTime()
    {
        _airTime += Time.deltaTime;
    }

    /// <summary>
    /// 입력 방향에 따라 관성 없이 이동합니다.
    /// </summary>
    public void Move(Vector3 inputDir)
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Vector3 moveDir = cam.transform.forward * inputDir.z + cam.transform.right * inputDir.x;
        moveDir.y = 0f;
        moveDir.Normalize();
        
        if (inputDir.magnitude > 0.1f)
        {
            float speed = _property.MoveSpeed.Value * (_isCrouching ? _crouchSpeedMultiplier : 1f);
            Vector3 targetVel = moveDir * speed;
            Vector3 velocity = Rigidbody.velocity;
            targetVel.y = velocity.y; // 유지
            Rigidbody.velocity = Vector3.MoveTowards(Rigidbody.velocity, targetVel, 100* Time.deltaTime);
        
            bool downRay = Physics.Raycast(transform.position + Vector3.up * 0.01f, transform.forward, 0.5f);
            bool middleRay = Physics.Raycast(transform.position + Vector3.up * 0.1f, transform.forward, 0.5f);
            bool upRay = Physics.Raycast(transform.position + Vector3.up * 0.3f, transform.forward, 0.5f);
            
            if (downRay && middleRay)
            {
                if (!upRay)
                {
                    Rigidbody.position += Vector3.up * 0.1f;
                } 
            }
            
            //transform.position += moveDir * speed * Time.deltaTime;
        }
        else
        {
            Vector3 targetVel = new Vector3(0, Rigidbody.velocity.y, 0);
            Rigidbody.velocity = Vector3.MoveTowards(Rigidbody.velocity, targetVel, 100* Time.deltaTime);
        }
        Quaternion targetRot = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);

        if (Rigidbody.velocity.y < 0)
        {
            Rigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
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
        Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
    public void SetCrouch(bool crouch)
    {
        _isCrouching = crouch;
    }

    public void SetRotation(float offset)
    {
        transform.rotation = Quaternion.Euler(0f, offset, 0f);

        /*if (_aim != null)
        {
            Vector3 aimEuler = _aim.localEulerAngles;
            _aim.localEulerAngles = new Vector3(offset, aimEuler.y, aimEuler.z);
        }*/
    }
    //TODO : 애니메이션 스피드 조정용 코드 추가 예정
    public float GetAnimatorSpeedMultiplier()
    {
        return Mathf.Clamp01(MoveInput.magnitude) * (_property?.MoveSpeed?.Value ?? 0f)+1;
    }
    
    public void SetGravity(bool enabled)
    {
        Rigidbody.useGravity = enabled;
    }
}