using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : Singleton<PlayerWeaponManager>
{
    protected override bool ShouldDontDestroy => false;

    private WeaponBase _rightCurrentWeapon;
    private WeaponBase _leftCurrentWeapon;
    

    //소환되는 Transform 계층
    [SerializeField] public GameObject _left_Hand_target;
    [SerializeField] public GameObject _right_Hand_target;

    // 이벤트 추가
    public static event System.Action<WeaponBase> OnRightWeaponChanged;
    public static event System.Action<WeaponBase> OnLeftWeaponChanged;


    //외부에서 참조할 현재 플레이어가 들고 있어야할 웨폰을 받아서
    //다시 한번 내 구독자들 한테 뿌림
    public WeaponBase LeftCurrentWeapon
    {
        get => _leftCurrentWeapon;
        set
        {
            if (_leftCurrentWeapon != value)
            {
                _leftCurrentWeapon = value;
                OnLeftWeaponChanged?.Invoke(_leftCurrentWeapon); // 이벤트 발생
            }
        }
    }

    public WeaponBase RightCurrentWeapon
    {
        get => _rightCurrentWeapon;
        set
        {
            if (_rightCurrentWeapon != value)
            {
                _rightCurrentWeapon = value;
                OnRightWeaponChanged?.Invoke(_rightCurrentWeapon); // 이벤트 발생
            }
        }
    }
    private void Update()
    {
        //테스트코드
        UpdateCurrentWeapon();
    }
    /// <summary>
    /// 플레이어 양손이 바뀐것을 업데이트 / 기본
    /// </summary>
    public void UpdateCurrentWeapon()
    {
        _leftCurrentWeapon = _left_Hand_target.GetComponentInChildren<WeaponBase>();
        _rightCurrentWeapon = _right_Hand_target.GetComponentInChildren<WeaponBase>();
    }

    /// <summary>
    /// 오른쪽 손이 바뀐 것을 업데이트 / 선택
    /// </summary>
    public void UpdateRightCurrentWeapon()
    {
        _rightCurrentWeapon = _right_Hand_target.GetComponentInChildren<WeaponBase>();
    }

    /// <summary>
    /// 왼쪽 손이 바뀐 것을 업데이트 / 선택
    /// </summary>
    public void UpdateLeftCurrentWeapon()
    {
        _leftCurrentWeapon = _left_Hand_target.GetComponentInChildren<WeaponBase>();
    }
}
