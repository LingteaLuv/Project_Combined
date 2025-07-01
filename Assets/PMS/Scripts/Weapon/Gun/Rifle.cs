using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : GunWeaponBase 
{   
    [SerializeField] private Animator _animator;
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
        if (_animator.GetBool("IsAim") == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                InventoryManager.Instance.DecreaseWeaponDurability();
            }
            // 마우스 우클릭을 누르는 순간 (조준 시작)
            if (Input.GetMouseButtonDown(1))
            {
                // 총알 궤적 미리보기를 위한 로직 (옵션)

                _lineRenderer.enabled = true; // 라인 렌더러 활성화
                GameObject bulletObj = _gunBulletObjectPool.GetInactive(); // 궤적 미리보기를 위해 임시 객체 가져오기 (실제 발사 아님)
                UpdateTrajectory(bulletObj, bulletObj.GetComponent<BulletBase>()._speed); // 궤적 업데이트
            }
            // 마우스 우클릭이 눌려 있는 동안 (조준 유지)
            if (Input.GetMouseButton(1))
            {
                // 여기서는 궤적 미리보기를 계속 업데이트
                if (_lineRenderer != null && showTrajectory) // showTrajectory 변수를 활용하여 궤적 표시 여부 제어
                {
                    _lineRenderer.enabled = true; // 라인 렌더러 활성화
                    UpdateTrajectory(null, _bulletPrefab.GetComponent<BulletBase>()._speed); // 궤적 미리보기 업데이트
                }
                if (Input.GetMouseButtonUp(0))
                {
                    InventoryManager.Instance.DecreaseWeaponDurability();
                    Attack();
                }
            }
            // 마우스 우클릭을 떼는 순간 (조준 해제)
            if (Input.GetMouseButtonUp(1))
            {
                if (_lineRenderer != null)
                {
                    _lineRenderer.enabled = false; // 라인 렌더러 비활성화
                    _lineRenderer.positionCount = 0; // 혹시 모를 잔상을 위해 정점 개수를 0으로 설정
                }
                _animator.SetBool("IsAim", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !_isReload)
        {
            //이미 탄창이 max탄창이거나 && 총알이 없을 없을 때 false return
            InventoryManager.Instance.Consume.Reload();
        }
    }

    //총알 딜레이 설정
    private IEnumerator ShotDelay()
    {
        playerAttack.IsAttacking = false;
        yield return new WaitForSeconds(_fireDelay);
        playerAttack.IsAttacking = true;
    }
    private IEnumerator ReloadCorutine()
    {
        _isReload = true;
        yield return new WaitForSeconds(_reloadTime);
        _isReload = false;
    }

    //외부에서 사용할 총을 쏘는 함수
    public override void Attack()
    {
        if (_item.CurrentAmmoCount == 0)
        {
            Debug.Log("R키를 눌러 장전하세요");
            return;
        }

        //발사 할 수 있는 총알이 있는지 총알풀 검사
        GameObject bulletObj = _gunBulletObjectPool.GetInactive();

        if (bulletObj != null && _item.CurrentAmmoCount > 0)  //만약 들고 왔다면 
        {
            // 발사 딜레이 시작 (총알이 실제로 발사될 때만)
            StartCoroutine(ShotDelay());
            //총알 위치 설정
            bulletObj.transform.position = _firePoint.transform.position;

            //총알 방향 설정
            BulletBase bullet = bulletObj.GetComponent<BulletBase>();
            bullet.SetDamage(_gunData.AtkDamage);
            if (bullet != null)
            {
                bullet.SetDirection(_firePoint.forward);
            }
            bulletObj.SetActive(true); //해당 총알을 활성화시킴
            _bulletcaseParticle.Play();
        }
        else
        {
            Debug.Log("총알이 준비되어 있지 않습니다");
        }
    }
}
