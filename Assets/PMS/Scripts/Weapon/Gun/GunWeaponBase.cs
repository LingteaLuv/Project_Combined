using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunWeaponBase : MonoBehaviour
{
    //총 프리팹
    [SerializeField] protected GameObject _bulletPrefab;

    //총 본체 관련 변수
    [SerializeField] protected string _gunName;     // 총의 이름
    [SerializeField] protected float _damage;       // 총의 데미지
    [SerializeField] protected float _range;        // 총의 유효 사거리
    [SerializeField] protected float _accuracy;     // 총의 정확도

    //총알 제어 관련 변수
    [SerializeField] protected int _maxAmmoCount;       //최대 탄약수
    [SerializeField] protected int _currentAmmoCount;   //현재 탄약수
    [SerializeField] protected float _reloadTime;       // 재장전 속도. 총의 종류마다 다름.
    [SerializeField] protected float _fireDelay = 1.5f; //총 발사 딜레이
    [SerializeField] protected Transform _firePoint;    //총알 발사 지점
    protected bool _canShot = true;

    //총알 오브젝트 풀링 관련 변수
    [Header("Gun other Setting")]
    [Tooltip("Gun Bullets ObjectPool Setting")]
    [SerializeField]protected int _bulletPoolSize;         //총 최대 탄약 개수 제한 및 오브젝트 풀 사이즈  
    [SerializeField]protected ParticleSystem muzzleFlash;  // 화염구 이펙트 재생을 담당할 파티클 시스템 컴포넌트
    [SerializeField]protected AudioClip fire_Sound;        // 총 발사 소리 오디오 클립
    protected ObjectPool _gunBulletObjectPool;

    public float Damage { get { return _damage; } private set { } }
    public virtual void Init()
    {
        //무기마다 Init() 불릿 풀 사이즈가 다를 것이고,불릿 프리팹이 다르다.
        _gunBulletObjectPool = new ObjectPool(_bulletPoolSize, _bulletPrefab, gameObject);
    }

    protected AudioSource audioSource; //공격 사운드
}
