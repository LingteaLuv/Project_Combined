using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : GunWeaponBase 
{   
    public PlayerCameraController _cameraController;

    [SerializeField][Range(-360, 360)] public float _correction;
    [SerializeField] public Animator _animator;
    public bool isAiming = false;
    private GameObject bulletObj;
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

        //_rotationRig.transform.Rotate(Vector3.right, _cameraController.OffsetY,Space.World);

        //transform.localEulerAngles = new Vector3(_cameraController.OffsetY, 0, 0) + _originalrotation;

        transform.localRotation = Quaternion.Euler(0f,_correction - _cameraController.OffsetY, -85f);

        //_rotationRig.transform.rotation = Quaternion.Euler(_cameraController.OffsetY, _rotationRig.transform.rotation.y, _rotationRig.transform.rotation.z);

        if (Input.GetKeyDown(KeyCode.R) && !_isReload)
        {
            //이미 탄창이 max탄창이거나 && 총알이 없을 없을 때 false return
            InventoryManager.Instance.Consume.Reload();
        }
    }
    public override void Attack()
    {
        ExecuteAttack();
    }

    private IEnumerator ReloadCorutine()
    {
        _isReload = true;
        yield return new WaitForSeconds(_reloadTime);
        _isReload = false;
    }

    protected override void ExecuteAttack()
    {
        if (_item == null)
        {
            Debug.Log("_item의 객체가 Null 입니다.");
            return;
        }

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
            //StartCoroutine(ShotDelay());
            _item.CurrentAmmoCount--;
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

    public void StartAim()
    {
        isAiming = true;
        _animator.SetBool("IsAim", true);
    }

    public void UpdateAim()
    {
        if(_lineRenderer.enabled == false) _lineRenderer.enabled = true;
        if (_lineRenderer != null && showTrajectory)
        {
            bulletObj = _gunBulletObjectPool.GetInactive();
            UpdateTrajectory(bulletObj, _bulletPrefab.GetComponent<BulletBase>()._speed);
        }
    }

    public void EndAim()
    {
        isAiming = false;
        _lineRenderer.enabled = false;
        _lineRenderer.positionCount = 0;
    }

    /*
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
    }*/
}
