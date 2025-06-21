using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private WeaponManager _weaponManager;
    [SerializeField][Range(0, 5)] private float _mouseSensitivity = 1;
    //추후에 플레이어가 어떤 키를 입력했는지 불러올것같아서
    //[SerializeField] private PlayerInputManager _playerInput;

    void Update()
    {
        //공격키 누를시 공격
        if (Input.GetKeyDown(KeyCode.X))
        {
            _weaponManager.Attack();
        }
        //무기 채인지
        if (Input.GetKeyDown(KeyCode.D))
        {
            //_weaponManager.SwitchWeapon();
        }
    }

    /*public Vector3 SetAimRotation()
    {
        Vector2 mouseDir = GetMouseDirection();
    }*/

    private Vector2 GetMouseDirection()
    {
        float mouseX = Input.GetAxis("Mouse X") + _mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") + _mouseSensitivity;

        return new Vector2(mouseX, mouseY);
    }
}
