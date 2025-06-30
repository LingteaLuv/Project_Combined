using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponEquip : MonoBehaviour
{
    #region 추후 이동 - 플레이어 장비 장착 해제 애니메이션를 관리하는 스크립트로

    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _left_Hand_target;
    [SerializeField] private GameObject _right_Hand_target;
    [SerializeField] private bool _isEquipmentChangeable = true; //아이템이 장착,해제가 가능한 시점인지

    private void Awake()
    {
        //모든 아이템은 해당 Hand_bone밑에 있다.
        _animator = gameObject.GetComponent<Animator>();
        _left_Hand_target = GameObject.Find("Hand_L");
        _right_Hand_target = GameObject.Find("Hand_R");
        UpdateWeapon();
    }
    private void UpdateWeapon()
    {
        _currentWeapon = _right_Hand_target.GetComponentInChildren<WeaponBase>();
    }

    private void Update()
    {
        //테스트 코드
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (_currentWeapon == null) return;
            _animator.SetTrigger("Equip");
            _animator.SetBool("IsEquipmentChangeable", _isEquipmentChangeable);
        }
        //무기 해제
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_currentWeapon == null) return;
            _animator.SetTrigger("UnEquip");
            _animator.SetBool("IsEquipmentChangeable", _isEquipmentChangeable);
        }
    }

    //빈손 일 때랑에서무기제네릭 메서드, 무기에서 빈손, 무기에서 -> 무기 
    [SerializeField] private Transform _gunAlwaysPos;
    [SerializeField] private Transform _gunEquipPos;
    [SerializeField] private float _rotationSpeed = 5.0f;
    [SerializeField] private WeaponBase _currentWeapon;

    private void WeaponSetActive()
    {
        if (_currentWeapon == null) return;
        _currentWeapon.gameObject.SetActive(true);
    }

    private void WeaponDeactivate()
    {
        if (_currentWeapon == null) return;
        _currentWeapon.gameObject.SetActive(false);
    }


    //총이 한번에 확돌아가는 문제가 존재
    private void SetGunEquippPos()
    {
        if (_currentWeapon.ItemType != ItemType.Gun) return;

        _currentWeapon.transform.rotation = _gunEquipPos.rotation;
        //StartCoroutine(GunLerpRotation());
    }

    private void SetGunAlwaysPos()
    {
        if (_currentWeapon.ItemType != ItemType.Gun) return;

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

    //무기에서 무기로 호출함수
    //TODO - 애니메이션을 다꺼줘야한다.
    public void WeaponToWeapon()
    {
        Debug.Log("무기에서 무기");
        if (_currentWeapon.ItemType == ItemType.Gun || _currentWeapon.ItemType == ItemType.Melee)
            switch (_currentWeapon.ItemType)
            {
                case ItemType.Melee:
                    _animator.SetTrigger("Equip");
                    break;
                case ItemType.Gun:
                    _animator.SetTrigger("Equip");
                    break;
                default:
                    Debug.Log($"해당 무기는 장착 애니메이션이 없습니다. {_currentWeapon.ItemType}");
                    break;
            }
    }

    //맨손에서 무기로 호출함수
    public void BarehandsToWeapon()
    {
        Debug.Log("맨손에서 무기");
        if (_currentWeapon.ItemType == ItemType.Gun || _currentWeapon.ItemType == ItemType.Melee)
            switch (_currentWeapon.ItemType)
            {
                case ItemType.Melee:
                    _animator.SetTrigger("Equip");      
                    break;
                case ItemType.Gun:
                    _animator.SetTrigger("Equip");      
                    break;
                default:
                    Debug.Log($"해당 무기는 장착 애니메이션이 없습니다. {_currentWeapon.ItemType}");
                    break;
            }
    }

    //무기에서 빈손
    public void WeaponToBarehands()
    {
        Debug.Log("무기에서 빈손");
        if (_currentWeapon.ItemType == ItemType.Gun || _currentWeapon.ItemType == ItemType.Melee)
            switch (_currentWeapon.ItemType)
            {
                case ItemType.Melee:
                    _animator.SetTrigger("UnEquip");
                    break;
                case ItemType.Gun:
                    _animator.SetTrigger("UnEquip");
                    break;
                default:
                    Debug.Log($"해당 무기는 장착 해제 애니메이션이 없습니다. {_currentWeapon.ItemType}");
                    break;
            }
    }
    #endregion

}
