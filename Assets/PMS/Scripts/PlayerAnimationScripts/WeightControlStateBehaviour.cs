using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightControlStateBehaviour : StateMachineBehaviour
{
    [SerializeField] private PlayerAttack _playerAttack;   
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(layerIndex, 1f);
        if(_playerAttack == null)
        {
            _playerAttack = animator.gameObject.GetComponent<PlayerAttack>();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack.IsAttacking = true;
    }
}
