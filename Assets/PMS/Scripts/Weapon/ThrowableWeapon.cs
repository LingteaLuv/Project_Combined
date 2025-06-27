using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : WeaponBase
{
    [Header("References")]
    public Transform cam; //일단 1인칭으로 해보자
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public int throwCooldown; //던지기 쿨다운

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;  //던지기 키코드
    public float throwForce;                    //던지는 힘 
    public float throwUpwardForce;              //던지기 위쪽 힘 

    private bool readyToThrow;

    private void OnEnable()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        readyToThrow = true;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

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

        rb.isKinematic = false;
        rb.useGravity = true;

        rb.AddForce(cam.forward * 10.0f + cam.up * 10.0f, ForceMode.Impulse);

        Invoke(nameof(ResetThrow), throwCooldown); //이런 식으로 간단한 쿨타임 구현이 가능
    }

    private void Throw()
    {
        readyToThrow = false;
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

    /*[Header("수류탄 셋팅값")]
    [SerializeField] private GameObject _throwPrefab;
    [SerializeField] private Transform _throwPoint;
    [SerializeField] [Range(0,5)] private float _speed;
    */
    public override bool IsAttack => throw new System.NotImplementedException();
    


    private void Reset()
    {
        ItemType = ItemType.Throw;
    }

    /*private void Throw()
    {
        GameObject instance = Instantiate(_throwPrefab,_throwPoint.position,Quaternion.identity);
        Rigidbody rb = instance.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * _speed;
    }*/

    /*private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Throw();
        }
    }*/

}
