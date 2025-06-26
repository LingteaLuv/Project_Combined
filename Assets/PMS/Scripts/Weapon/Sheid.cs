using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheid : WeaponBase
{
    [Header("방패 셋팅값")]
    [SerializeField] private int _defenseValue = 50;            //막아주는 피해량
    [SerializeField] private float _blockAngle = 180f;           //막아주는 각도 - 뒤에서 180도막기
    [SerializeField] private bool _canCounterAttack = false;
    public override bool IsAttack => false;

    private void Reset()
    {
        _itemType = ItemType.Shield;
    }

    public override void Attack()
    {
        throw new System.NotImplementedException("Shield cannot attack");
    }
}
