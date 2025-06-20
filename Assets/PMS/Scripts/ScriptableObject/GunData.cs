// 1. 총기 데이터를 담는 ScriptableObject 생성
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun Data", menuName = "Weapons/Gun Data")]
public class GunData : ScriptableObject
{
    [SerializeField] public GameObject gunPrefab; //총의 프리팹

    [Header("Gun Setting")]
    [Tooltip("Change gun performance value by gun type")]
    [SerializeField] public string _gunName;  // 총의 이름
    [SerializeField] public float _damage;    //총의 데미지
    [SerializeField] public float _fireDelay = 0.1f; // 발사 간격
    [SerializeField] public float _reloadTime;// 재장전 속도. 총의 종류마다 다름.

    //나중에 추가될 수 있을 것듯
    //[SerializeField] private float accuracy;  // 총의 정확도. 총의 종류마다 정확도가 다름.
    //[SerializeField] private float fireRate;  // 연사 속도. 즉 한발과 한발간의 시간 텀. 높으면 높을 수록 연사가 느려짐. 총의 종류마다 다름.
    //[SerializeField] private float retroActionForce;  // 반동 세기. 총의 종류마다 다름.

    [Header("Gun Bullet Setting")]
    [SerializeField] public GameObject _bulletPrefab; //총알 프리팹
    [SerializeField] public Transform _firePoint; //총알 발사 지점
    [SerializeField] public int _maxLoadedAmmo; //최대 장전 할 수 있는 탄약수
    [SerializeField] public float _bulletSpeed;  //총알 스피드
    [SerializeField] public float _bulletrange;     // 총알 유효 사거리    


    [Header("Gun Effects Setting")]
    [SerializeField] public ParticleSystem _muzzleFlash;  // 화염구 이펙트 재생을 담당할 파티클 시스템 컴포넌트
    [SerializeField] public AudioClip _fireSound;    // 총 발사 소리 오디오 클립
    [SerializeField] public AudioClip _reloadSound;  // 총의 리로드 사운드
    [SerializeField] public AudioClip _emptySound;  //빈 탄창음

    [SerializeField] public int _bulletPoolSize; //발사 할 수 있는 총 최대 탄약 개수 제한 및 오브젝트 풀 사이즈  
    private ObjectPool _gunBulletObjectPool;
    public Property<ObjectPool> GunBulletObjectPool;
}
