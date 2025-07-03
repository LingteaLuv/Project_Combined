using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    //List의 장점 연결 - 삭제 O(1)
    //연결삭제를 자주 할 일이 있을까? 플레이어 무기를 습득하면 잘 안버리지 않을까?
    protected override bool ShouldDontDestroy => false;
    //Dictionary사용 - key,value
    [SerializeField] private List<WeaponBase> _availableWeapons; //플레이어 사용가능한 들고 있는 웨폰들 - 퀵슬롯 기준
    [SerializeField] private WeaponBase _currentWeapon;
    [SerializeField] private GameObject _currentWeaponPos;
    [SerializeField] private int _currentWeaponIndex = 0;

    [SerializeField] public GameObject spawn;
    //[SerializeField] private BareHands _bareHands; // 맨손

    // 무기 프리팹들을 등록해놓는 딕셔너리
    [SerializeField] private GameObject[] _weaponPrefabs; //프리팹에 다 넣기 일단

    protected override void Awake()
    {
        base.Awake();
    }

    //같은아이템이 2개있으면 안되는데?
    //한무기가 여러개 있을 수 있는 상황이 존재 할수도 있다.
    //인벤토리에서 무기를 관리하기 위한 무기id가 따로 존재할 필요가 있다.
    /*
    //현재 플레이어가 들고 있는 웨폰 반환
    public WeaponBase GetCurrentWeapon() => _currentWeapon;

    public void AddWeapon(WeaponBase weapon)
    {
        //등록할 무기가 null이거나 이미 해당 무기가 존재하는 경우 추가적인 리스트 에 Add방지
        if (weapon == null || _availableWeapons.Contains(weapon)) return;

        _availableWeapons.Add(weapon);      //사용가능한 무기에 등록하고
        Instantiate(weapon, _currentWeaponPos.transform);
        weapon.gameObject.SetActive(false); //해당 무기 비활성화
        Debug.Log($"{weapon.name} 무기를 획득했습니다!");
    }

    public void RemoveWeapon(WeaponBase weapon)
    {
        //삭제할 무기가 null이거나 사용가능한 무기중에 포함이 되어있지 않은 경우 빠른 return
        if (weapon == null || !_availableWeapons.Contains(weapon)) return;

        //삭제할 무기가 현재 들고있는 무기와 같은 무기라면
        if (_currentWeapon == weapon)
        {
            //현재 무기 null로 변경 후 현재 무기 비활성화처리
            _currentWeapon = null;
            weapon.gameObject.SetActive(false);
        }
        
        //해당 무기를 사용가능한 무기에서 삭제
        _availableWeapons.Remove(weapon);
        Debug.Log($"{weapon.name} 무기를 삭제했습니다!");
    }

    //퀵슬롯에서 사용해야할 때
    public void SetCurrentWeapon(WeaponBase weapon)
    {
        if (!_availableWeapons.Contains(weapon)) return;

        // 현재 무기 비활성화
        if (_currentWeapon != null)
        {
            _currentWeapon.gameObject.SetActive(false);
        }

        // 새 무기 활성화
        _currentWeapon = weapon;
        _currentWeapon.gameObject.SetActive(true);

        Debug.Log($"현재 무기: {_currentWeapon.name}");
    }

    /// <summary>
    /// 무기 공격함수 호출
    /// </summary>
    public void Attack()
    {
        if (_currentWeapon != null)
        {
            Debug.Log("무기로 공격");
            _currentWeapon.Attack();
        }
        else
        {
            Debug.Log("현재 들고 있는 무기가 존재하지 않습니다");
            //_bareHands.Attack(); // 무기 없을 때 맨손 공격
        }
    }

    public void SwitchToNextWeapon()
    {
        if (_availableWeapons.Count <= 1) return;

        _currentWeaponIndex = (_currentWeaponIndex + 1) % _availableWeapons.Count;
        SetCurrentWeapon(_availableWeapons[_currentWeaponIndex]);
    }*/
}
