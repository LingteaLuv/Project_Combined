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
         Debug.Log("[MonsterHandDetector] Start 호출됨");
        // Layer가 설정되지 않았다면 "Player"로 자동 설정
        if (playerLayer == 0)
        {
            playerLayer = LayerMask.GetMask("Player");
        }

        // 부모 오브젝트에서 Monster_temp 찾기
        _monster = GetComponentInParent<Monster_temp>();
        if (_monster == null)
        {
            Debug.LogError("MonsterHandDetector: Monster_temp를 찾을 수 없습니다.");
            return;
        }

        // IAttackable 캐스팅
        attackLogic = _monster as IAttackable;
        if (attackLogic == null)
        {
            Debug.LogError("MonsterHandDetector: IAttackable 구현체를 찾을 수 없습니다.");
            return;
        }

        // 지속적으로 플레이어 감지
        InvokeRepeating(nameof(DetectPlayer), 0f, detectInterval);
    }

    public void DetectPlayer()
    {
        if (_monster == null || _monster._isDead) return;

        if (_monster._monsterMerchine == null || _monster._monsterMerchine.CurState == null)
        {
            Debug.LogWarning("상태머신 초기화 전 > 감지 중단");
            return;
        }


        if (_monster._monsterMerchine.CurState != _monster._monsterMerchine.StateDic[Estate.Attack])
            return;
    }
}
