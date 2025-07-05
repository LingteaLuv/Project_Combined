using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField] private int _currentBulletDamage;
    [SerializeField][Range(0.1f,10f)] public float _speed; //총알 스피드
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
        // 활성화될 때마다 시작 위치 저장 -> 거리를 알아서 일정거리 밖으로 나가면 비활성화 처리를 위하여
        // 방향은 transform.forward 기준        
        _startPosition = transform.position;   
        _rb.velocity = _fireDir * _speed;       
    }

    private void Update()
    {
        //시작점으로 부터 일정 거리 이상멀어지면 비활성화
        if (Vector3.Distance(_startPosition, transform.position) > _distance)
        {
            gameObject.SetActive(false);
        }
    }

    // 외부에서 방향을 설정하는 메서드 
    // 총구의 회전값도 들고오기
    public void SetDirection(Vector3 direction,Quaternion rotation)
    {
        _fireDir = direction.normalized;
        transform.rotation = Quaternion.LookRotation(_fireDir) * Quaternion.Euler(-90, 0, 0); 
    }

    // 외부에서 데미지를 받아온다
    public void SetDamage(int damage)
    {
        _currentBulletDamage = damage;
    }


    // IDamageable 컴포넌트 확인
    private void OnCollisionEnter(Collision collision)
    {
        //플레이어는 무시
        if (collision.gameObject.CompareTag("Player")) return;

        //IDamageable을 가지고 있는 객체를 담는다.
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        //피해를 받을 객체가 있다면
        if (damageable != null)
        {
            //피해 대상자 확인용 Log 디버깅
            Debug.Log(collision.gameObject.name);
            damageable.Damaged(_currentBulletDamage);  // 먼저 데미지 처리
            gameObject.SetActive(false);               // 그 다음 비활성화
        }
        else
        {
            gameObject.SetActive(false);  // 벽이나 다른 오브젝트와 충돌시 
        }
    }
}