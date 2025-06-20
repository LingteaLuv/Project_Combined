using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGun : MonoBehaviour
{
    [Header("Gun Data")]
    [SerializeField] private GunData _gunData;  // 스크립터블 오브젝트 참조

    [Header("Gun Components")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioSource audioSource;

    // 런타임 데이터
    private int _currentAmmo;
    private bool _isReloading = false;
    private ObjectPool _gunBulletObjectPool;

    // 프로퍼티로 건 데이터 접근
    public GunData GunData => _gunData;
    public int CurrentAmmo => _currentAmmo;
    public int MaxAmmo => _gunData._maxLoadedAmmo;
    public bool IsReloading => _isReloading;
    public bool CanShoot => !_isReloading && _currentAmmo > 0;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (_gunData == null)
        {
            Debug.LogError("Gun Data가 할당되지 않았습니다!");
            return;
        }

        // 총알 풀 생성
        _gunBulletObjectPool = new ObjectPool(_gunData._bulletPoolSize, _gunData._bulletPrefab, gameObject);

        // 초기 탄약 설정
        _currentAmmo = _gunData._maxLoadedAmmo;

        //총구 설정
        if(!_gunData._firePoint)
        {
            //_gunData._firePoint = transform.Find("FirePoint");
            Debug.Log("firePoint를 찾음");
        }

        // 이펙트 설정
        if (muzzleFlash == null && _gunData._muzzleFlash != null)
        {
            muzzleFlash = Instantiate(_gunData._muzzleFlash, _gunData._firePoint.transform);
        }

        // 오디오 소스 설정
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void SetGunData(GunData newGunData)
    {
        _gunData = newGunData;
        Init();
    }

    public ObjectPool GetBulletPool()
    {
        return _gunBulletObjectPool;
    }

    /// <summary>
    /// 총 사운드 재생함수 총을 쏠때,총을 장전 할 때,총이 탄약이 없을 때
    /// </summary>
    public void PlayFireSound()
    {
        if (_gunData._fireSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(_gunData._fireSound);
        }
    }
    
    public void PlayReloadSound()
    {
        if (_gunData._reloadSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(_gunData._reloadSound);
        }
    }

    public void PlayEmptySound()
    {
        if (_gunData._emptySound != null && audioSource != null)
        {
            audioSource.PlayOneShot(_gunData._emptySound);
        }
    }

    //탄알 한발 소비
    public void ConsumeBullet()
    {
        if (_currentAmmo > 0)
        {
            _currentAmmo--;
        }
    }

    //리로드 함수
    //만약 리로드 하고 있지 않고 현재 탄창이 max탄창보다 적을때 리로드 코루틴 실행
    public void Reload()
    {
        if (!_isReloading && _currentAmmo < _gunData._maxLoadedAmmo)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }
    
    //리로드 중일 때에는 리로드 사운드 재생 리로드 시간이 지나면 현재 탄창을 다 채우고 리로드를 종료
    private IEnumerator ReloadCoroutine()
    {
        _isReloading = true;
        PlayReloadSound();

        yield return new WaitForSeconds(_gunData._reloadTime);

        _currentAmmo = _gunData._maxLoadedAmmo;
        _isReloading = false;
    }
}
