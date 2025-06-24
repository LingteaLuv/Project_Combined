using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : GunWeaponBase //이후 총마다 상속을 시켜 줘야 하지 않을까
{
    private void Awake()
    {
        Init();     //나중에 플레이어 해당 사용할려고 할 때
        SetInit();
    }
    public override void Init()
    {
       base.Init();
    }


    private void Update()
    {
        //나중에 플레이어 Input으로 Shot()
        if(Input.GetKeyDown(KeyCode.X) && _canShot && _currentAmmoCount > 0) 
        {
            GameObject bulletObj = _gunBulletObjectPool.GetInactive();
            UpdateTrajectory(bulletObj, bulletObj.GetComponent<BulletBase>()._speed);
            //Attack();
        }
        if (Input.GetKeyDown(KeyCode.R) && !_isReload)
        {
            StartCoroutine(ReloadCorutine());
            Debug.Log("총 재장전중");
        }
    }

    //총알 딜레이 설정
    private IEnumerator ShotDelay()
    {
        _canShot = false;
        yield return new WaitForSeconds(_fireDelay);
        _canShot = true;
    }
    private IEnumerator ReloadCorutine()
    {
        //장전 애니메이션 재생 추가

        _isReload = true;

        _currentAmmoCount = _maxAmmoCount;

        yield return new WaitForSeconds(_reloadTime);


        _isReload = false;
    }

    //외부에서 사용할 총을 쏘는 함수
    public override void Attack()
    {
        if (_currentAmmoCount == 0)
        {
            Debug.Log("R키를 눌러 장전하세요");
            return;
        }

        //발사 할 수 있는 총알이 있는지 총알풀 검사
        GameObject bulletObj = _gunBulletObjectPool.GetInactive();

        if (bulletObj != null && _currentAmmoCount > 0)  //만약 들고 왔다면 
        {
            // 발사 딜레이 시작 (총알이 실제로 발사될 때만)
            StartCoroutine(ShotDelay());
            //총알 위치 설정
            bulletObj.transform.position = _firePoint.transform.position;

            //총알 방향 설정
            BulletBase bullet = bulletObj.GetComponent<BulletBase>();
            bullet.SetDamage(Damage);
            if (bullet != null)
            {
                bullet.SetDirection(_firePoint.forward);
            }
            bulletObj.SetActive(true); //해당 총알을 활성화시킴

            _currentAmmoCount--; //총알 한발씩 제거
        }
        else
        {
            Debug.Log("총알이 준비되어 있지 않습니다");
        }
    }
}
