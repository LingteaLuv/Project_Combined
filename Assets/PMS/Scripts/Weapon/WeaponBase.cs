using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//최상위 부모 - 공통된 속성은 여기서 처리
public abstract class WeaponBase : MonoBehaviour
{
    //아이템 클래스를 가져온다 -> 나중에 인벤토리에 있는 데이터를 읽고 쓰기 위해서
    public Item _item;
    private ItemType _itemType;
    //플레이어 손위치에 소환도되야하는 위치 local위치를 변경해야 할 것 같음 -> 나중에 고민해보자
    [SerializeField] protected GameObject _weaponSpawnPos;   

    public ItemType ItemType { get { return _itemType; } protected set { _itemType = value; } }
    //튜토리얼에 Looting으로 나온 무기들은 Awake.Start,Update 주의 해야한다. 
    [SerializeField] protected bool _fixedWeapon = false;

    // ========== 이벤트 시스템 관련 ==========
    protected bool _canAttack = true; // 공격 가능 상태 저장

    // 컴포넌트가 활성화될 때 이벤트 구독
    protected virtual void OnEnable()
    {
        // PlayerAttack의 이벤트들에 구독
        PlayerAttack.OnAttackStateChanged += HandleAttackStateChanged;
    }

    // 컴포넌트가 비활성화될 때 이벤트 구독 해제 (메모리 누수 방지)
    protected virtual void OnDisable()
    {
        PlayerAttack.OnAttackStateChanged -= HandleAttackStateChanged;
    }

    // 공격 상태 변경 이벤트 핸들러
    private void HandleAttackStateChanged(bool canAttack)
    {
        _canAttack = canAttack;
        OnAttackStateChanged(canAttack);
    }

    //공격 상태 변경 시점
    // 하위 클래스에서 오버라이드할 수 있는 가상 메서드들
    protected virtual void OnAttackStateChanged(bool canAttack)
    {
        // 필요한 경우 하위 클래스에서 구현
        //Debug.Log($"{gameObject.name}: 공격 가능 상태 변경 - {canAttack}");
    }

    //각 무기에서 초기화 시 더필요한 부분 override하고 base.Init()
    public virtual void Init()
    {
        //fixedWeapon이 false가 아니면 고정된 오브젝트 X
        if (!_fixedWeapon)
        {
            if (_weaponSpawnPos == null)
            {
                Debug.Log("무기의 스폰포인트가 지정되어 있지 않습니다.");
                return;
            }
            else
            {
                gameObject.transform.localPosition = _weaponSpawnPos.transform.localPosition;
                gameObject.transform.localRotation = _weaponSpawnPos.transform.localRotation;
            }
        }
    } 

    // Attack 메서드 수정 - 공격 가능 상태 체크 추가
    public virtual void Attack()
    {
        // 공격 불가능한 상태면 공격하지 않음
        if (!_canAttack)
        {
            Debug.Log($"{gameObject.name}: 현재 공격할 수 없습니다.");
            return;
        }

        // 실제 공격 로직 실행
        ExecuteAttack();
    }

    // 하위 클래스에서 구현할 실제 공격 로직
    protected abstract void ExecuteAttack();
}

