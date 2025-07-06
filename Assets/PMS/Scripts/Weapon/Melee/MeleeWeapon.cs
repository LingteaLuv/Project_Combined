using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [Tooltip("받아올 MeleeItem 스크립트 데이터")]
    [SerializeField] private MeleeItem _meleeData;

    public Transform _playerPos;      //플레이어의 위치
    [SerializeField] private Transform _attackPointPos; //공격의 충돌을 감지할 Pivot Transform
    [SerializeField] private LayerMask _targetLayer;

    [Tooltip("기획분들이 설정해주시면 좋겠습니다.")]
    [Header("직접 셋팅해야 하는 값")]
    [SerializeField] private float _attackRange;        //근거리 무기의 유효 범위
    [SerializeField] private float _attackAngle;        //근거리 무기 유효 각도

    private void Reset()
    {
        ItemType = ItemType.Melee;
    }

    public void Awake()
    {
        base.Init();
        _playerPos = transform.root;
    }

    public override void Attack()
    {
        //공격을 조정 할 필요가없음 PlayerAttack에서 조정
        ExecuteAttack();
    }

    /// <summary>
    /// Physics.OverlapSphere + 범위 + 애니메이션 Event를 통한 특정 프레임 이벤트 호출, 각도 체크O - 플레이어기준 
    /// // TODO - Physics.OverlapSphere 콜라이더 범위 - y축 신경 X
    /// </summary>
    protected override void ExecuteAttack() 
    {
        // 무기에 달려있는 _attack를 중심으로 범위를 설정하고 타겟레이어와 충돌검사
        Collider[] colliders = Physics.OverlapSphere(_attackPointPos.position, _attackRange, _targetLayer);

        // 가장 가까운 타겟을 찾기 위한 변수 초기화
        //IDamageable closestDamageable = null;
        GameObject closeGameObject = null;
        float minDistance = float.MaxValue; // 초기 최소 거리는 무한대로 설정

        // 플레이어의 앞쪽 방향 벡터
        Vector3 playerForward = _playerPos.transform.forward;

        // 충돌체를 저장한 배열을 순회하며 가장 가까운 적 찾기
        foreach (Collider targetCollider in colliders)
        {
            Debug.Log(targetCollider.gameObject.name);

            IDamageable damageable = targetCollider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                float y = _playerPos.transform.eulerAngles.y;
                //Vector3 a = new Vector3(Mathf.Sin(y), 0, Mathf.Cos(y));
                // 플레이어에서 타겟으로의 방향 벡터
                Vector3 directionToTarget = (targetCollider.transform.position - _playerPos.transform.position).normalized;
                // 플레이어의 앞쪽 방향과 타겟 방향 사이의 각도 계산
                float angle = Vector3.Angle(playerForward, directionToTarget);
                Debug.Log(angle);
                // 각도 안에 존재하는지 확인
                if (angle <= (_attackAngle / 2))
                {
                    // 현재 공격 지점과 타겟 간의 거리 계산
                    float distance = Vector3.Distance(_attackPointPos.position, targetCollider.transform.position);
                    // 만약 현재 타겟이 이전에 찾은 타겟보다 더 가깝다면
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        //closestDamageable = damageable;
                        Debug.Log("맞을 사람 모임 -> " + targetCollider.name);
                        closeGameObject = targetCollider.gameObject;
                    }
                }
            }
        }

        // 가장 가까운 적이 있다면 데미지 부여 로직 실행
        if (closeGameObject != null)//(closestDamageable != null)
        {
            closeGameObject.GetComponent<IDamageable>().Damaged(_meleeData.AtkDamage);
            //TODO - 시각적 디버깅용 코드 추후 제거 예정
            StartCoroutine(DamageRoutine(closeGameObject.gameObject));
        }
        else
        {
            Debug.Log("공격 범위 내에 적이 없습니다.");
        }
    }
    #region 근접 무기 공격 로직 - Physics.OverlapSphere + 범위 + 각도 체크 - 플레이어기준
    //문제점: 다수의 몹을 공격하면 안된다는 기획이 나왔다.
    /// <summary>
    /// Physics.OverlapSphere + 범위 + 각도 체크 - 플레이어기준
    /// </summary>
    /*public override void Attack()
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
            // +@ 피격 시 색깔 변화 - 디버깅용
            StartCoroutine(DamageRoutine(target.gameObject));
        }

    }*/
    #endregion

    #region 근접 무기 공격 로직 - 무기에 콜라이더 달고 때리기
    //문제점:무기에 그냥 콜라이더 추가해버리면 공격하지 않아도 닿으면 몹들이 맞은걸로 인식하게 된다. 
    /*private void OnTirrigerEnter(Collision collision)
    {
        if(collision.gameObject.layer == _targetLayer)
        {
            collision.gameObject.GetComponent<IDamageable>().Damaged(_attackDamage);
        }
    }*/
    #endregion

    #region 근접 무기 공격 로직 - 애니메이션 Event를 활용하여 특정 프레임에서 충돌감지
    //2번+ 애니메이션 Event를 활용하여 특정 프레임에서 충돌감지
    //문제점:플레이어 공격 범위가 제한적이게 된다. 
    /*public void Attack3()
    {
        // 1. 공격 애니메이션을 재생한다.
        // animator.SetTrigger("Attack");
        _animator.SetTrigger("DownwardAttack");

        // 2. 애니메이션 클립의 특정 타이밍에 Animation Event를 추가하여,
        //    공격 판정(콜라이더 생성 및 활성화) 함수를 호출한다.

        // 3. 해당 함수에서 공격 범위 내 적을 감지하고 데미지를 적용한다.

        // 4. 공격 처리 후, 콜라이더 컴포넌트를 비활성화하거나 제거하여
        //    불필요한 충돌 처리를 방지한다.
    }*/
    #endregion

    #region 기즈모 출력 - 플레이어 기준
    private void OnDrawGizmos()
    {
        //왼쪽 라인 플레이어로 부터 
        Vector3 leftDir = Quaternion.Euler(0, -_attackAngle * 0.5f, 0) * _playerPos.transform.forward;
        Gizmos.DrawLine(_playerPos.transform.position, _playerPos.transform.position + leftDir * _attackRange);

        Vector3 rightDir = Quaternion.Euler(0, _attackAngle * 0.5f, 0) * _playerPos.transform.forward;
        Gizmos.DrawLine(_playerPos.transform.position, _playerPos.transform.position + rightDir * _attackRange);

        //오버랩 스피어 범위
        Gizmos.DrawWireSphere(transform.transform.position, _attackRange);
    }
    #endregion

    #region 기즈모 출력 - 무기 기준
    /*private void OnDrawGizmos()
    {
        //오버랩 스피어 범위
        Gizmos.DrawWireSphere(_attackPointPos.transform.transform.position, _attackRange);
    }*/
    #endregion

    #region 피격 시 색깔 변화 코루틴 - 디버깅용
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
