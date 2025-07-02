using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiedWeapon : WeaponBase, IDefendable
{
    //읽어오는 SO데이터 값
    [SerializeField]protected ShieldItem _shiedData;
    private Transform _playerPos;
    [SerializeField]public int _defenseAmount => _shiedData.DefenseAmount;

    //내가 사용하는 변수
    //[Header("방패 셋팅값")]
    //[SerializeField] private float _blockAngle = 180f;  //막아주는 각도 - 뒤에서 180도막기

    private void Reset()
    {
        _itemType = ItemType.Shield;
    }
    public void Awake()
    {
        Init();
    }

    public override void Init()
    {
        gameObject.transform.localPosition = _weaponSpawnPos.transform.localPosition;
        gameObject.transform.localRotation = _weaponSpawnPos.transform.localRotation;
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
