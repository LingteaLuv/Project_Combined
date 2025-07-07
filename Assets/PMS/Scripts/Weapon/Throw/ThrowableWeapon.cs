using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ThrowableWeapon : WeaponBase
{
    private Animator _animator; 
    [Tooltip("SO데이터")]
    [SerializeField] private ThrowItem _throwData;
    private float _damage => _throwData.AtkDamage;
    //private float _range = 5.0f;
    private Vector3 _startPos;

    [Header("References Value")]
    [SerializeField] private Transform cam;                  //카메라 시점
    //[SerializeField] private Transform attackPoint;          //투척 포인트
    private float _moveSpeed; //인스펙터 창에서 값조절하기

    private Vector3 targetDir;               //타겟 방향 Direction
    private bool isThrowing;               //던지가 가능한지
    private int throwCooldown = 3;           //던지기 쿨다운
    private Coroutine throwCorouine;         //throw코루틴 저장

    [Header("Charge Settings")]
    [SerializeField] private float minSpeed = 1f;      // 최소 속도
    [SerializeField] private float maxSpeed = 20f;     // 최대 속도
    [SerializeField] private float maxChargeTime = 3f; // 최대 차징 시간
    [SerializeField] private AnimationCurve powerCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // 파워 증가 곡선 

    [Header("날아가면서 회전 횟수")]
    private float throwRotationMaxValue = 50;    //max회전값
    private float throwRotationValue;       //돌아가는 회전값
    private float MaxthrowRotationValue = 20;    //돌아가는 회전값의 최대
    private float MinthrowRotationValue = 1;   //돌아가는 회전값의 최소

    //LineRender Value
    private float projectileSpeed = 10f;     //라인렌더러가 사용하는 speed값인데 변경필요
    private int lineSegmentCount = 30;       //정점의 개수
    private float timeStep = 0.1f;           //0.1초마다 한번씩 찍기
    private LineRenderer lineRenderer;      //라인렌더러 변수
    private Vector3 gravity;                //중력값
    private Rigidbody rb;                   //리지드바디
    private bool flag = true;
    // 차징 관련 변수
    private float chargeStartTime;
    private float currentChargeTime;
    private float currentPower;
    private float currentRotationValue;
    public bool isCharging;

    private bool finish_attack = false;

    //차징 시간 UI보여줄 목적
    public float CurrentChargeNormalized => Mathf.Clamp01(currentChargeTime / maxChargeTime);

    private void Start()
    {
        _animator = transform.root.GetComponent<Animator>();
    }

    private void Awake()
    {
        cam = Camera.main.transform;
        base.Init();
        lineRenderer = GetComponent<LineRenderer>();
        rb = gameObject.GetComponent<Rigidbody>();
        gravity = Physics.gravity;
        _itemType = ItemType.Throw;
    }

    private void Update()
    {
        if (!_animator.GetBool("IsThrow") && !finish_attack)
        {
            HandleInput();
            UpdateCharging();
        }
    }
    public void HandleInput()
    {
        if (Input.GetMouseButton(0) && flag == true && !UIManager.Instance.IsUIOpened.Value) 
        {
            _animator.SetTrigger("Throw");
            _animator.SetBool("IsCharging", true);
            flag = false;
        }
        if (Input.GetMouseButtonDown(0) && !UIManager.Instance.IsUIOpened.Value)  // (Input.GetMouseButton(0)) 
        {
            StartCharging();
        }
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            //왜 던지기 전에 호출해야지 Item 스택이 잘깎이는 걸까?
            InventoryManager.Instance.Controller.ReduceEquippedItem(0, 1);
            _animator.SetBool("IsCharging", false);
            ExecuteAttack();
            StopCharging();         

            //InventoryManager.Instance.Controller.ReduceEquippedItem(0, 1);
            //StartCoroutine(Delay());
        }
        /*if(isThrowing)
        {
            if (_range < Vector3.Distance(_startPos, transform.position))
            Destroy(gameObject);
        }*/
    }

    public override void Attack()
    {
        base.Attack();
    }

    //차징 시간에 따른 Max값 올리기
    private void StartCharging()
    {
        isCharging = true;
        chargeStartTime = Time.time;
        currentChargeTime = 0f;
        currentPower = 0f;
        Debug.Log("차징 시작!");
    }

    public void UpdateCharging()
    {
        if (!isCharging) return;

        DrawTrajectory();

        currentChargeTime = Time.time - chargeStartTime;

        // 최대 차징 시간을 넘지 않도록 제한
        currentChargeTime = Mathf.Min(currentChargeTime, maxChargeTime);

        // 파워 계산 (0~1 사이의 값)
        float normalizedTime = currentChargeTime / maxChargeTime;
        currentPower = powerCurve.Evaluate(normalizedTime);
        currentRotationValue = powerCurve.Evaluate(normalizedTime);

        // 현재 속도 계산
        float currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, currentPower);
        float rotationValue = Mathf.Lerp(MinthrowRotationValue, MaxthrowRotationValue, currentRotationValue);

        // 디버그 정보 출력 (옵션)
        //Debug.Log($"차징 시간: {currentChargeTime:F2}s, 파워: {currentPower:F2}, 속도: {currentSpeed:F2}, 회전속도: {rotationValue:F2}");
    }

    private void StopCharging()
    {
        if (_throwData.ThrowSoundResource)
        {
            AudioManager.Instance.PlaySFX(_throwData.ThrowSoundResource, _animator.transform.position);
        }
        isCharging = false;
        finish_attack = true;
        ClearTrajectory();
    }


    protected override void ExecuteAttack()
    {
        transform.parent = null; // 손에서 분리

        _startPos = transform.position;

        rb.isKinematic = false;

        rb.useGravity = true;

        float finalSpeed = Mathf.Lerp(minSpeed, maxSpeed, currentPower);
        float rotationValue = Mathf.Lerp(MinthrowRotationValue, MaxthrowRotationValue, currentRotationValue);

        targetDir = cam.transform.forward * finalSpeed;
        rb.velocity = targetDir;    

        // TODO - speed값에 따라 회전 값을 다르게 해줘야 할 것 같다.
        rb.maxAngularVelocity = throwRotationMaxValue;

        transform.rotation = cam.transform.rotation;
        rb.angularVelocity = cam.transform.forward * rotationValue;

        Debug.Log($"투척 실행! 최종 속도: {finalSpeed:F2}, 회전값 : {rotationValue:F2}");


        //Invoke(nameof(ResetThrow), throwCooldown); 
        Destroy(gameObject, 10.0f);
    }

    private void ResetThrow()
    {

    }

    //포물선 궤적 그리는 함수
    private void DrawTrajectory()
    {
        Vector3 startPos = transform.position;

        // 차징 중이면 현재 파워에 따른 속도로, 아니면 최소 속도로
        float trajectorySpeed = isCharging ?
            Mathf.Lerp(minSpeed, maxSpeed, currentPower) :
            minSpeed;

        Vector3 startVelocity = cam.transform.forward * trajectorySpeed;

        Vector3[] points = new Vector3[lineSegmentCount];

        for (int i = 0; i < lineSegmentCount; i++)
        {
            float t = i * timeStep;
            //포물선 궤적(Projectile motion)
            Vector3 point = startPos + startVelocity * t + 0.5f * gravity * t * t;
            points[i] = point;
        }

        lineRenderer.positionCount = lineSegmentCount;
        lineRenderer.SetPositions(points);
    }

    private void ClearTrajectory()
    {
        lineRenderer.positionCount = 0; // 정점을 0개로 만들어 화면에서 사라지게
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        //패턴 매칭
        if (other.gameObject.transform.root.GetComponent<IDamageable>() is IDamageable damageable)
        {
            damageable.Damaged(_damage);
        }
        Destroy(gameObject);
    }
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.0f);
        InventoryManager.Instance.Controller.ReEquip();
    }


    /*private IEnumerator OneFrame()
    {
        yield return new WaitForEndOfFrame();
        if (_item.StackCount == 1)
        {
            Debug.Log("리턴하면안댐");
        }
        else if (_item.StackCount >= 2)
        {
            _recycle = Instantiate(PlayerWeaponManager.Instance._throwDagger,
                PlayerWeaponManager.Instance._right_Hand_target.transform);
        }

        InventoryManager.Instance.Controller.ReduceEquippedItem(0, 1);
        if (_recycle != null)
        {
            _recycle.GetComponent<ThrowableWeapon>()._item = _item;
        }

        yield return new WaitForEndOfFrame();

        PlayerWeaponManager.Instance.UpdateCurrentWeapon();
    }*/

}
