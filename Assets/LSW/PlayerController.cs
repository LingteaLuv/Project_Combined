using System;
using UnityEngine;

/// <summary>
/// 플레이어 상태머신과 애니메이션을 관리하는 컨트롤러입니다.
/// </summary>
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    #region Public 
    public Animator _animator;
    public bool IsCrouch { get; set; } = false;
    #endregion

    #region Player State
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    public PlayerIdleCrouchState IdleCrouchState { get; private set; }
    public PlayerClimbState ClimbState { get; private set; }
    #endregion

    #region Private
    private PlayerCameraController _cameraController;
    private PlayerMovement _movement;
    private PlayerStateMachine _fsm;
    private Vector3 _lastMoveInput = Vector3.zero;
    private bool _isJumping = false;
    #endregion

    #region Unity MonoBehaviour
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
            _movement.SetRotation(_cameraController.Offset);
        }
        UpdateMoveAnimation();
    }
    private void FixedUpdate()
    {
        _fsm.FixedUpdate();
    }
    #endregion

    # region Private Mathood
    private void Init()
    {
        _fsm = new PlayerStateMachine();
        _movement = GetComponent<PlayerMovement>();
        _cameraController = GetComponent<PlayerCameraController>();
        _animator = GetComponent<Animator>();
        _movement.Controller = this;

        IdleState = new PlayerIdleState(_fsm, _movement);
        MoveState = new PlayerMoveState(_fsm, _movement);
        JumpState = new PlayerJumpState(_fsm, _movement);
        CrouchState = new PlayerCrouchState(_fsm, _movement);
        IdleCrouchState = new PlayerIdleCrouchState(_fsm, _movement);
        ClimbState = new PlayerClimbState(_fsm, _movement);
    }

    /// <summary>
    /// 이동 애니메이션 상태를 갱신합니다.
    /// </summary>
    private void UpdateMoveAnimation()
    {
        Vector3 currentInput = _movement.MoveInput;
        bool isMoving = currentInput != Vector3.zero;

        if ((_lastMoveInput != Vector3.zero) != isMoving && !_movement.IsOnLadder)
        {
            _animator.SetBool("IsMove", isMoving);
            //_animator.speed = _animator.speed = _movement.GetAnimatorSpeedMultiplier();
        }
        UpdateGroundParameter();
        _lastMoveInput = currentInput;
    }
    private void UpdateGroundParameter()
    {
        bool isGrounded = _movement.IsGrounded;
        _animator.SetBool("IsGround", isGrounded);
    }
    #endregion

    #region Public Mathood
    /// <summary>
    /// 점프 애니메이션 상태를 외부 상태에서 직접 설정합니다.
    /// </summary>
    public void PlayJumpAnimation()
    {
        _animator.SetTrigger("IsJump");
    }
    public void PlayClimbAnimation()
    {
        _animator.SetBool("IsClimb",true);
    }
    
    public void StopClimbAnimation()
    {
        _animator.SetBool("IsClimb",false);
    }

    public void SetAnimatorSpeed()
    {
        _animator.speed = Mathf.Abs(Input.GetAxis("Vertical"));
    }
    #endregion
}
