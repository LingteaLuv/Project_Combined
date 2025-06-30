using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnEquipAnimationBehaviour : StateMachineBehaviour
{
    [SerializeField] private PlayerAttack _playerAttack;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(layerIndex, 1f);
        animator.SetLayerWeight(animator.GetLayerIndex("Upper Layer"), 0f);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(layerIndex, 0f);
        if (_playerAttack == null)
        {
            _playerAttack = animator.GetComponent<PlayerAttack>();
        }
        if (_playerAttack != null && _playerAttack.CurrentWeapon != null)
        {
            if (_playerAttack.CurrentWeapon.ItemType == ItemType.Gun)
            {
                animator.SetBool("IsGun", false);
                animator.SetLayerWeight(animator.GetLayerIndex("Upper Layer"), 0f);
            }
        }
    }
}
