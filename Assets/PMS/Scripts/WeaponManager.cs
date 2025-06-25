using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager> 
{
    //List의 장점 연결 - 삭제 O(1)
    //연결삭제를 자주 할 일이 있을까? 플레이어 무기를 습득하면 잘 안버리지 않을까?
    protected override bool ShouldDontDestroy => false;
    //Dictionary사용 - key,value
    [SerializeField] private List<WeaponBase> _availableWeapons; //플레이어 사용가능한 들고 있는 웨폰들
    [SerializeField] private WeaponBase _currentWeapon;
    //[SerializeField] private BareHands _bareHands; // 맨손

    protected override void Awake()
    {
        base.Awake();
    }

    //만약 습득을 했다면 사용가능한 웨폰이어여 할 것이다.

    /// <summary>
    /// 무기아이템 플레이어가 들고 있는 리스트(사용가능한 무기 리스트)에 추가하는 함수 
    /// </summary>
    /// <param name="weapon"> 무기 아이템</param>
    public void AddWeapon(WeaponBase weapon)
    {
        if (weapon == null) return;

        if (!_availableWeapons.Contains(weapon))
        {
            _availableWeapons.Add(weapon);
            Debug.Log($"{weapon.name} 무기를 획득했습니다!");
        }
    }

    public void RemoveWeapon(WeaponBase weapon)
    {
        if (weapon == null) return;

        if (!_availableWeapons.Contains(weapon))
        {
            _availableWeapons.Remove(weapon);
            Debug.Log($"{weapon.name} 무기를 삭제했습니다!");
        }
    }

    //같은아이템이 2개있으면 안되는데?
    /// <summary>
    /// 특정 무기를 
    /// </summary>
    /// <param name="weapon"></param>
    public void SetCurrentWeapon(WeaponBase weapon)
    {
        _currentWeapon = weapon; //나중에 플레이어의 인벤토리에서 꺼내오도록 해야함
    }

    /// <summary>
    /// 무기 공격함수 호출
    /// </summary>
    public void Attack()
    {
        if (_currentWeapon != null)
            Debug.Log("무기로 공격");
        //_currentWeapon.Attack();
        else
            Debug.Log("현재 들고 있는 무기가 존재하지 않습니다");
            //_bareHands.Attack(); // 무기 없을 때 맨손 공격
    }

    /*public void SwitchWeapon(WeaponBase weapon);
    {
        _currentWeapon = 
    }*/
}
