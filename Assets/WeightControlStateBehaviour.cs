using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightControlStateBehaviour : StateMachineBehaviour
{
     
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(layerIndex, 1f);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(layerIndex, 0f);
    }
}
