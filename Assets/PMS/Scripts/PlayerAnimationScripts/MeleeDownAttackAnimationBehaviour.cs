using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDownAttackAnimationBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Melee Upper Layer"), 1f);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Melee Upper Layer"), 0f);
    }
}
