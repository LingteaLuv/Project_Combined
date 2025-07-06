using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw_AttackState : StateMachineBehaviour
{
    private PlayerAttack _playerAttack;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerAttack == null)
        {
            _playerAttack = animator.GetComponent<PlayerAttack>();
        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(3, 0f);
        _playerAttack.IsAttacking = false;
    }
}
