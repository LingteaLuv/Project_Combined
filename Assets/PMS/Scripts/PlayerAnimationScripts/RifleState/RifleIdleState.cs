using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleIdleState : StateMachineBehaviour
{
    [SerializeField] private PlayerAttack _playerAttack;
    private bool flag = true;

    //총을 들고 있으므로 Gun Idle상태가 되어야한다.
    //만약 playerAttack이 null이면 GetComponent로 들고오자.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerAttack == null)
        {
            _playerAttack = animator.gameObject.GetComponent<PlayerAttack>();
        }
    }

    //Idle상태가 유지될 때 
    //마우스 우클릭을 통해 조준상태 돌입
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetMouseButtonDown(1) && !UIManager.Instance.IsUIOpened.Value &&flag)
        {
            _playerAttack.ToggleAimMode();
            animator.SetBool("IsAim", true);
            _playerAttack.IsAttacking = true;
            flag = false;
        }
    }

    //조준상태 일때 부터는 IsAttacking이 true라서 다른 행동 제약을 줄 수 있음(ex)장비 장착 해제)
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flag = true;
    }
}
