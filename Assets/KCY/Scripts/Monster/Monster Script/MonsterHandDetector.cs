using UnityEngine;

public class MonsterHandDetector : MonoBehaviour
{
    [SerializeField] private float detectRadius = 2f;
    [SerializeField] private LayerMask playerLayer; // 플레이어 레이어
    [SerializeField] private float detectInterval = 0.1f;

    private IAttackable attackLogic;
    private Monster_temp _monster;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3f); 
    }
    private void Start()
    {
        // Layer가 설정되지 않았다면 "Player"로 자동 설정
        if (playerLayer == 0)
        {
            playerLayer = LayerMask.GetMask("Player");
        }

        // 부모 오브젝트에서 Monster_temp 찾기
        _monster = GetComponentInParent<Monster_temp>();
        if (_monster == null)
        {
            return;
        }

        // IAttackable 캐스팅
        attackLogic = _monster as IAttackable;
        if (attackLogic == null)
        {
            return;
        }

        // 지속적으로 플레이어 감지
        InvokeRepeating(nameof(DetectPlayer), 0f, detectInterval);
    }

    public void DetectPlayer()
    {
        if (_monster == null || _monster._isDead) return;

        // 상태 누락 방지
        if (_monster._monsterMerchine == null || _monster._monsterMerchine.CurState == null)
        {
            return;
        }

        // 중복 호출 방지
        if (_monster._monsterMerchine.CurState != _monster._monsterMerchine.StateDic[Estate.Attack])
            return;
    }
}
