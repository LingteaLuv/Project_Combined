using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ThrowableWeapon : WeaponBase
{

    [Tooltip("SO데이터")]
    [SerializeField] private ThrowItem _throwData;
    private float _damage => _throwData.AtkDamage;

    [Header("References Value")]
    [SerializeField] private Transform cam;                  //카메라 시점
    [SerializeField] private Transform attackPoint;          //투척 포인트
    [SerializeField][Range(0, 20)] private float _moveSpeed; //인스펙터 창에서 값조절하기

    private Vector3 targetDir;               //타겟 방향 Direction
    private bool readyToThrow;               //던지가 가능한지
    private int throwCooldown = 3;           //던지기 쿨다운
    private Coroutine throwCorouine;         //throw코루틴 저장

    [Header("Charge Settings")]
    [SerializeField] private float minSpeed = 1f;      // 최소 속도
    [SerializeField] private float maxSpeed = 20f;     // 최대 속도
    [SerializeField] private float maxChargeTime = 3f; // 최대 차징 시간
    [SerializeField] private AnimationCurve powerCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // 파워 증가 곡선 

    [Header("날아가면서 회전 횟수")]
    private float throwRotationMaxValue;    //max회전값
    private float throwRotationValue;       //돌아가는 회전값

    //LineRender Value
    public float projectileSpeed = 10f;     //라인렌더러가 사용하는 speed값인데 변경필요
    public int lineSegmentCount = 30;       //정점의 개수
    public float timeStep = 0.1f;           //0.1초마다 한번씩 찍기
    private LineRenderer lineRenderer;      //라인렌더러 변수
    private Vector3 gravity;                //중력값

    // 차징 관련 변수
    private float chargeStartTime;
    private float currentChargeTime;
    private float currentPower;
    private bool isCharging;

    private void Start() => readyToThrow = true;


    private void Update()
    {
        HandleInput();
        UpdateCharging();
        DrawTrajectory();
    }
    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && readyToThrow)  // (Input.GetMouseButton(0)) 
        {
            StartCharging();
        }
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            ExecuteAttack();
            StopCharging();
        }
    }

    private void Awake()
    {
        base.Init();
        lineRenderer = GetComponent<LineRenderer>();
        gravity = Physics.gravity;
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

    private void UpdateCharging()
    {
        if (!isCharging) return;

        currentChargeTime = Time.time - chargeStartTime;

        // 최대 차징 시간을 넘지 않도록 제한
        currentChargeTime = Mathf.Min(currentChargeTime, maxChargeTime);

        // 파워 계산 (0~1 사이의 값)
        float normalizedTime = currentChargeTime / maxChargeTime;
        currentPower = powerCurve.Evaluate(normalizedTime);

        // 현재 속도 계산
        float currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, currentPower);

        // 디버그 정보 출력 (옵션)
        Debug.Log($"차징 시간: {currentChargeTime:F2}s, 파워: {currentPower:F2}, 속도: {currentSpeed:F2}");
    }

    private void StopCharging()
    {
        isCharging = false;
    }


    protected override void ExecuteAttack()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        transform.parent = null; // 손에서 분리

        rb.isKinematic = false;

        rb.useGravity = true;

        float finalSpeed = Mathf.Lerp(minSpeed, maxSpeed, currentPower);

        targetDir = transform.forward * finalSpeed;
        rb.velocity = targetDir;

        // TODO - speed값에 따라 회전 값을 다르게 해줘야 할 것 같다.
        rb.maxAngularVelocity = 50;

        rb.angularVelocity = transform.right * 30;

        Debug.Log($"투척 실행! 최종 속도: {finalSpeed:F2}");


        Invoke(nameof(ResetThrow), throwCooldown); 
        Destroy(gameObject, 10.0f);
    }

    private IEnumerator HoldThrow()
    { 

        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));

        /*Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        transform.parent = null; // 손에서 분리

        rb.isKinematic = false;

        rb.useGravity = true;

        //transform.rotation = cam.transform.rotation;

        targetRot = transform.forward * _moveSpeed;

        rb.velocity = targetRot;
        
        rb.maxAngularVelocity = 1;

        rb.angularVelocity = transform.right * 1;


        Invoke(nameof(ResetThrow), throwCooldown); //이런 식으로 간단한 쿨타임 구현이 가능
        //해당 오브젝트 파괴 시점을 고려해봐야 할것 같다.
        Destroy(gameObject, 10.0f);*/
    }

    //포물선 궤적 그리는 함수
    private void DrawTrajectory()
    {
        Vector3 startPos = transform.position;

        // 차징 중이면 현재 파워에 따른 속도로, 아니면 최소 속도로
        float trajectorySpeed = isCharging ?
            Mathf.Lerp(minSpeed, maxSpeed, currentPower) :
            minSpeed;

        Vector3 startVelocity = transform.forward * trajectorySpeed;

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
        /*Vector3 startPos = transform.position;
        Vector3 startVelocity = transform.forward * _moveSpeed;

        Vector3[] points = new Vector3[lineSegmentCount];

        for (int i = 0; i < lineSegmentCount; i++)
        {
            float t = i * timeStep;
            //포물선 궤적(Projectile motion)
            Vector3 point = startPos + startVelocity * t + 0.5f * gravity * t * t;
            points[i] = point;
        }

        lineRenderer.positionCount = lineSegmentCount;
        lineRenderer.SetPositions(points);*/
    }


    private void OnCollisionEnter(Collision collision)
    {
        //패턴 매칭
        if (collision.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
        {
            damageable.Damaged(1/*_throwData.AtkDamage*/);
        }
    }

    /*private void Throw()
    {
        readyToThrow = false;

        //Instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        //get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calulate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        //add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce; //(방향→ * 던지는 힘) + (방향↑ * 던지는 힘) 

        projectileRb.AddForce(cam.forward * 10.0f ,ForceMode.Impulse);
       

        //implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown); //이런 식으로 간단한 쿨타임 구현이 가능
    }*/

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    private void Reset()
    {
        ItemType = ItemType.Throw;
    }

    /*protected override void ExecuteAttack()
    {
        //StartCoroutine(WaifForInput());

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        transform.parent = null; // 손에서 분리

        rb.isKinematic = false;

        rb.useGravity = true;

        transform.rotation = cam.transform.rotation;

        //
        //카메라 -> 에임시스템? -> 어려울것같다
        //방향 * 힘 ?

        targetRot = transform.forward * _moveSpeed;
        //targetRot = ((transform.forward * 1f) + (transform.up * 0.05f)).normalized;

        rb.velocity = targetRot;

        rb.maxAngularVelocity = 100;

        rb.angularVelocity = transform.right * 100;*/

        //rb.AddTorque(transform.right * 100);
        //rb.AddForce(cam.forward * 1/*_throwData.Rof*/, ForceMode.VelocityChange);
        //rb.AddTorque(cam.right* 300 , ForceMode.Impulse);

        //Invoke(nameof(ResetThrow), throwCooldown); //이런 식으로 간단한 쿨타임 구현이 가능
        //해당 오브젝트 파괴 시점을 고려해봐야 할것 같다.
        //Destroy(gameObject,10.0f)};
}
