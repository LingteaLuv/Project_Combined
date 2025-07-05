using UnityEngine;

/// <summary>
/// 플레이어가 사망했을 때의 상태입니다.
/// 이동, 입력을 차단하고 죽음 애니메이션을 재생한 뒤 게임오버 처리를 합니다.
/// </summary>
public class PlayerDeadState : PlayerState
{
    private float _deathDelay = 2.5f; // 죽음 연출 시간
    private float _timer;

    public PlayerDeadState(PlayerStateMachine fsm, PlayerMovement movement)
        : base(fsm, movement) { }

    public override void Enter()
    {
        _timer = _deathDelay;
        var anim = _movement.Controller._animator;
        anim.SetBool("IsDead", true);

        // 모든 추가 레이어 비활성화
        for (int i = 1; i < anim.layerCount; i++)
            anim.SetLayerWeight(i, 0f);
        anim.SetLayerWeight(0, 1f); // Base Layer만 활성화

        _movement.SetGravity(true);
        _movement.Rigidbody.velocity = Vector3.zero;

        // TODO: 필요한 경우, InputHandler 비활성화 또는 UI 연출
    }

    public override void Exit()
    {
        // DeadState는 보통 종료되지 않음
    }

    public override void FixedTick()
    {
        // 죽은 상태에서는 물리 처리 없음
    }

    public override void Tick()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            // 사망 후 게임 오버 처리
            GameManager.Instance.GameOver(); // 씬 전환, UI 처리 등
        }
    }
}
