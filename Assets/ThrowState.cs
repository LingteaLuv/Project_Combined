using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowState : StateMachineBehaviour
{
    private PlayerAttack _playerAttack;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerAttack == null)
        {
            _playerAttack = animator.GetComponent<PlayerAttack>();
        }
        animator.SetLayerWeight(3, 1f);
        _playerAttack.IsAttacking = true;
    }
}
