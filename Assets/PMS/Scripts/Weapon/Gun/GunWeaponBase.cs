using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWeaponBase : WeaponBase
{
    //총 프리팹
    [SerializeField] protected GameObject _bulletPrefab;

    //총 본체 관련 변수
    [SerializeField] protected string _gunName;     // 총의 이름
    [SerializeField] protected int _damage;       // 총의 데미지
    [SerializeField] protected float _range;        // 총의 유효 사거리
    [SerializeField] protected float _accuracy;     // 총의 정확도

    //총알 제어 관련 변수
    [SerializeField] protected int _maxAmmoCount;       //최대 탄약수
    [SerializeField] protected int _currentAmmoCount;   //현재 탄약수
    [SerializeField] protected float _reloadTime;       // 재장전 속도. 총의 종류마다 다름.
    [SerializeField] protected float _fireDelay = 1.5f; //총 발사 딜레이
    [SerializeField] protected Transform _firePoint;    //총알 발사 지점

    //궤적 설정 관련 변수
    protected LineRenderer _lineRenderer;
    [SerializeField] protected bool showTrajectory = true;

    protected bool _canShot = true;
    protected bool _isReload = false;

    //총알 오브젝트 풀링 관련 변수
    [Header("Gun other Setting")]
    [Tooltip("Gun Bullets ObjectPool Setting")]
    [SerializeField]protected int _bulletPoolSize;         //총 최대 탄약 개수 제한 및 오브젝트 풀 사이즈  
    [SerializeField]protected ParticleSystem muzzleFlash;  // 화염구 이펙트 재생을 담당할 파티클 시스템 컴포넌트
    [SerializeField]protected AudioClip fire_Sound;        // 총 발사 소리 오디오 클립
    protected ObjectPool _gunBulletObjectPool;

    public int Damage { get { return _damage; } private set { } }

    public override bool IsAttack => throw new System.NotImplementedException();

    //무기마다 Init() 불릿 풀 사이즈가 다를 것이고,불릿 프리팹이 다르다. 총 아래에 Bullet 오브젝트가 생성
    public virtual void Init()
    {       
        _lineRenderer = GetComponent<LineRenderer>();
        _gunBulletObjectPool = new ObjectPool(_bulletPoolSize, _bulletPrefab, gameObject);
    }

    protected AudioSource audioSource; //공격 사운드

    public override void Attack() { }

    #region 총알 궤적을 보여주는 함수
    /// <summary>
    /// 총알 궤적을 보여주는 함수
    /// </summary>
    /// <param name="bullet"> bullet 객체 </param>
    /// <param name="bulletSpeed">bullet 객체의 스피드 </param>
    public void UpdateTrajectory(GameObject bullet, float bulletSpeed)
    {
        // 궤적을 그리는데 사용될 정점 개수
        int pointCount = 30;
        // 정점 간의 시간차
        float deltaTime = 0.1f;
        Vector3[] trajectorys = new Vector3[pointCount];
        // 궤적의 시작점
        Vector3 startPos = _firePoint.transform.position;
        // 무기 방향 → 카메라 방향
        /*transform.rotation = Quaternion.Euler
        (_camera.transform.eulerAngles.x, _camera.transform.eulerAngles.y, 0);*/
        // 발사체 방향 = 무기 방향
        //bullet.transform.rotation = transform.rotation;
        float speed = bulletSpeed;
        // 궤적 및 발사체 시작 속도
        Vector3 startVel = transform.forward * speed;
        // 궤적 발사체 운동 동기화, 계산
        for (int i = 0; i < pointCount; i++)
        {
            trajectorys[i] = CalculatePoint(startPos, startVel, deltaTime * i);
        }
        // 
        _lineRenderer.positionCount = pointCount;
        // 렌더러로 계산된 궤적 표시
        _lineRenderer.SetPositions(trajectorys);
    }
    private Vector3 CalculatePoint(Vector3 startPos, Vector3 startVel, float time)
    {
        return startPos + startVel * time;
    }
    #endregion
}
