using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : WeaponBase
{
    [Header("수류탄 셋팅값")]
    [SerializeField] private GameObject _throwPrefab;
    [SerializeField] private Transform _throwPoint;
    [SerializeField] [Range(0,5)] private float _speed;
    public override bool IsAttack => throw new System.NotImplementedException();

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    private void Reset()
    {
        ItemType = ItemType.Throw;
    }

    private void Throw()
    {
        GameObject instance = Instantiate(_throwPrefab,_throwPoint.position,Quaternion.identity);
        Rigidbody rb = instance.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * _speed;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Throw();
        }
    }

}
