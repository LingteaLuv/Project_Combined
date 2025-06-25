using UnityEngine;

public class InstantMonsterScirpt : MonoBehaviour
{
    public int MonsterHp = 10;
    public int MoveSpeed = 1;
    public int attackDamage = 10;
    public float detectRange = 7f;
    public float detectRadius = 1.5f;
    public float attackRange = 2f;
    public float attackRadius = 0.6f;

    public Rigidbody rb;
    public Animator anime;
    public LayerMask PlayerLayerMask;

    private float coolTime = 1f;
    private float lastAttack;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anime = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDead) return;

        Vector3 origin = transform.position + Vector3.up * 1f;
        Vector3 direction = transform.forward;

        if (Physics.SphereCast(origin, detectRadius, direction, out RaycastHit detectHit, detectRange, PlayerLayerMask))
        {
            Transform player = detectHit.transform;

            anime.SetBool("IsDetect", true);

            Vector3 dir = (player.position - transform.position).normalized;
            dir.y = 0;
            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 2f);
            }

            Vector3 velocity = dir * MoveSpeed;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

            if (Physics.SphereCast(origin, attackRadius, direction, out RaycastHit attackHit, attackRange, PlayerLayerMask))
            {
                Vector3 dirToPlayer = (attackHit.transform.position - transform.position).normalized;
                dirToPlayer.y = 0;
                float angle = Vector3.Angle(transform.forward, dirToPlayer);
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                if (distanceToPlayer <= attackRange && angle <= 45f && Time.time - lastAttack > coolTime)
                {
                    rb.velocity = Vector3.zero;
                    anime.SetBool("IsAttack", true);

                    PlayerHealth playerHealth = attackHit.transform.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.Damaged(attackDamage);
                        lastAttack = Time.time;
                    }
                }
                // 대체 왜 이렇게 마구잡이로 꺼야 되는거지;
                else
                {
                    anime.SetBool("IsAttack", false);
                }
            }
            else
            {
                anime.SetBool("IsAttack", false);
            }
        }
        else
        {
            anime.SetBool("IsDetect", false);
            anime.SetBool("IsAttack", false);
            rb.velocity = Vector3.zero;
        }
    }

    private void TakeDamage(PlayerAttack attack)
    {
        if (isDead) return;

        MonsterHp -= 1;
        if (MonsterHp <= 0)
        {
            isDead = true;
            Die();
        }
    }

    private void Die()
    {
        anime.SetTrigger("isDead");
        anime.SetBool("isDead", false);
        anime.SetBool("isAttack", false);
        rb.velocity = Vector3.zero;
    }
}
