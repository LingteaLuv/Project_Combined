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
    #endregion

    #region State Flags
    public bool IsCrouch { get; set; } = false;
    public bool IsInHit { get; set; } = false;
    #endregion

    #region Player State
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    public PlayerIdleCrouchState IdleCrouchState { get; private set; }
    public PlayerClimbState ClimbState { get; private set; }
    public PlayerInteractState InteractState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerHitState HitState { get; private set; }
    #endregion

    #region Private
    private PlayerCameraController _cameraController;
    private PlayerMovement _movement;
    private PlayerStateMachine _fsm;
    #endregion

    #region Unity MonoBehaviour
    private void Awake() => Init();

    private void Start()
    {
        _movement.Property.OnHited += OnPlayyerHided;
        _movement.Property.OnDied += OnPlayerDied;
        _fsm.ChangeState(IdleState);
    }

    private void Update()
    {
        _fsm.Update();
        if (!_movement.CanMove) {
            _animator.SetBool("IsRunning", false);
            _animator.SetBool("IsMove", false);
            _fsm.ChangeState(IdleState);
            return;
        }
        if (_cameraController._cinemachineBrain.enabled)
        {
            _movement.SetRotation(_cameraController.OffsetX);
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
        _movement = GetComponent<PlayerMovement>();
        _cameraController = GetComponent<PlayerCameraController>();
        _animator = GetComponent<Animator>();

        _fsm = new PlayerStateMachine();
        FallState = new PlayerFallState(_fsm, _movement);
        InteractState = new PlayerInteractState(_fsm, _movement);
        IdleState = new PlayerIdleState(_fsm, _movement);
        MoveState = new PlayerMoveState(_fsm, _movement);
        JumpState = new PlayerJumpState(_fsm, _movement);
        CrouchState = new PlayerCrouchState(_fsm, _movement);
        IdleCrouchState = new PlayerIdleCrouchState(_fsm, _movement);
        ClimbState = new PlayerClimbState(_fsm, _movement);
        DeadState = new PlayerDeadState(_fsm, _movement);
        HitState = new PlayerHitState(_fsm, _movement);
    }

    /// <summary>
    /// 이동 애니메이션 상태를 갱신합니다.
    /// </summary>
    private void UpdateMoveAnimation()
    {
        bool isMoving = _movement.MoveInput != Vector3.zero;
        if (!_movement.IsOnLadder)
        {
            _animator.SetBool("IsMove", isMoving);
        }
        UpdateGroundParameter();
    }
    private void UpdateGroundParameter()
    {
        bool isGrounded = _movement.IsGrounded;
        _animator.SetBool("IsGround", isGrounded);
    }
    private void OnPlayyerHided()
    {
        _fsm.ChangeState(HitState);
    }

    private void OnPlayerDied()
    {
        Debug.Log("사망");
        _fsm.ChangeState(DeadState);
    }
    #endregion

    #region Public Mathood
    /// <summary>
    /// 점프 애니메이션 상태를 외부 상태에서 직접 설정합니다.
    /// </summary>
    public void PlayJumpAnimation()
    {
        AudioManager.Instance.PlaySFX("Whoosh 08", transform.position);
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
        _animator.SetFloat("ClimbSpeed", Mathf.Abs(Input.GetAxis("Vertical")));
    }
    public void PlayInteractAnimation()
    {
        _animator.SetBool("IsInteracting", true);
    }
    public void StopInteractAnimation()
    {
        _animator.SetBool("IsInteracting", false);
    }
    public void PlayRunning()
    {   
        _animator.SetBool("IsRunning", true);
    }
    public void StopRunning()
    {
        _animator.SetBool("IsRunning", false);
    }
    public void PlayHited()
    {
        AudioManager.Instance.PlaySFX("Hit 14", this.transform.position);
        _animator.SetTrigger("IsHit");
    }
    public void PlayMoveSound()
    {
        AudioManager.Instance.PlaySFX("Light Armor Running 2_01", this.transform.position);
    }
    public void PlayRunSound()
    {
        AudioManager.Instance.PlaySFX("Light Armor Running 2_01", this.transform.position);
    }
    #endregion
}
