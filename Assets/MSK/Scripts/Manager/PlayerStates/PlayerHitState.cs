using UnityEngine;

/// <summary>
/// 플레이어가 피격되었을 때 실행되는 상태입니다.
/// 데미지 적용, 무적 처리, 피격 애니메이션 재생, 사망 판정까지 모두 수행합니다.
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
        _damageApplied = false;
    }

    public override void Enter()
    {

        var health = _movement.Controller.PlayerHealthEdit;
        if (health.IsDead || health.IsInvincible)
        {
            _fsm.ChangeState(_movement.Controller.IdleState);
            return;
        }

        health.SetInvincible(true);
        _movement.Controller._animator.SetTrigger("IsHit");
        _movement.Controller.IsInHit = true;
    }
    public override void Exit()
    {
        _movement.Controller.IsInHit = false;
        _movement.Controller.PlayerHealthEdit.SetInvincible(false);
    }

    public override void FixedTick() { }

    public override void Tick()
    {
        var health = _movement.Controller.PlayerHealthEdit;

        if (!_damageApplied)
        {
            health.ApplyDamage(_damageAmount);
            _damageApplied = true;

            if (health.CurrentHp <= 0)
            {
                health.MarkDead();
                _fsm.ChangeState(_movement.Controller.DeadState);
                return;
            }
        }

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _fsm.ChangeState(_movement.MoveInput == Vector3.zero
                ? _movement.Controller.IdleState
                : _movement.Controller.MoveState);
        }
    }
}