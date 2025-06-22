using EPOOutline.Demo;
using UnityEngine;

/// <summary>
/// 플레이어 상태머신과 애니메이션을 관리하는 컨트롤러입니다.
/// </summary>
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] public Animator _animator;
    [SerializeField] private PlayerCameraController _cameraController;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    private PlayerMovement _movement;
    private PlayerStateMachine _fsm;

    private Vector3 _lastMoveInput = Vector3.zero;
    private bool _isJumping = false;


    private void Init() 
    {
        _fsm = new PlayerStateMachine();
        _movement = GetComponent<PlayerMovement>();
        _cameraController = GetComponent<PlayerCameraController>();
        _movement.Controller = this;

        IdleState = new PlayerIdleState(_fsm, _movement);
        MoveState = new PlayerMoveState(_fsm, _movement);
        JumpState = new PlayerJumpState(_fsm, _movement);
        CrouchState = new PlayerCrouchState(_fsm, _movement);
    }
    private void Awake() => Init();

    private void Start()
    {
        _fsm.ChangeState(IdleState);
    }

    private void Update()
    {
        _fsm.Update();
        if (_cameraController != null)
        {
            _movement.SetRotation(_cameraController.CurrentRotation);
        }
        UpdateMoveAnimation();
    }
    private void FixedUpdate()
    {
        _fsm.FixedUpdate();
        
    }
    /// <summary>
    /// 이동 애니메이션 상태를 갱신합니다.
    /// </summary>
    private void UpdateMoveAnimation()
    {
        Vector3 currentInput = _movement.MoveInput;
        bool isMoving = currentInput != Vector3.zero;

        if ((_lastMoveInput != Vector3.zero) != isMoving)
        {
            _animator.SetBool("IsMove", isMoving);
            _animator.speed = _animator.speed = _movement.GetAnimatorSpeedMultiplier();
        }
        UpdateGroundParameter();
        _lastMoveInput = currentInput;
    }

    /// <summary>
    /// 점프 애니메이션 상태를 외부 상태에서 직접 설정합니다.
    /// </summary>
    public void PlayJumpAnimation()
    {
        _animator.SetTrigger("IsJump");
    }
    private void UpdateGroundParameter()
    {
        bool isGrounded = _movement.IsGrounded;
        _animator.SetBool("IsGround", isGrounded);
    }
}
