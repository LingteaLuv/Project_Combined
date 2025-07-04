using System.Collections;
using UnityEngine;

public class MyGunAttackController : MonoBehaviour
{
    [Header("Gun Data")]
    [SerializeField] private MyGun _myGun;  // 스크립터블 오브젝트 참조

    private bool _canShot = true;

    private void Awake()
    {
        _myGun= GetComponent<MyGun>();
    }
    private void Update()
    {
        //나중에 플레이어 Input으로 Shot()
        if (Input.GetKeyDown(KeyCode.X) && _canShot && _myGun.CurrentAmmo > 0)
        {
            Shot();
        }
    }

    //총알 딜레이 설정
    private IEnumerator ShotDelay()
    {
        _canShot = false;
        yield return new WaitForSeconds(_myGun.GunData._fireDelay);
        _canShot = true;
    }

    //외부에서 사용할 총을 쏘는 함수
    public void Shot()
    {
        if (_myGun.CurrentAmmo  == 0)
        {
            Debug.Log("R키를 눌러 장전하세요");
        }

        //발사 할 수 있는 총알이 있는지 총알풀 검사
        GameObject bulletObj = _myGun.GetBulletPool().GetInactive();

        if (bulletObj != null)  //만약 들고 왔다면 
        {
            // 발사 딜레이 시작 (총알이 실제로 발사될 때만)
            StartCoroutine(ShotDelay());

            //총알 위치 설정
            bulletObj.transform.position = _myGun.GunData._firePoint.transform.position;

            //총알 방향 설정
            BulletBase bullet = bulletObj.GetComponent<BulletBase>();
            if (bullet != null)
            {
                bullet.SetDirection(_myGun.GunData._firePoint.transform.forward,transform);
            }
            bulletObj.SetActive(true); //해당 총알을 활성화시킴
        }
        else
        {
            Debug.Log("총알이 준비되어 있지 않습니다");
        }
    }

    private void PlayShootEffects()
    {
        // 총구 화염 이펙트
        if (_myGun.GetComponent<ParticleSystem>() != null)
        {
            _myGun.GetComponent<ParticleSystem>().Play();
        }

        // 발사음 재생
        _myGun.PlayFireSound();
    }
}
