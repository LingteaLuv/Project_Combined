using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : WeaponBase
{
    [SerializeField] ThrowItem _throwData;
    /*
     * public class ThrowItem : ItemBase
    public float Range;
    public int MaxStack; //?
    public string ThrowSoundResource;
    */

    [SerializeField] private ThrowItem _throwItem;
    
    [Header("References")]
    public Transform cam;               //카메라 시점
    public Transform attackPoint;    //투척 포인트
    private float _damage => _throwData.AtkDamage;   
    //public GameObject objectToThrow;  //던질 게임 오브젝트 프리팹 -> 추후 오브젝트 풀링 생각해보기 생성/파괴 관련

    [Header("Settings")]
    public int throwCooldown;           //던지기 쿨다운

    private float throwUpwardForce;     //던지기 위쪽 힘 

    private bool readyToThrow;          

    private void Start() => readyToThrow = true;

    /*private void Update()
    {
        if(Input.GetKeyDown(throwKey) && readyToThrow)
        {
            Throw ();
        }
    }*/
    
    public override void Attack()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        transform.parent = null; // 손에서 분리

        rb.AddForce(cam.forward * _throwItem.Rof, ForceMode.Impulse);

        rb.isKinematic = true;

        rb.useGravity = true;

        Invoke(nameof(ResetThrow), throwCooldown); //이런 식으로 간단한 쿨타임 구현이 가능
        
        //해당 오브젝트 파괴 시점을 고려해봐야 할것 같다.
        Destroy(gameObject,10.0f); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        //패턴 매칭
        if (collision.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
        {
            damageable.Damaged(_throwItem.AtkDamage);
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
}
