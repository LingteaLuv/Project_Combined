using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentWeaponCheck : StateMachineBehaviour
{
    [SerializeField] private WeaponBase _currentWeapon;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_currentWeapon == null)
        {
            _currentWeapon = PlayerWeaponManager.Instance.RightCurrentWeapon;
        }
        if (_currentWeapon != null)
        {
            if (_currentWeapon.ItemType == ItemType.Gun)
            {
                animator.SetBool("IsGun", true);
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
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
