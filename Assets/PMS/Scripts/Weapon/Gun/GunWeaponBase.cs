using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//다음에 혼자 총이라는 클래스를 여러개 구현해보자
//다양한 방법으로 총을 구현해보고, 다양한 종류의 총(ex)샷건,저격총)을 구현해보자.
//다양한 총관련 시스템, 1인칭 Zoom 공격
public abstract class GunWeaponBase : WeaponBase
{
    [SerializeField] protected GunItem _gunData; //건 데이터
    //사용 할 불릿 프리팹
    //추후 프리팹 매니저 관리
    [SerializeField] protected GameObject _bulletPrefab;
    //총 본체 관련 변수
    [SerializeField] protected int _damage => _gunData.AtkDamage;     // 총의 데미지
    [SerializeField] protected float _range => _gunData.Range;        // 총의 유효 사거리

    //총알 제어 관련 변수
    [SerializeField] protected int _maxAmmoCount => _gunData.AmmoCapacity;       //최대 탄약수
    [SerializeField] protected float _reloadTime;       // 재장전 속도. 총의 종류마다 다름.
    [SerializeField] protected float _fireDelay = 1.5f; //총 발사 딜레이
    [SerializeField] protected Transform _firePoint;    //총알 발사 지점

    //궤적 설정 관련 변수
    public LineRenderer _lineRenderer;
    [SerializeField] protected bool showTrajectory = true;

    protected bool _isReload = false;

    //총알 오브젝트 풀링 관련 변수
    [Header("Gun other Setting")]
    [Tooltip("Gun Bullets ObjectPool Setting")]
    [SerializeField]protected int _bulletPoolSize;         //총 최대 탄약 개수 제한 및 오브젝트 풀 사이즈  
    [SerializeField]protected ParticleSystem muzzleFlash;  // 화염구 이펙트 재생을 담당할 파티클 시스템 컴포넌트
    [SerializeField]protected AudioClip fire_Sound;        // 총 발사 소리 오디오 클립
    protected ObjectPool _gunBulletObjectPool;

    [SerializeField] protected ParticleSystem _bulletcaseParticle;  // 탄피가 떨어지는 이펙트를 재생하는 파티클 시스템

    public int Damage { get { return _damage; } private set { } }

    protected AudioSource audioSource; //공격 사운드


    private void Reset()
    {
        ItemType = ItemType.Gun; 
    }

    //무기마다 Init()해줘야 되는 사항.
    //불릿 풀 사이즈가 다를 것이고,불릿 프리팹이 다르다. 총 아래에 Bullet 오브젝트가 생성
    public override void Init()
    {
        base.Init();
        PlayerAttack.OnAttackStateChanged += OnAttackStateChanged;
        _lineRenderer = GetComponent<LineRenderer>();
        _gunBulletObjectPool = new ObjectPool(_bulletPoolSize, _bulletPrefab, gameObject);
    }

    #region 총알 궤적을 보여주는 함수
    /// <summary>
    /// 총알 궤적을 보여주는 함수 30개 정점으로 구현
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

    //한줄만 나가는 라인구현하기
    public void UpdateTrajectory()
    {
        Vector3 startPos = _firePoint.transform.position;
    }
}
