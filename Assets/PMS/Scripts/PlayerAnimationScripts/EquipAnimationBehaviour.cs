using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipAnimationBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Weapon Equip"), 1f);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Weapon Equip"), 0f);
    }
}
