using UnityEngine;


public class PlayerDamageReceiver : MonoBehaviour, IDamageable
{
    public int maxHp = 10;
    private float currentHp;

    private void Start()
    {
        currentHp = maxHp;
        Debug.Log($"플레이어 HP: {currentHp}");
    }

    public void Damaged(float damage)
    {
        currentHp -= damage;

       
        currentHp = Mathf.Max(0, currentHp);

        Debug.Log($"받은 데미지: {damage}, 남은 HP: {currentHp}");

        if (currentHp <= 0)
        {
            Debug.Log("끝");
            
        }
    }
}

