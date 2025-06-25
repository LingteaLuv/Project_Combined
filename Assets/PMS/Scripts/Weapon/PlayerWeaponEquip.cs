using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponEquip : MonoBehaviour
{
    [SerializeField] private GameObject _currentWeapon;
    [SerializeField] private Transform _gunAlwaysPos;
    [SerializeField] private Transform _gunEquipPos;
    [SerializeField] private float _rotationSpeed = 5.0f;
    //[SerializeField] private WeaponBase _currentWeapon;

    private void WeaponSetActive()
    {
        if (_currentWeapon == null) return;
        _currentWeapon.SetActive(true);
    }

    private void WeaponDeactivate()
    {
        if (_currentWeapon == null) return;
        _currentWeapon.SetActive(false);
    }


    //총이 한번에 확돌아가는 문제가 존재
    private void SetGunEquippPos()
    {
        if (_currentWeapon.GetComponent<WeaponBase>().ItemType != ItemType.Gun) return;

        _currentWeapon.transform.rotation = _gunEquipPos.rotation;
        //StartCoroutine(GunLerpRotation());
    }

    private void SetGunAlwaysPos()
    {
        if (_currentWeapon.GetComponent<WeaponBase>().ItemType != ItemType.Gun) return;

        _currentWeapon.transform.rotation = _gunAlwaysPos.rotation;
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


}
