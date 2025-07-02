using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : WeaponBase
{
    /*
     * public class ThrowItem : ItemBase
    public float Range; -> 아마 최대 범위 키를 눌렀을 때
    public int MaxStack; //?
    public string ThrowSoundResource;
    */
    [Tooltip("SO데이터")]
    [SerializeField] private ThrowItem _throwData;
    
    [Header("References")]
    public Transform cam;               //카메라 시점
    public Transform attackPoint;    //투척 포인트
    private float _damage => _throwData.AtkDamage;
    //public GameObject objectToThrow;  //던질 게임 오브젝트 프리팹 -> 추후 오브젝트 풀링 생각해보기 생성/파괴 관련
    [SerializeField] private float _moveSpeed;
    private Vector3 targetRot;
    public int throwCooldown;           //던지기 쿨다운

    private float throwUpwardForce = 5;     //던지기 위쪽 힘 

    private bool readyToThrow;          

    private void Start() => readyToThrow = true;

    /*private void Update()
    {
        if(Input.GetKeyDown(throwKey) && readyToThrow)
        {
            Throw ();
        }
    }*/

    public override void Init()
    {
        if (_weaponSpawnPos == null) {
            Debug.Log("무기의 스폰포인트가 지정되어 있지 않습니다.");
            return;
        }
        else { 
            gameObject.transform.localPosition = _weaponSpawnPos.transform.localPosition;
            gameObject.transform.localRotation = _weaponSpawnPos.transform.localRotation;
        }
    }

    public override void Attack()
    {
        if (!_canAttack)
        {
            Debug.Log("투척할 수 없는 상태입니다.");
            return;
        }
        ExecuteAttack();
    }

    private IEnumerator WaifForInput()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        transform.parent = null; // 손에서 분리

        rb.isKinematic = false;

        rb.useGravity = true;

        transform.rotation = cam.transform.rotation;

        //
        //카메라 -> 에임시스템? -> 어려울것같다
        //방향 * 힘 ?

        targetRot = transform.forward * _moveSpeed + transform.up * 1f;
        //targetRot = ((transform.forward * 1f) + (transform.up * 0.05f)).normalized;

        rb.velocity = targetRot;

        rb.maxAngularVelocity = 50;

        rb.angularVelocity = transform.right * 100;

        //rb.AddTorque(transform.right * 100);
        //rb.AddForce(cam.forward * 1/*_throwData.Rof*/, ForceMode.VelocityChange);
        //rb.AddTorque(cam.right* 300 , ForceMode.Impulse);

        Invoke(nameof(ResetThrow), throwCooldown); //이런 식으로 간단한 쿨타임 구현이 가능
        //해당 오브젝트 파괴 시점을 고려해봐야 할것 같다.
        Destroy(gameObject, 10.0f);
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

    protected override void ExecuteAttack()
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

        rb.angularVelocity = transform.right * 100;

        //rb.AddTorque(transform.right * 100);
        //rb.AddForce(cam.forward * 1/*_throwData.Rof*/, ForceMode.VelocityChange);
        //rb.AddTorque(cam.right* 300 , ForceMode.Impulse);

        //Invoke(nameof(ResetThrow), throwCooldown); //이런 식으로 간단한 쿨타임 구현이 가능
        //해당 오브젝트 파괴 시점을 고려해봐야 할것 같다.
        //Destroy(gameObject,10.0f);
    }
}
