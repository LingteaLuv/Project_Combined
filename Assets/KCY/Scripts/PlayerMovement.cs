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
    private Rigidbody _rb;
    private bool _jumpConsumedThisFrame;

    private Vector3 _currentRotation;
    private bool _isCrouching;

    [Header("Settings")]
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _groundCheckDistance = 0.05f;
    [SerializeField] private float _crouchSpeedMultiplier = 0.5f;

    public Vector3 MoveInput => _inputHandler.MoveInput;
    public bool JumpPressed => _inputHandler.JumpPressed;
    public bool CrouchHeld => _inputHandler.CrouchHeld;
    public bool IsOnLadder { get; private set; }
    public bool IsGrounded { get; private set; }


    private void Init()
    {
        _rb = GetComponent<Rigidbody>();
        PlayerClimbHandler = GetComponent<PlayerClimb>();
    }

    private void Awake() => Init();

    private void Update()
    {
        _jumpConsumedThisFrame = false;
        IsGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, _groundCheckDistance + 0.1f);
        IsOnLadder = _inputHandler.IsOnLadder;
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
            Vector3 velocity = _rb.velocity;
            targetVel.y = velocity.y; // 유지
            _rb.velocity = Vector3.MoveTowards(_rb.velocity, targetVel, 100* Time.deltaTime);
        
            bool downRay = Physics.Raycast(transform.position + Vector3.up * 0.01f, transform.forward, 0.5f);
            bool middleRay = Physics.Raycast(transform.position + Vector3.up * 0.1f, transform.forward, 0.5f);
            bool upRay = Physics.Raycast(transform.position + Vector3.up * 0.3f, transform.forward, 0.5f);
            
            if (downRay && middleRay)
            {
                if (!upRay)
                {
                    _rb.position += Vector3.up * 0.1f;
                } 
            }
            
            //transform.position += moveDir * speed * Time.deltaTime;
        }
        else
        {
            Vector3 targetVel = new Vector3(0, _rb.velocity.y, 0);
            _rb.velocity = Vector3.MoveTowards(_rb.velocity, targetVel, 100* Time.deltaTime);
        }
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
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
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
        _rb.useGravity = enabled;
    }
}