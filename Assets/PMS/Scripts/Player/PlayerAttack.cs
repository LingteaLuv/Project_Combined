using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool _isAttacking = false;
    /// <summary>
    /// 플레이어 공격 상태 - 이벤트를 통해 무기들에게 알림
    /// </summary>
    public bool IsAttacking //공격중일 때 true, 공격중이 아닐 때 false
    {
        get => _isAttacking;
        set
        {
            if (_isAttacking != value)
            {
                _isAttacking = value;
                // 공격 상태가 변경될 때마다 이벤트 발생
                OnAttackStateChanged?.Invoke(!_isAttacking); // CanAttack 상태 전달

                if (_isAttacking)
                    OnAttackStart?.Invoke();
                else
                    OnAttackEnd?.Invoke();
            }
        }
    }

    // ========== 이벤트 시스템 추가 ==========
    // 공격 상태 변경 이벤트 (무기들이 구독할 이벤트) <- 이친구가 IsAttacking의 값을 바꿔줍니다.
    public static event Action<bool> OnAttackStateChanged;

    // 공격 시작 할 때 이벤트
    public static event Action OnAttackStart;

    // 공격 종료 할 때 이벤트
    public static event Action OnAttackEnd;


    [SerializeField] Animator _animator; 
    [SerializeField] private WeaponBase _currentWeapon;
    public WeaponBase CurrentWeapon { get { return _currentWeapon; } }
    
    //소환되는 Transform 계층
    [SerializeField] public WeaponBase LeftCurrentWeapon;
    [SerializeField] public WeaponBase RightCurrentWeapon;

    private Rifle _rifle;//캐싱

    [Header("공격 애니메이션 클립")]
    [SerializeField] private AnimationClip _melee;

    /// <summary>
    /// 플레이어 공격 못하게 하고 싶을때 IsAttacking = true, 공격하게 하고 싶을 때 IsAttacking = false; 
    /// </summary>

    private PlayerProperty _playerProperty;
    private Coroutine _currentAttackCoroutine; // 현재 실행 중인 공격 코루틴

    //TODO - 나중에 어디서인가 그 현재 무기가 뭔지 있어야하는 부분이 있지 않을까? Action 연결
    // 플레이어의 왼쪽 오른쪽 들고있는 템이 뭔지 바뀌는 이벤트가 존재 할 때 나도 업데이트해서 사용할 수 있지 않을까?

    private void Awake()
    {
        //이벤트 구독 - 오른쪽 무기가 바뀔 때마다 알림받기
        PlayerWeaponManager.OnRightWeaponChanged += OnRightWeaponChanged;
        LeftCurrentWeapon = PlayerWeaponManager.Instance.LeftCurrentWeapon;
        RightCurrentWeapon = PlayerWeaponManager.Instance.RightCurrentWeapon;
        _playerProperty = GetComponent<PlayerProperty>();
    }

    //오른쪽 무기가 변경될 때 호출되는 콜백 함수
    private void OnRightWeaponChanged(WeaponBase newWeapon)
    {
        // 라이플 참조 업데이트
        if (newWeapon != null && newWeapon.ItemType == ItemType.Gun)
        {
            _rifle = newWeapon.GetComponent<Rifle>();
        }
        else
        {
            _rifle = null;
        }
    }

    void Update()
    {
        RightCurrentWeapon = PlayerWeaponManager.Instance.RightCurrentWeapon;
        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    private IEnumerator MeleeAttackSequence()
    {
        IsAttacking = true;

        Debug.Log("애니메이션 실행");
        _animator.SetTrigger("DownAttack");

        float playerAttackSpeed = _playerProperty.AtkSpeed.Value;

        if (playerAttackSpeed <= 0)
        {
            playerAttackSpeed = 1;
            Debug.Log("플레이어의 공격속도가 0보다 작기 때문에 공격 할 수 없습니다.");
            /*_isAttacking = false;
            _canAttack = true;
            _currentAttackCoroutine = null;
            yield break;*/
        }
        _animator.SetFloat("AttackSpeed", playerAttackSpeed);

        float currentTime = _melee.length;
        float coolTime = 1 / playerAttackSpeed;

        //디버깅용 코드
        Debug.Log($"애니메이션 기본시간 {currentTime}");
        Debug.Log($"애니메이션 실제재생시간 {(currentTime) / playerAttackSpeed}");
        Debug.Log($"실제 재생시간 + 공격 속도에 따른 쿨타임 {(currentTime) / playerAttackSpeed + coolTime}");
        yield return new WaitForSeconds((currentTime) / playerAttackSpeed + coolTime);

        Debug.Log("대기 끝");


        IsAttacking = false;
        _currentAttackCoroutine = null;
    }

    private void TryAttack()
    {
        if (IsAttacking) return; 

        if (RightCurrentWeapon == null)
        {
            Debug.Log("현재 손에 공격무기가 없습니다");
            return;
        }

        switch (RightCurrentWeapon.ItemType)
        {
            case ItemType.Melee:
                StartMeleeAttack();
                _playerProperty.StaminaConsume(3f);
                break;
            case ItemType.Gun:
                StartRangedAttack();
                break;
            case ItemType.Throw:
                StartThrowAttack();
                break;
            default:
                Debug.Log($"알 수 없는 무기 타입: {RightCurrentWeapon.ItemType}");
                break;
        }
    }

    //플레이어 공격 실제 로직 실행하는 함수
    //애니메이션 이벤트에서 호출 되는 함수 - Melee Attack,Throw Attack
    public void PlayerAttackStart()
    {
        RightCurrentWeapon.Attack();
    }  

    //빠따공격 실행
    private void StartMeleeAttack()
    {
        // 이전 공격 코루틴이 있다면 정지
        if (_currentAttackCoroutine != null)
        {
            StopCoroutine(_currentAttackCoroutine);
            _currentAttackCoroutine = null;
        }

        _currentAttackCoroutine = StartCoroutine(MeleeAttackSequence());
    }

    //원거리 공격 실행
    private void StartRangedAttack()
    {
        _animator.SetBool("IsAim",true);
    }

    //레이어를 일단은 혼자 쓰는 것 같은데 계속 플레이어의 animtor가 수정될 일이 많은데
    //AnimatorUtil 유틸 클래스로 SetLayerWeight함수를
    //특정 레이어의 이름으로 찾아보도록 하는 함수를 만들어보자 -> 추후 -> StateBehaviour사용

    private void StartThrowAttack()
    {
        _animator.SetTrigger("Throw");
        _animator.SetLayerWeight(3, 1); //Throw Layer 
    }

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

    private void WeaponLogicStopAndAnimationStop()
    {
        if (CurrentWeapon.ItemType != ItemType.Throw) return;

        _animator.speed = 0f;
        StartThrowAttack();
    }
    private IEnumerator WaifForInput()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
    }

    public void StartAim()
    {
        if (_rifle == null) return;
        
        _rifle.StartAim();
    }

    public void UpdateAim()
    {
        if (_rifle == null) return;

        _rifle.UpdateAim();
    }

    public void EndAim()
    {
        if(_rifle == null) return;

        _rifle.EndAim();
    }
}