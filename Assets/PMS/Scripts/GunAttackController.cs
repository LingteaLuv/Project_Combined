using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttackController : MonoBehaviour
{
    [SerializeField] private Gun _currentGun; //Gun과 gunAttackConrtoller는 연동
    [SerializeField] private Transform _firePoint; //총알 발사 지점
    [SerializeField] private float _fireDelay = 1.5f; //총 발사 딜레이

    [Space(10)]
    [Header("Legacy Information")]
    [Tooltip("현재 총의 남아있는 탄약수 ,나중에 인벤토리에서 들고와야하는 정보 총이 알수가 없음")]
    [SerializeField] private int _currentAmmo = 30; 

    private bool _canShot = true;

    private void Start()
    {
        if(!_currentGun)
        {
            _currentGun = gameObject.GetComponent<Gun>();
        }
    }

    private void Update()
    {
        //나중에 플레이어 Input으로 Shot()
        if(Input.GetKeyDown(KeyCode.X) && _canShot && _currentAmmo > 0)
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
        if (_currentAmmo == 0)
        {
            Debug.Log("R키를 눌러 장전하세요");
        }

        //발사 할 수 있는 총알이 있는지 총알풀 검사
        GameObject bulletObj = _currentGun._gunBulletObjectPool.GetInactive();

        if (bulletObj != null)  //만약 들고 왔다면 
        {
            // 발사 딜레이 시작 (총알이 실제로 발사될 때만)
            StartCoroutine(ShotDelay());

            //총알 위치 설정
            bulletObj.transform.position = _firePoint.transform.position;

            //총알 방향 설정
            Bullet bullet = bulletObj.GetComponent<Bullet>();
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
