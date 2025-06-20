using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : GunWeaponBase //이후 총마다 상속을 시켜 줘야 하지 않을까
{
    private void Awake()
    {
        Init();     //나중에 플레이어 해당 사용할려고 할 때
    }
    public override void Init()
    {
       base.Init();
    }

    private void Update()
    {
        //나중에 플레이어 Input으로 Shot()
        if (Input.GetKeyDown(KeyCode.X) && _canShot && _currentAmmoCount > 0)
        {
            Shot();
        }
    }

    //총알 딜레이 설정
    private IEnumerator ShotDelay()
    {
        _canShot = false;
        yield return new WaitForSeconds(_fireDelay);
        _canShot = true;
    }

    //외부에서 사용할 총을 쏘는 함수
    public void Shot()
    {
        if (_currentAmmoCount == 0)
        {
            Debug.Log("R키를 눌러 장전하세요");
        }

        //발사 할 수 있는 총알이 있는지 총알풀 검사
        GameObject bulletObj = _gunBulletObjectPool.GetInactive();

        if (bulletObj != null)  //만약 들고 왔다면 
        {
            // 발사 딜레이 시작 (총알이 실제로 발사될 때만)
            StartCoroutine(ShotDelay());

            //총알 위치 설정
            bulletObj.transform.position = _firePoint.transform.position;

            //총알 방향 설정
            BulletBase bullet = bulletObj.GetComponent<BulletBase>();
            if (bullet != null)
            {
                bullet.SetDirection(_firePoint.forward);
            }
            bulletObj.SetActive(true); //해당 총알을 활성화시킴
        }
        else
        {
            Debug.Log("총알이 준비되어 있지 않습니다");
        }
    }
}
