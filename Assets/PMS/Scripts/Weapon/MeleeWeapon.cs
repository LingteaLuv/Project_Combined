using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    //[SerializeField] private PMS_Enemy _enemy;
    [SerializeField] private Transform _playerPos; //플레이어의 위치

    [Header("근접무기 셋팅값")]
    [SerializeField] private int _attackDamage; //근거리 무기의 공격력
    [SerializeField] private float _attackRange;  //근거리 무기의 유효 범위
    [SerializeField] private float _attackAngle; //근거리 무기 유효 각도
    [SerializeField] private float _attackInterval = 1.0f; // 1초에 한 번 0.5f 값이 1초에 2번때림

    [SerializeField] Animator _animator; //플레이어의 애니메이터가 필요 -> 공격시 플레이어 애니메이션 재생하기 위하여

    [Header("Attack")]
    [SerializeField] private LayerMask _targetLayer; //타겟 대상 레이어 -> 추후 몬스터가 레이어로 관리되지 않을까?

    //1번:Physics.OverlapSphere + 범위 + 각도 체크
    public override void Attack()
    {
        _animator.SetTrigger("DownwardAttack");

        // 8. 주변 범위 내, 몬스터 레이어 오브젝트의 충돌체를 배열로 저장
        Collider[] _colliders = Physics.OverlapSphere(transform.position, _attackRange, _targetLayer);

        // 9. 충돌체를 저장한 배열을 순회하며 데미지 부여 로직 실행
        foreach (Collider target in _colliders)
        {
            // 10. 공격자와 타겟의 y축을 0으로 고정하여 y축 고려X
            Vector3 targetPos = target.transform.position;
            targetPos.y = 0;
            Vector3 attackPos = _playerPos.transform.position;
            attackPos.y = 0;

            // 11. 공격자에서 타겟으로의 벡터를 계산하고 정규화
            //    벡터는 기본적으로 (종점-시점) 이므로 attackPos -> targetPos의 방향이 됨
            Vector3 targetDir = (targetPos - attackPos).normalized;

            // 12. 공격자의 정면 방향과 대상 방향 사이의 각도를 내적 이용하여 계산,
            //    공격 각도(_attackAngle)보다 크면 공격 범위 바깥이므로 무시.
            if (Vector3.Angle(_playerPos.transform.forward, targetDir) > _attackAngle * 0.5f)
                continue;

            //타겟의 IDamageable 인터페이스를 받아와서 데미지 부여
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damaged(_attackDamage); //TakeDamage함수명이 더좋을듯
            }
            // +@ 피격 시 색깔 변화
            StartCoroutine(DamageRoutine(target.gameObject));
        }

    }

    //2번 애니메이션 그냥 콜라이더 추가해버리면 공격하지 않아도 닿으면 몹들이 맞은걸로 인식하게 된다.
    public void Attack2()
    {
        Attack();
        //SetTrigger("Attack"); //플레이어가 공격을 하는 애니메이션을 재생한다
    }

    //2번 애니메이션
    public void Attack3()
    {
        // 1. 공격 애니메이션을 재생한다.
        // animator.SetTrigger("Attack");

        // 2. 애니메이션 클립의 특정 타이밍에 Animation Event를 추가하여,
        //    공격 판정(콜라이더 생성 및 활성화) 함수를 호출한다.

        // 3. 해당 함수에서 공격 범위 내 적을 감지하고 데미지를 적용한다.

        // 4. 공격 처리 후, 콜라이더 컴포넌트를 비활성화하거나 제거하여
        //    불필요한 충돌 처리를 방지한다.
    }

    //3번 애니메이션 키프레임을 활용한 감지
    public void AttackEvent()
    {

    }

    private void OnTirrigerEnter(Collision collision)
    {
        if(collision.gameObject.layer == _targetLayer)
        {
            collision.gameObject.GetComponent<IDamageable>().Damaged(_attackDamage);
        }
    }


    public Animator Animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
    }

    

    #region 기즈모 출력
    private void OnDrawGizmos()
    {
        //왼쪽 라인 플레이어로 부터 
        Vector3 leftDir = Quaternion.Euler(0, -_attackAngle * 0.5f, 0) * _playerPos.transform.forward;
        Gizmos.DrawLine(_playerPos.transform.position, _playerPos.transform.position + leftDir * _attackRange);

        Vector3 rightDir = Quaternion.Euler(0, _attackAngle * 0.5f, 0) * _playerPos.transform.forward;
        Gizmos.DrawLine(_playerPos.transform.position, _playerPos.transform.position + rightDir * _attackRange);

        //오버랩 스피어 범위
        Gizmos.DrawWireSphere(_playerPos.transform.transform.position, _attackRange);
    }
    #endregion

    #region 피격 시 색깔 변화 코루틴
    WaitForSeconds delay = new(0.2f);
    IEnumerator DamageRoutine(GameObject gameObject)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer == null) yield break;

        Color originColor = renderer.material.color;
        renderer.material.color = Color.red;
        yield return delay;
        renderer.material.color = originColor;
    }
    #endregion
}
