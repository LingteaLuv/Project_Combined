using UnityEngine;

/// <summary>
/// 특정 위치에서 플레이어가 VaultState로 진입하도록 유도하는 트리거입니다.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class ParkourTrigger : MonoBehaviour
{
    [Tooltip("Vault 애니메이션의 지속 시간 (초)")]
    [SerializeField] private float _vaultDuration = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        Animator anim = player.Animator;

        // 현재 Vault 중이 아니라면 상태 전환
        if (!anim.GetBool("isVault"))
        {
            player.StateMachine.ChangeState(new VaultState(player, _vaultDuration));
        }
    }

    private void Reset()
    {
        // 트리거 콜라이더 세팅 보장
        BoxCollider box = GetComponent<BoxCollider>();
        box.isTrigger = true;
    }
}