using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipAnimationBehaviour : StateMachineBehaviour
{
    [SerializeField] private PlayerAttack _playerAttack;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(layerIndex, 1f);
        animator.SetLayerWeight(animator.GetLayerIndex("Upper Layer"), 0f);
        animator.SetTrigger("IsWeaponChange");
        if (animator.GetBool("IsGun"))
        {
            animator.SetBool("IsGun", false);
        }
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
                animator.SetLayerWeight(animator.GetLayerIndex("Upper Layer"), 1f);
            }
        }
    }
}
