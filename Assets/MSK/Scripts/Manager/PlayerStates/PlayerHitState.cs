using UnityEngine;

/// <summary>
/// 플레이어가 피격되었을 때 실행되는 상태입니다.
/// 데미지 적용, 피격 애니메이션 재생, 사망 판정까지 수행합니다.
/// </summary>
public class PlayerHitState : PlayerState
{
    private float _hitDuration = 0.3f;
    private float _timer;
    private int _damageAmount;
    private bool _damageApplied;

    public PlayerHitState(PlayerStateMachine fsm, PlayerMovement movement)
        : base(fsm, movement) { }

    /// <summary>
    /// 피격 시 받을 데미지를 설정합니다.
    /// </summary>
    public void SetDamage(int amount)
    {
        _damageAmount = amount;
    }

    public override void Enter()
    {
        var controller = _movement?.Controller;
        var health = controller?.PlayerHealth;
        var animator = controller?._animator;

        _timer = _hitDuration;
        controller.IsInHit = true;
        _damageApplied = false;
    }

    public override void Exit()
    {
        var controller = _movement?.Controller;
        if (controller != null)
            controller.IsInHit = false;
    }

    public override void FixedTick() { }

    public override void Tick()
    {
        var controller = _movement?.Controller;
        var health = controller?.PlayerHealth;

        if (controller == null || health == null)
            return;

        if (!_damageApplied)
        {
            health.Damaged(_damageAmount);
        }

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _fsm.ChangeState(_movement.MoveInput == Vector3.zero
                ? controller.IdleState
                : controller.MoveState);
        }
    }
}