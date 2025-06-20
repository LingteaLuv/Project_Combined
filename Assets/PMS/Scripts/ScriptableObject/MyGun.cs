using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGun : MonoBehaviour
{
    [Header("Gun Data")]
    [SerializeField] private GunData _gunData;  // ��ũ���ͺ� ������Ʈ ����

    [Header("Gun Components")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioSource audioSource;

    // ��Ÿ�� ������
    private int _currentAmmo;
    private bool _isReloading = false;
    private ObjectPool _gunBulletObjectPool;

    // ������Ƽ�� �� ������ ����
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
            Debug.LogError("Gun Data�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        // �Ѿ� Ǯ ����
        _gunBulletObjectPool = new ObjectPool(_gunData._bulletPoolSize, _gunData._bulletPrefab, gameObject);

        // �ʱ� ź�� ����
        _currentAmmo = _gunData._maxLoadedAmmo;

        //�ѱ� ����
        if(!_gunData._firePoint)
        {
            //_gunData._firePoint = transform.Find("FirePoint");
            Debug.Log("firePoint�� ã��");
        }

        // ����Ʈ ����
        if (muzzleFlash == null && _gunData._muzzleFlash != null)
        {
            muzzleFlash = Instantiate(_gunData._muzzleFlash, _gunData._firePoint.transform);
        }

        // ����� �ҽ� ����
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
    /// �� ���� ����Լ� ���� ��,���� ���� �� ��,���� ź���� ���� ��
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

    //ź�� �ѹ� �Һ�
    public void ConsumeBullet()
    {
        if (_currentAmmo > 0)
        {
            _currentAmmo--;
        }
    }

    //���ε� �Լ�
    //���� ���ε� �ϰ� ���� �ʰ� ���� źâ�� maxźâ���� ������ ���ε� �ڷ�ƾ ����
    public void Reload()
    {
        if (!_isReloading && _currentAmmo < _gunData._maxLoadedAmmo)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }
    
    //���ε� ���� ������ ���ε� ���� ��� ���ε� �ð��� ������ ���� źâ�� �� ä��� ���ε带 ����
    private IEnumerator ReloadCoroutine()
    {
        _isReloading = true;
        PlayReloadSound();

        yield return new WaitForSeconds(_gunData._reloadTime);

        _currentAmmo = _gunData._maxLoadedAmmo;
        _isReloading = false;
    }
}
