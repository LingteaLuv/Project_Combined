using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentWeaponCheck : StateMachineBehaviour
{
    [SerializeField] private PlayerAttack _playerAttack;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerAttack == null)
        {
            _playerAttack = animator.GetComponent<PlayerAttack>();
        }
        if (_playerAttack != null && _playerAttack.CurrentWeapon != null)
        {
            if (_playerAttack.CurrentWeapon.ItemType == ItemType.Gun)
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
