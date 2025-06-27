using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerWeightResetBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 레이어 weight를 부드럽게 0으로
        AnimatorUtil.SetLayerWeightSmooth(animator.GetComponent<MonoBehaviour>(), animator, 4, 0f, 0.5f);
    }
}
