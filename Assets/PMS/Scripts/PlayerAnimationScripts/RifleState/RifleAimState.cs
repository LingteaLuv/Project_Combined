using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleAimState : StateMachineBehaviour
{
    [SerializeField] private GameObject _backPack;
    private SkinnedMeshRenderer m_SkinnedMeshRenderer;
    [SerializeField] private PlayerAttack _playerAttack;
    private Material _prevPlayerMt;
    private Material _playerMt;
    private Color _prevColor;
    private Color _Color;
    private float _alpha = 0.5f;
    private float _alpha1 = 1.0f;
    private bool flag = true;                               //한번만 사용할 flag
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flag = true;
        if (_playerAttack == null)
        {
            _playerAttack = animator.gameObject.GetComponent<PlayerAttack>();
            m_SkinnedMeshRenderer = animator.transform.GetComponentInChildren<SkinnedMeshRenderer>();
            _playerMt = m_SkinnedMeshRenderer.material;
            _prevPlayerMt = _playerMt;
            _prevColor = _playerMt.color;
        }
        _Color= _playerMt.color;
        _Color.a = _alpha;
        _playerMt.color = _Color;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //flag가 되기 전까지 플레이어 Gun 궤적표시 해주도록 함
        if (flag != false) _playerAttack.UpdateAim();

        //만약 왼쪽키를 누를 때 공격 할 수 있다.
        if (Input.GetMouseButtonDown(0) && !UIManager.Instance.IsUIOpened.Value)
            _playerAttack.RightCurrentWeapon.Attack();

        //만약 우클릭을 하면 flag값이 false가 되어 궤적보이는 효과를 없애고 조준모드를 해제
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
        _Color.a = _alpha1;
        _Color = _prevColor;
        _playerMt.color = _Color;
    }
}
