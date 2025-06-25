using UnityEngine;

public class InstantMonsterScirpt : MonoBehaviour
{
    public int MonsterHp = 10;  // 몬스터 체력
    public int MoveSpeed = 1;  // 몬스터 속도
    public int attackDamage = 1; // 몬스터 데미지
    public Rigidbody rb; // 충돌 및 감지용 콜라이더
    public LayerMask PlayerLayerMask; // 플레이어 감지를 위한 플레이어 레이어를 가져오기
    private bool _isDead = false;
    private InstantAnimation ani;
    private bool isDetecting = false;
    public bool IsFrozen = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponentInChildren<InstantAnimation>();
        PlayerLayerMask = LayerMask.GetMask("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isDead) return;
        DetectAndMove(other);
    }
    private void OnTriggerStay(Collider other)
    {
        if (_isDead) return;
        DetectAndMove(other);
        Attack(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (_isDead) return;
        if ((PlayerLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            ani.ani.SetBool("IsDetect", false);
        }
    }



    public void DetectAndMove(Collider other)
    {
        if (_isDead || IsFrozen) return;

        if ((PlayerLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {

            if (!ani.isAttacking && ani.ani.GetBool("IsDetect") == false)
            {
                ani.ani.SetBool("IsDetect", true);
            }

            Vector3 dir = other.transform.position - transform.position;
            dir.y = 0;
            dir = dir.normalized;

            if (dir.sqrMagnitude > 0)
            {
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 3f);
            }

            rb.MovePosition(transform.position + dir * MoveSpeed * Time.deltaTime);

            if (!isDetecting)
            {
                isDetecting = true;
                ani.ani.SetBool("IsDetect", true);
                Debug.Log("IsDetect true");
            }
        }
    }

    public void Attack(Collider other)
    {
        if (_isDead||ani.isAttacking) return;

        if ((PlayerLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            // 몬스터와 플레이어 사이의 거리
            float distance = Vector3.Distance(transform.position, other.transform.position);

            // 둘 사이의 방향
            Vector3 direction = other.transform.position - transform.position;
            direction.y = 0;
            direction = direction.normalized;

            float angle = Vector3.Angle(direction, transform.forward);
            //거리가 가까우면
            if (distance < 1.5f && angle < 45f)
            {
                PlayerHealth2 playerHealth = other.GetComponent<PlayerHealth2>();
                if (playerHealth != null)
                {
                    ani.ani.SetBool("IsDetect", false);
                    Debug.Log("코루틴 시작 시도");
                    StartCoroutine(ani.AttackRoutine(other));
                   

                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        MonsterHp -= damage;
        ani.ani.SetTrigger("IsHit");

        if (MonsterHp <= 0)
        {
            _isDead = true;
            Die();
        }
    }
    private void Die()
    {
        rb.velocity = Vector3.zero;
        ani.ani.SetTrigger("Dead");
        Destroy(gameObject, 10f);
        Debug.Log("몬스터가 소멸했어요");
    }

}

