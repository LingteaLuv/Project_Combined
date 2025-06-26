using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Layer Settings")]
    [SerializeField] private int targetLayerIndex = 1; // 조절할 레이어 인덱스
    [SerializeField] private float targetWeight = 1.0f; // 목표 가중치 (0~1)
    [SerializeField] private float weightChangeSpeed = 2.0f; // 가중치 변경 속도

    private Coroutine _currentAttackCoroutine; // 현재 실행 중인 공격 코루틴
    private void Awake()
    {
        _left_Hand_target = GameObject.Find("Hand_L");
        _right_Hand_target = GameObject.Find("Hand_R");
    }
    private void Start()
    {
        UpdateWeapon();
    }

    private void UpdateWeapon()
    {
        //손에 어떤 무기가 있는지 검사해야한다.
        _currentWeapon = _right_Hand_target.GetComponentInChildren<WeaponBase>();
    }

    void Update()
    {
        //계속 감지해야하는데 플레이어가 사용할 무기가 Instantiate 되엇을 때 이벤트 호출할 때 감지하면 좋을 것 같다.
        //_currentWeapon = _right_Hand_target.GetComponentInChildren<WeaponBase>();

        //빈손 일 때랑에서무기제네릭 메서드, 무기에서 빈손, 무기에서 -> 무기 

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryAttack();
        }
        //저는 해당 스크립트에서 _currentWeapon을 제가 알아야합니다
        //손에 어떤 아이템이 있는지 알려야 하기 때문에 해당 값을 들고와야하는데
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
    }

    private void StartRangedAttack()
    {
        // 원거리 무기는 즉시 공격 (연사 가능)
        PlayerAttackStart();
    }

    private IEnumerator MeleeAttackSequence()
    {
        _canAttack = false;
        _isAttacking = true;

        Debug.Log("근접 공격 시작 - 선딜 시작");

        // 선딜 대기
        yield return new WaitForSeconds(_startAttackDelay);

        Debug.Log("선딜 완료 - 애니메이션 실행");

        // 공격 애니메이션 실행
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
            default:
                Debug.Log($"알 수 없는 무기 타입: {_currentWeapon.ItemType}");
                break;
        }
    }

    public void PlayerAttackStart()
    {
        _currentWeapon.Attack();
    }


    /*public Vector3 SetAimRotation()
    {
        Vector2 mouseDir = GetMouseDirection();
    }*/

    /*private Vector2 GetMouseDirection()
    {
        float mouseX = Input.GetAxis("Mouse X") + _mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") + _mouseSensitivity;

        return new Vector2(mouseX, mouseY);
    }*/

    [SerializeField] private Transform _gunAlwaysPos;
    [SerializeField] private Transform _gunEquipPos;
    [SerializeField] private float _rotationSpeed = 5.0f;
    //[SerializeField] private WeaponBase _currentWeapon;

    private void WeaponSetActive()
    {
        if (_currentWeapon == null) return;
        _currentWeapon.gameObject.SetActive(true);
    }

    private void WeaponDeactivate()
    {
        if (_currentWeapon == null) return;
        _currentWeapon.gameObject.SetActive(false);
    }


    //총이 한번에 확돌아가는 문제가 존재
    private void SetGunEquippPos()
    {
        if (_currentWeapon.ItemType != ItemType.Gun) return;

        _currentWeapon.transform.rotation = _gunEquipPos.rotation;
        //StartCoroutine(GunLerpRotation());
    }

    private void SetGunAlwaysPos()
    {
        if (_currentWeapon.ItemType != ItemType.Gun) return;

        _currentWeapon.transform.rotation = _gunAlwaysPos.rotation;
    }

    //유기
    /*private IEnumerator GunLerpRotation()
    {
        while (Quaternion.Angle(_currentWeapon.transform.rotation, _gunAlwaysPos.rotation) > 0.1f)
        {
            _currentWeapon.transform.rotation = Quaternion.Slerp(_currentWeapon.transform.rotation,
                _gunAlwaysPos.rotation, Time.deltaTime * _rotationSpeed);
        }
        yield return null;
        _currentWeapon.transform.rotation = _gunAlwaysPos.rotation;
    }*/

    //무기에서 무기로 호출함수
    public void WeaponToWeapon()
    {
        _currentWeapon.gameObject.SetActive(false); //기존의 무기를 비활성화 처리하고
        _currentWeapon = null;                      //현재 무기를 null로 변경하고;
        //여기서 퀵슬롯이 바껴야함
        UpdateWeapon();                             //현재 무기를 무엇을 들고있는지 다시 검사
        _animator.SetTrigger("Equip");              //다시 해당 장비를 장착하는 애니메이션 재생                         
    }

    //맨손에서 무기로 호출함수
    public void BarehandsToWeapon()
    {
        //어차피 맨손이라서 무기가 없겠지만 혹시나 모르니
        if (_currentWeapon != null) return; //null이 아니면 return하고
        UpdateWeapon();                     //현재 무기를 무엇인지 업데이트 하고
        _animator.SetTrigger("Equip");      //해당무기에 맞는 장비를 장착하는 애니메이션 재생
    }

    //무기에서 빈손
    public void WeaponToBarehands()
    {
        //무기가 있어야하므로
        if (_currentWeapon == null) return; //null이면 return하고
        _currentWeapon = null;              //현재 무기를 무엇인지 업데이트 하고
        _animator.SetTrigger("UnEquip");
    }
    private void StartMeleeAttack()
    {
        // 이전 공격 코루틴이 있다면 정지
        if (_currentAttackCoroutine != null)
        {
            StopCoroutine(_currentAttackCoroutine);
        }

        _currentAttackCoroutine = StartCoroutine(MeleeAttackSequence());
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
}