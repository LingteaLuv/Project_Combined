using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerWeightResetBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Upper Layer"), 1f);
    }
}
