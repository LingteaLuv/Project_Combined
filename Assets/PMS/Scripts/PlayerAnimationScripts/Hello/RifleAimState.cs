using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleAimState : StateMachineBehaviour
{
    [SerializeField] private PlayerAttack _playerAttack;
    private bool flag = true;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flag = true;
        if (_playerAttack == null)
        {
            _playerAttack = animator.gameObject.GetComponent<PlayerAttack>();
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        _playerAttack.UpdateAim();

        if (Input.GetMouseButtonDown(0))
            _playerAttack.RightCurrentWeapon.Attack();

        if (Input.GetMouseButton(1) && flag)
        {
            _playerAttack.ToggleAimMode();
            animator.SetBool("IsAim", false);
            flag = false;
        }
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerAttack._rifle != null)
        {
            _playerAttack._rifle.isAiming = false;
        }
    }
}
