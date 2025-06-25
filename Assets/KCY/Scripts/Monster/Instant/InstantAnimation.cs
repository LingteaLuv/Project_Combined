using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class InstantAnimation : MonoBehaviour
{
 
    public Animator ani; 
    private Rigidbody rb;
    PlayerHealth2 playerHealth;
    InstantMonsterScirpt sc;
    public bool isAttacking = false;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
        sc = GetComponentInParent<InstantMonsterScirpt>();
    }


    public IEnumerator AttackRoutine(Collider other)
    {
        if (isAttacking) yield break;
        isAttacking = true;
        Debug.Log("코루틴 시작 시도");

        ani.SetBool("IsDetect", false);
        sc.IsFrozen = true; // 이동 정지
        ani.SetBool("Attack", true); // 모션 시작
        yield return null; // 대기 
        ani.SetBool("Attack", false);
        
        PlayerHealth2 playerHealth = other.GetComponent<PlayerHealth2>();
        if (playerHealth != null)
        {
            playerHealth.Damaged(sc.attackDamage); // 데미지 1회
            Debug.Log("공격 성공_코루틴 안안안안안");
 
        }
        yield return new WaitForSeconds(2.5f); // 후딜레이 시간 (전체 쿨타임 - 앞의 시간)
        
        isAttacking = false;
        sc.IsFrozen = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sc.TakeDamage(1);
        }
    }

}
