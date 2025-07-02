using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleAimState : StateMachineBehaviour
{
    [SerializeField] private PlayerAttack _playerAttack;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerAttack == null)
        {
            _playerAttack = animator.gameObject.GetComponent<PlayerAttack>();
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
        _playerAttack.UpdateAim();

        if (Input.GetMouseButtonDown(0))
            _playerAttack.RightCurrentWeapon.Attack();

        if (Input.GetMouseButtonDown(1))
            _playerAttack.EndAim();
        */
    }
}
