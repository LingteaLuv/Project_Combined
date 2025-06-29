using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//최상위 부모
public abstract class WeaponBase : MonoBehaviour
{
    public Item _item;

    [SerializeField] protected ItemType _itemType;

    public ItemType ItemType { get { return _itemType; } protected set {_itemType = value; } }

    private float _weaponBaseid; 
    public bool IsAttack { get; private set; }

    public abstract void Attack();
}
