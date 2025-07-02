using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightControlStateBehaviour : StateMachineBehaviour
{
    [SerializeField] private PlayerAttack _playerAttack;   
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(layerIndex, 1f);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetMouseButtonDown(1))
        {
            _playerAttack.IsAttacking = true;
            animator.SetBool("IsAim", true);
        }
    }
}
