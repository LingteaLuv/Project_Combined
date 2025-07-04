using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentWeaponCheck : StateMachineBehaviour
{
    [SerializeField] private WeaponBase _currentWeapon;

    //무기 변경 이벤트 구독시 해제 처리가 애매하다.
    //그냥 매번 새로운 값을 받아오자.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _currentWeapon = PlayerWeaponManager.Instance.RightCurrentWeapon;
        if (_currentWeapon != null)
        {
            if (_currentWeapon.ItemType == ItemType.Gun)
            {
                animator.SetBool("IsGun", true);
            }
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
    }

}
