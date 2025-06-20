using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBullet : MonoBehaviour
{
    [SerializeField] private GunData _gunData;  // ��ũ���ͺ� ������Ʈ ����

    private Rigidbody _rb;
    private Vector3 _fireDir = Vector3.forward; //���� ���󰡴� ����
    private Vector3 _startPosition;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() //Bullet ��ü�� Ȱ��ȭ �� ��
    {
        _startPosition = transform.position; // Ȱ��ȭ�� ������ ���� ��ġ ����
        _rb.velocity = transform.forward * _gunData._bulletSpeed; // ������ transform.forward ����
    }

    private void Update()
    {
        //Move(_fireDir);

        //���������� ���� ���� �Ÿ� �̻�־����� ��Ȱ��ȭ
        if (Vector3.Distance(_startPosition, transform.position) > _gunData._bulletrange)
        {
            gameObject.SetActive(false);
        }
    }

    /*public void Move(Vector3 dir)   
    { 
        // ���� ��ǥ�迡�� �̵� (���� ��ǥ�� ���� �ذ�)
        transform.position += dir.normalized * _speed * Time.deltaTime;
    }*/

    // �ܺο��� ������ �����ϴ� �޼���
    public void SetDirection(Vector3 direction)
    {
        _fireDir = direction.normalized;
    }
}
