using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth2 : MonoBehaviour
{
    public int currentHp = 10;

    public void Damaged(int damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        Debug.Log($"[InstantPlayer] 피격! 현재 체력: {currentHp}");
    }
}
