using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Hit : MonsterState_temp, IDamageable
{
    public Monster_Hit(Monster_temp _monster) : base(_monster)
    {
        _ani = monster.Ani;
        _agent = monster.MonsterAgent;
        _hp = monster.MaxHp; 
        stateMerchine = monster._monsterMerchine;
    }

    private float _hp;
    private NavMeshAgent _agent;
    private Animator _ani;
    private bool _isInvincible = false;
    protected MonsterStateMachine_temp stateMerchine;
    private Coroutine _hitCoroutine;

    // 피격 인터페이스 구성
    public void Damaged(float PlayerAttackDamage)
    {
        //Debug.Log("몬스터 피격 확인");
        // 죽어있거나 피격 쿨타임의 경우 안맞는다.
        if (monster._isDead || _isInvincible) return;
       
        _hp -= PlayerAttackDamage;
        //Debug.Log("맞음");
        // 죽을때
        if (_hp <= 0 && !monster._isDead)
        {
            monster._isDead = true;
            stateMerchine.ChangeState(stateMerchine.StateDic[Estate.Dead]);
            return;
        }

        // if문 위에 두면 맞는 모션 나오고 죽는 모션나온다 바꾸자
        _ani.SetTrigger("IsHit");
        if (_hitCoroutine == null)
        {
            _hitCoroutine = monster.StartCoroutine(InvTime());
        }
    }

    private IEnumerator InvTime()
    {
        // 피격 상태에서 외부 상태로 이전되는 것을 방어하기 위함 + 모션안정을 위한 잠시 대기 
        stateMerchine.ChangeState(this);
        yield return new WaitForSeconds(0.8f);
        
        if (!monster._isDead && monster.PrevState != null)
        {
            stateMerchine.ChangeState(monster.PrevState);
        }
    }
    public override void Enter()
    {
        //Debug.Log("맞았다 맞았다 맞았다 맞았다.....");
    }

}
