using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMS_Enemy : MonoBehaviour, IDamageable
{
    public float hp;
    private void Start()
    {
        hp = 50;
    }

    public void Damaged(float aktDamage)
    {
        hp -= aktDamage;
        Debug.Log($"[{gameObject.name}] Current HP: {hp}");
    }
}
