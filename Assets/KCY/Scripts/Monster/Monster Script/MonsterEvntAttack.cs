using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEvntAttack : MonoBehaviour
{
    private Monster_temp monster;

    private void Awake()
    {
        monster = GetComponentInParent<Monster_temp>();
    }

    
    public void AttackEvent()
    {
        if (monster != null)
        {
            Debug.Log("제발 나와주세요,제발 나와주세요,제발 나와주세요,제발 나와주세요,제발 나와주세요");
            monster.AttackEvent(); 
        }
    }
}
