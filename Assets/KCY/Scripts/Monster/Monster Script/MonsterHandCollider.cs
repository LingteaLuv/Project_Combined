using UnityEngine;

public class MonsterHandDetector : MonoBehaviour
{
    [SerializeField] private float detectRadius = 2f;
    [SerializeField] private LayerMask playerLayer; // 플레이어 레이어
    [SerializeField] private float detectInterval = 0.1f;

    private IAttackable attackLogic;
    private Monster_temp _monster;

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
            Debug.LogError("❌ MonsterHandDetector: Monster_temp를 찾을 수 없습니다.");
            return;
        }

        // IAttackable 인터페이스로 캐스팅
        attackLogic = _monster as IAttackable;
        if (attackLogic == null)
        {
            Debug.LogError("❌ MonsterHandDetector: IAttackable 구현체를 찾을 수 없습니다.");
            return;
        }

        // 주기적으로 플레이어 감지
        InvokeRepeating(nameof(DetectPlayer), 0f, detectInterval);
    }

    private void DetectPlayer()
    {
        if (_monster == null || _monster._isDead) return;

        // 반드시 공격 상태일 때만 탐지
        if (_monster._monsterMerchine.CurState != _monster._monsterMerchine.StateDic[Estate.Attack])
            return;

        // 범위 내 플레이어 탐색
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, playerLayer);
        Debug.Log($"[OverlapSphere] 감지된 수: {hits.Length}");

        foreach (var hit in hits)
        {
            // 🔍 먼저 GetComponent, 없으면 부모까지 검사
            IDamageable damageTarget = hit.GetComponent<IDamageable>();
            if (damageTarget == null)
            {
                damageTarget = hit.GetComponentInParent<IDamageable>();
            }

            if (damageTarget != null)
            {
                Debug.Log($"[HandSensor] 감지된 대상 타입: {damageTarget.GetType().Name}, 이름: {((MonoBehaviour)damageTarget).gameObject.name}");
                Debug.Log("[HandSensor] 플레이어 감지됨 → 공격 시도");
                attackLogic.Attack(damageTarget);
                break;
            }
            else
            {
                Debug.Log($"[OverlapSphere] IDamageable 없음: {hit.name}");
            }
        }
    }
}
