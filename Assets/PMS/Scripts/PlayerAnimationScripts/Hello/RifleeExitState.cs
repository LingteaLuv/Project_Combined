using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleeExitState : StateMachineBehaviour
{
    [SerializeField] private PlayerAttack _playerAttack;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerAttack == null)
        {
            _playerAttack = animator.gameObject.GetComponent<PlayerAttack>();
        }
        _playerAttack._rifle._lineRenderer.enabled = false;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack.IsAttacking = false;
    }
}
