using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour //이후 총마다 상속을 시켜 줘야 하지 않을까
{
    [Header("Gun Setting")]
    [Tooltip("Change value by gun type")]
    [SerializeField] private string _gunName;  // 총의 이름
    [SerializeField] private float _range;     // 총의 사정 거리    
    [SerializeField] public float _reloadTime;// 재장전 속도. 총의 종류마다 다름.
    [SerializeField] private Transform muzzlePoint;

    [Space(100)]
    [Tooltip("Gun Damage value")]
    public int _damage;      // 총의 공격력. 총의 종류마다 다름.

    //나중에 추가될 수 있을 것듯
    //[SerializeField] private float accuracy;  // 총의 정확도. 총의 종류마다 정확도가 다름.
    //[SerializeField] private float fireRate;  // 연사 속도. 즉 한발과 한발간의 시간 텀. 높으면 높을 수록 연사가 느려짐. 총의 종류마다 다름.
    //[SerializeField] private float retroActionForce;  // 반동 세기. 총의 종류마다 다름.


    [Header("Gun Bullet Setting")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _bulletPoolSize; //총 최대 탄약 개수 제한 및 오브젝트 풀 사이즈  

    [Header("Gun Effects Setting")]
    [Tooltip("Gun Other Setting")]
    [SerializeField] private ParticleSystem muzzleFlash;  // 화염구 이펙트 재생을 담당할 파티클 시스템 컴포넌트
    [SerializeField] private AudioClip fire_Sound;    // 총 발사 소리 오디오 클립
    
    public ObjectPool _gunBulletObjectPool;


    private void Awake()
    {
        Init();     //나중에 플레이어 해당 총 오브젝트를 습득 했을 때 Init이 되게 해야할 것 같음.
    }
    private void Init()
    {
        _gunBulletObjectPool = new ObjectPool(_bulletPoolSize, _bulletPrefab, gameObject);
    }
}
