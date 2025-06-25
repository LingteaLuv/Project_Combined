using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //[SerializeField] private WeaponManager _weaponManager;
    [SerializeField] private WeaponBase _currentWeapon;
    [SerializeField] Animator _animator; //플레이어의 애니메이터가 필요 -> 공격시 플레이어 애니메이션 재생하기 위하여
    //[SerializeField][Range(0, 5)] private float _mouseSensitivity = 1;
    //추후에 플레이어가 어떤 키를 입력했는지 불러올것같아서
    //[SerializeField] private PlayerInputManager _playerInput;

    void Update()
    {
        //공격키 누를시 공격
        if (Input.GetKeyDown(KeyCode.Q))
        {
            switch(_currentWeapon.ItemType)
            {
                case ItemType.Melee:
                    //_animator.Set    <- 해당 특정 애니메이션 재생 할 수 있도록
                    PlayerAttackStart();
                    break;
                case ItemType.Gun :
                    //_animator.Set   <- 해당 특정 애니메이션 재생 할 수 있도록
                    PlayerAttackStart();
                    break;
                default:
                    Debug.Log($"들고 있는 무기 에러.\n 현재 무기 : {_currentWeapon} , ItemType {_currentWeapon.ItemType}");
                    break;
            }
        }
        //무기 채인지
        if (Input.GetKeyDown(KeyCode.W))
        {
            //_weaponManager.SwitchWeapon();
        }
        //무기 장착
        if (Input.GetKeyDown(KeyCode.E))
        {
            _animator.SetTrigger("Equip");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _animator.SetTrigger("UnEquip");
        }
    }

    public void PlayerAttackStart()
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.Attack();
            Debug.Log($"공격한 무기 : {_currentWeapon}");
        }
    }


    /*public Vector3 SetAimRotation()
    {
        Vector2 mouseDir = GetMouseDirection();
    }*/

    /*private Vector2 GetMouseDirection()
    {
        float mouseX = Input.GetAxis("Mouse X") + _mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") + _mouseSensitivity;

        return new Vector2(mouseX, mouseY);
    }*/
}
