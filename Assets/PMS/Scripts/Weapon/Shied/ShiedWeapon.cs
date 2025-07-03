using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiedWeapon : WeaponBase, IDefendable
{
    //읽어오는 SO데이터 값
    [SerializeField]protected ShieldItem _shiedData;
    private void Reset()
    {
        ItemType = ItemType.Shield;
    }
    public void Awake()
    {
        base.Init();
    }

    public override void Attack()
    {
        throw new System.NotImplementedException("Shield can not attack");
    }

    // IDefendable 인터페이스 구현
    public int GetDefenseAmount()
    {
        return _shiedData.DefenseAmount;
    }

    protected override void ExecuteAttack()
    {
        throw new System.NotImplementedException("Shield can not attack");
    }
}
