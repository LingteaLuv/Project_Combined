using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//최상위 부모 - 공통된 속성은 여기서 처리
public abstract class WeaponBase : MonoBehaviour
{
    //아이템 클래스를 가져온다 -> 나중에 인벤토리에 있는 데이터를 읽고 쓰기 위해서
    public Item _item;      

    [SerializeField] protected ItemType _itemType;
    [SerializeField] protected GameObject _weaponSpawnPos;    //플레이어 손위치에 소환도되야하는 위치 local위치를 변경해야 할 것 같음

    public ItemType ItemType { get { return _itemType; } protected set {_itemType = value; } }
    public abstract void Attack(); //자식에서 구현 - 무기 공격
    public virtual void Defense() { }
    public virtual void Init() { }
}
