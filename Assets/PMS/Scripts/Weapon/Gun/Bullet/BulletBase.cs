using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField] private int _currentBulletDamage;

    [SerializeField] private float _speed = 10f; //총알 스피드
    [SerializeField] private float _distance = 50f; //총알 유효 사거리

    private Rigidbody _rb;
    private Vector3 _fireDir = Vector3.forward; //총이 날라가는 방향
    private Vector3 _startPosition;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() //Bullet 객체가 활성화 될 때
    {
        _startPosition = transform.position; // 활성화될 때마다 시작 위치 저장
        _rb.velocity = _fireDir * _speed; // 방향은 transform.forward 기준
    }

    private void Update()
    {
        //Move(_fireDir);

        //시작점으로 부터 일정 거리 이상멀어지면 비활성화
        if (Vector3.Distance(_startPosition, transform.position) > _distance)
        {
            gameObject.SetActive(false);
        }
    }

    /*public void Move(Vector3 dir)   
    { 
        // 월드 좌표계에서 이동 (로컬 좌표계 문제 해결)
        transform.position += dir.normalized * _speed * Time.deltaTime;
    }*/

    // 외부에서 방향을 설정하는 메서드
    public void SetDirection(Vector3 direction)
    {
        _fireDir = direction.normalized;
    }

    public void SetDamage(int damage)
    {
        _currentBulletDamage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    { 
        //몬스터 뿐만 아니라 모든 IDamageable을 가지고 있는 모든 객체들
        if(collision.gameObject.GetComponent<IDamageable>() != null)
        {
            Debug.Log(collision.gameObject.name);
            gameObject.SetActive(false);
            collision.transform.GetComponent<IDamageable>().Damaged(_currentBulletDamage);
        }
        //벽에 닿으면 없애기
        /*if (collision.gameObject.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }*/
        //몬스터 닿으면 데미지 처리
        /*if(collision.gameObject.CompareTag("Monster"))
        {
            collision.transform.GetComponent<IDamageable>().Damaged(_currentGunDamage);
            gameObject.SetActive(false);
        }*/
    }
}
