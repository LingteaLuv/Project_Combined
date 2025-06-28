using UnityEngine;


public class PlayerDamageReceiver : MonoBehaviour, IDamageable
{
    public int maxHp = 10;
    private int currentHp;

    private void Start()
    {
        currentHp = maxHp;
        Debug.Log($"플레이어 HP: {currentHp}");
    }

    public void Damaged(int damage)
    {
        currentHp -= damage;

        // HP가 0 아래로 떨어지지 않도록
        currentHp = Mathf.Max(0, currentHp);

        Debug.Log($"받은 데미지: {damage}, 남은 HP: {currentHp}");

        if (currentHp <= 0)
        {
            Debug.Log("끝");
            // 필요한 사망 처리 로직 추가 가능
        }
    }
}

