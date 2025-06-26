using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Animator _animator; //플레이어의 애니메이터가 필요 -> 공격시 플레이어 애니메이션 재생하기 위하여
    //[SerializeField][Range(0, 5)] private float _mouseSensitivity = 1;
    //추후에 플레이어가 어떤 키를 입력했는지 불러올것같아서
    //[SerializeField] private PlayerInputManager _playerInput;

    [SerializeField] private WeaponBase _currentWeapon;
    //소환되는위치
    [SerializeField] private GameObject _left_Hand_target;
    [SerializeField] private GameObject _right_Hand_target;

    [SerializeField][Range(0, 2)] private float _startAttackDelay;      //플레이어 근접공격모션 시작 딜레이
    [SerializeField][Range(0, 2)] private float _endAttackDelay;  //플레이어 근접공격모션 종료 딜레이

    [SerializeField] private bool _canAttack = true;
    [SerializeField] private bool _isAttacking = false; // 공격 중인지 체크

    [Header("Player Attack AnimatorLayer Settings")]
    [SerializeField] private int targetLayerIndex = 1; // 조절할 레이어 인덱스
    [SerializeField] private float targetWeight = 1.0f; // 목표 가중치 (0~1)
    [SerializeField] private float weightChangeSpeed = 2.0f; // 가중치 변경 속도

    private Coroutine _currentAttackCoroutine; // 현재 실행 중인 공격 코루틴

    //TODO - 나중에 어디서인가 그 현재 무기가 뭔지 있어야하는 부분이 있지 않을까? Action 연결
    // 플레이어의 왼쪽 오른쪽 들고있는 템이 뭔지 바뀌는 이벤트가 존재 할 때 나도 업데이트해서 사용할 수 있지 않을까?

    private void Awake()
    {
        //모든 아이템은 해당 Hand_bone밑에 있다.
        _left_Hand_target = GameObject.Find("Hand_L");      
        _right_Hand_target = GameObject.Find("Hand_R");
    }
    private void Start()
    {
        UpdateWeapon();
    }

    //손에 어떤 무기가 있는지 검사해야한다.
    //계속 감지해야하는데 플레이어가 사용할 무기가 Instantiate 되엇을 때 or 퀵슬롯 변경을 통한 SetActive가 되었을 때
    //이벤트 같은 것을 사용해 메서드 등록하여 감지하면 좋을 것 같다.
    private void UpdateWeapon()
    {
        _currentWeapon = _right_Hand_target.GetComponentInChildren<WeaponBase>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryAttack();
        }

        // TODO - 플레이어의 장비 장착 해제는 나중에 다른 스크립트에서 관리해야하지 않을까?
        #region 추후 이동예정
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (_currentWeapon == null) return;
            _animator.SetTrigger("Equip");
        }
        //무기 해제
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_currentWeapon == null) return;
            _animator.SetTrigger("UnEquip");
        }
        #endregion
    }

    private IEnumerator MeleeAttackSequence()
    {
        _canAttack = false;
        _isAttacking = true;

        Debug.Log("근접 공격 시작 - 선딜 시작");

        // 선딜 대기
        yield return new WaitForSeconds(_startAttackDelay);

        Debug.Log("선딜 완료 - 애니메이션 실행");

        SetLayerWeight(2, 1);
        _animator.SetTrigger("DownAttack");

        // 실제 공격 실행 (애니메이션 이벤트 대신 여기서 실행)
        PlayerAttackStart();

        Debug.Log("공격 실행 - 후딜 시작");

        // 후딜 대기
        yield return new WaitForSeconds(_endAttackDelay);

        SetLayerWeight(2, 0);
        Debug.Log("후딜 완료 - 공격 가능");

        _isAttacking = false;
        _canAttack = true;
        _currentAttackCoroutine = null;
    }

    private void TryAttack()
    {
        // 공격 불가능한 상태면 리턴
        if (!_canAttack || _isAttacking)
        {
            Debug.Log("공격 불가능한 상태입니다.");
            return;
        }

        if (_currentWeapon == null)
        {
            Debug.Log("현재 손에 무기가 없습니다");
            return;
        }

        switch (_currentWeapon.ItemType)
        {
            case ItemType.Melee:
                StartMeleeAttack();
                break;
            case ItemType.Gun:
                StartRangedAttack();
                break;
            case ItemType.Throw:
                StartThrowAttack();
                break;
            default:
                Debug.Log($"알 수 없는 무기 타입: {_currentWeapon.ItemType}");
                break;
        }
    }

    public void PlayerAttackStart()
    {
        _currentWeapon.Attack();
    }  

    //빠따공격
    private void StartMeleeAttack()
    {
        // 이전 공격 코루틴이 있다면 정지
        if (_currentAttackCoroutine != null)
        {
            StopCoroutine(_currentAttackCoroutine);
        }

        _currentAttackCoroutine = StartCoroutine(MeleeAttackSequence());
    }

    private void StartRangedAttack()
    {
        // 원거리 무기는 즉시 공격  애니메이션 나중에
        PlayerAttackStart();
    }

    //레이어를 일단은 혼자 쓰는 것 같은데 계속 플레이어의 animtor가 수정될 일이 많은데
    //AnimatorUtil 유틸 클래스로 SetLayerWeight함수를 특정 레이어의 이름으로 찾아보도록 하는 함수를 만들어보자.
    private void StartThrowAttack()
    {
        _animator.SetTrigger("Throw");
        _animator.SetLayerWeight(4, 1); //Throw Layer 
    }

    //원거리 공격

    /// <summary>
    /// 특정 레이어의 가중치를 즉시 설정
    /// </summary>
    /// <param name="layerIndex">레이어 인덱스 (0이 Base Layer)</param>
    /// <param name="weight">가중치 (0~1)</param>
    public void SetLayerWeight(int layerIndex, float weight)
    {
        if (layerIndex >= _animator.layerCount || layerIndex < 0)
        {
            Debug.LogError($"잘못된 레이어 인덱스: {layerIndex}. 총 레이어 수: {_animator.layerCount}");
            return;
        }

        // Base Layer (인덱스 0)의 가중치는 항상 1이므로 변경할 수 없음
        if (layerIndex == 0)
        {
            Debug.LogWarning("Base Layer의 가중치는 변경할 수 없습니다!");
            return;
        }

        weight = Mathf.Clamp01(weight); // 0~1 범위로 제한
        _animator.SetLayerWeight(layerIndex, weight);

        Debug.Log($"레이어 {layerIndex}의 가중치를 {weight}로 설정했습니다.");
    }
}