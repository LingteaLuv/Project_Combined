using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponEquip : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private bool _isEquipmentChangeable = false;
    //장비 공격 Attack

    private void Update()
    {
        //테스트 코드
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (_playerAttack.CurrentWeapon == null) return;
            _animator.SetTrigger("Equip");
            _animator.SetBool("IsEquipmentChangeable", _isEquipmentChangeable);
        }
        //무기 해제
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_playerAttack.CurrentWeapon == null) return;
            _animator.SetTrigger("UnEquip");
            _animator.SetBool("IsEquipmentChangeable", _isEquipmentChangeable);
        }
    }

    //빈손 일 때랑에서무기제네릭 메서드, 무기에서 빈손, 무기에서 -> 무기 
    [SerializeField] private Transform _gunAlwaysPos;
    [SerializeField] private Transform _gunEquipPos;
    [SerializeField] private float _rotationSpeed = 5.0f;
    [SerializeField] private PlayerAttack _playerAttack;

    /*private void WeaponSetActive()
    {
        if (_playerAttack.CurrentWeapon == null) return;
        _playerAttack.CurrentWeapon.gameObject.SetActive(true);
    }

    private void WeaponDeactivate()
    {
        if (_playerAttack.CurrentWeapon == null) return;
        _playerAttack.CurrentWeapon.gameObject.SetActive(false);
    }*/


    //총이 한번에 확돌아가는 문제가 존재
    private void SetGunEquippPos()
    {
        if (_playerAttack.CurrentWeapon.ItemType != ItemType.Gun) return;

        _playerAttack.CurrentWeapon.transform.rotation = _gunEquipPos.rotation;
    }

    private void SetGunAlwaysPos()
    {
        if (_playerAttack.CurrentWeapon.ItemType != ItemType.Gun) return;

        _playerAttack.CurrentWeapon.transform.rotation = _gunAlwaysPos.rotation;
    }

    //유기
    /*private IEnumerator GunLerpRotation()
    {
        while (Quaternion.Angle(_currentWeapon.transform.rotation, _gunAlwaysPos.rotation) > 0.1f)
        {
            _currentWeapon.transform.rotation = Quaternion.Slerp(_currentWeapon.transform.rotation,
                _gunAlwaysPos.rotation, Time.deltaTime * _rotationSpeed);
        }
        yield return null;
        _currentWeapon.transform.rotation = _gunAlwaysPos.rotation;
    }*/

    //무기에서 무기로 호출함수
    //TODO - 애니메이션을 다꺼줘야한다.

    /*public void WeaponToWeapon()
    {
        Debug.Log("무기에서 무기");
        if(_playerAttack.CurrentWeapon.ItemType == ItemType.Gun || _playerAttack.CurrentWeapon.ItemType == ItemType.Melee || _playerAttack.CurrentWeapon.ItemType == ItemType.Throw)
        {
            _animator.SetTrigger("UnEquip");
        }
        //??
        //??
        if (_playerAttack.CurrentWeapon.ItemType == ItemType.Gun || _playerAttack.CurrentWeapon.ItemType == ItemType.Melee || _playerAttack.CurrentWeapon.ItemType == ItemType.Throw)
        {
            _animator.SetTrigger("Equip");
        }
    }

    //맨손에서 무기로 호출함수
    public void BarehandsToWeapon()
    {
        Debug.Log("맨손에서 무기");
        if (_playerAttack.CurrentWeapon.ItemType == ItemType.Gun || _playerAttack.CurrentWeapon.ItemType == ItemType.Melee)
            switch (_playerAttack.CurrentWeapon.ItemType)
            {
                case ItemType.Melee:
                    _animator.SetTrigger("Equip");      
                    break;
                case ItemType.Gun:
                    _animator.SetTrigger("Equip");      
                    break;
                default:
                    Debug.Log($"해당 무기는 장착 애니메이션이 없습니다. {_playerAttack.CurrentWeapon.ItemType}");
                    break;
            }
    }

    //무기에서 빈손
    public void WeaponToBarehands()
    {
        Debug.Log("무기에서 빈손");
        if (_playerAttack.CurrentWeapon.ItemType == ItemType.Gun || _playerAttack.CurrentWeapon.ItemType == ItemType.Melee)
            switch (_playerAttack.CurrentWeapon.ItemType)
            {
                case ItemType.Melee:
                    _animator.SetTrigger("UnEquip");
                    break;
                case ItemType.Gun:
                    _animator.SetTrigger("UnEquip");
                    break;
                default:
                    Debug.Log($"해당 무기는 장착 해제 애니메이션이 없습니다. {_playerAttack.CurrentWeapon.ItemType}");
                    break;
            }
    }*/
}
