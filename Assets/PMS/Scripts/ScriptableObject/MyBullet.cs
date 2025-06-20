using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBullet : MonoBehaviour
{
    [SerializeField] private GunData _gunData;  // 스크립터블 오브젝트 참조

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
        _rb.velocity = transform.forward * _gunData._bulletSpeed; // 방향은 transform.forward 기준
    }

    private void Update()
    {
        //Move(_fireDir);

        //시작점으로 부터 일정 거리 이상멀어지면 비활성화
        if (Vector3.Distance(_startPosition, transform.position) > _gunData._bulletrange)
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
}
