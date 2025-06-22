using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerProperty : MonoBehaviour
{
    [SerializeField] private Hp _hp;
    [SerializeField] private Hunger _hunger;
    [SerializeField] private Thirsty _thirsty;
    [SerializeField] private Stamina _stamina;

    public Property<float> MoveSpeed;
    public Property<float> AtkSpeed;
    public Property<float> AtkDamage;

    private float _moveSpeed;
    private float _atkSpeed;
    private float _atkDamage;

    [SerializeField] private float _baseMoveSpeed;
    [SerializeField] private float _baseAtkSpeed;
    [SerializeField] private float _baseAtkDamage;
    
    [SerializeField] private float _moveSpeedOffset;
    [SerializeField] private float _atkSpeedOffset;
    [SerializeField] private float _atkDamageOffset;

    // Todo : 외부(강한 행동 메서드)에서 호출하여 행동 제약 조건(PlayerController)에 대입
    public bool IsOnStaminaPenalty { get; private set; }

    private float _eatTimer;
    private float _drinkTimer;
    private float _staminaTimer;

    private const float MaxEatTimer = 300f;
    private const float MaxDrinkTimer = 300f;
    private const float MaxStaminaTimer = 2f;
    
    private WaitForSeconds _delay;

    private bool _isOnCorHunger;
    private bool _isOnCorThirsty;
    private bool _isOnCorStamina;
    private bool _isOnCorRecoverHp;
    private bool _isOnCorDecreaseHp;

    private bool _isOnLack;
    private bool _isOnDepletion;
    
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        FieldUpdate();
        ParameterUpdate();
        ParameterAct();
    }

    private void ParameterAct()
    {
        _hp.Act(ref _atkDamage, _baseAtkDamage, _atkDamageOffset);
        AtkDamage.Value = _atkDamage;
        _hunger.Act(ref _atkSpeed, _baseAtkSpeed, _atkSpeedOffset);
        AtkSpeed.Value = _atkSpeed;
        _thirsty.Act(ref _moveSpeed, _baseMoveSpeed, _moveSpeedOffset);
        MoveSpeed.Value = _moveSpeed;
        _stamina.Act();
        IsOnStaminaPenalty = _stamina.IsOnPenalty;
    }
    
    private void FieldUpdate()
    {
        _isOnLack = (_hunger.State == ParamState.Lack) || 
                    (_thirsty.State == ParamState.Lack) ||
                    (_stamina.State == ParamState.Lack);
        
        _isOnDepletion =  (_hunger.State == ParamState.Depletion) || 
                          (_thirsty.State == ParamState.Depletion) ||
                          (_stamina.State == ParamState.Depletion);
        
        if (_eatTimer > 0) _eatTimer -= Time.deltaTime;
        if (_drinkTimer > 0) _drinkTimer -= Time.deltaTime;
    }
    
    private void ParameterUpdate()
    {
        if (_eatTimer <= 0 && !_isOnCorHunger)
        {
            StartCoroutine(DecreaseHunger());
        }
        
        if (_drinkTimer <= 0 && !_isOnCorThirsty)
        {
            StartCoroutine(DecreaseThirsty());
        }

        if (!_isOnLack && !_isOnCorRecoverHp)
        {
            StartCoroutine(RecoverHp());
        }

        if (_isOnDepletion && !_isOnCorDecreaseHp)
        {
            StartCoroutine(DecreaseHp());
        }
        
        if (_staminaTimer <= 0 && !_isOnCorStamina)
        {
            StartCoroutine(RecoverStamina());
        }
    }

    private IEnumerator RecoverHp()
    {
        _isOnCorRecoverHp = true;
        while (true)
        {
            if (_isOnLack) break;
            _hp.Recover(1f);
            yield return _delay;
        }
        _isOnCorRecoverHp = false;
    }
    
    private IEnumerator DecreaseHp()
    {
        _isOnCorDecreaseHp = true;
        while (true)
        {
            if (!_isOnDepletion) break;
            _hp.Decrease(1f);
            yield return _delay;
        }
        _isOnCorDecreaseHp = false;
    }
    
    private IEnumerator DecreaseHunger()
    {
        _isOnCorHunger = true;
        while (true)
        {
            if (_eatTimer > 0) break;
            _hunger.Decrease(1f);
            yield return _delay;
        }
        _isOnCorHunger = false;
    }
    
    private IEnumerator DecreaseThirsty()
    {
        _isOnCorThirsty = true;
        while (true)
        {
            if (_drinkTimer > 0) break;
            _thirsty.Decrease(1f);
            yield return _delay;
        }
        _isOnCorThirsty = false;
    }
    
    private IEnumerator RecoverStamina()
    {
        _isOnCorStamina = true;
        while (true)
        {
            if (_staminaTimer > 0) break;
            _stamina.Recover(5f);
            yield return _delay;
        }
        _isOnCorStamina = false;
    }
    
    // 외부에서 음식을 먹을 경우 호출하는 메서드
    /*public void Eat(Item item)
    {
        // todo : 아이템의 수치 적용, Timer 시간 초기화(5분)
        // _hunger.Recover(item.value);
        _eatTimer = MaxEatTimer;
    }
    
    // 외부에서 물을 마실 경우 호출하는 메서드
    public void Drink(Item item)
    {
        // todo : 아이템의 수치 적용, Timer 시간 초기화(5분)
        // _thirsty.Recover(item.value);
        _drinkTimer = MaxDrinkTimer;
    }
    */

    // 강한 행동에서 호출되는 이벤트에 구독되는 메서드
    public void ExpendAction()
    {
        _stamina.Decrease(15f);
        _staminaTimer = 2f;
    }
    
    private void Init()
    {
        _hp.Init(100);
        _hunger.Init(100);
        _thirsty.Init(100);
        _stamina.Init(100);

        _delay = new WaitForSeconds(1f);

        _eatTimer = MaxEatTimer;
        _drinkTimer = MaxDrinkTimer;
        _staminaTimer = 0f;

        AtkSpeed = new Property<float>(_baseAtkSpeed);
        MoveSpeed = new Property<float>(_baseMoveSpeed);
        AtkDamage = new Property<float>(_baseAtkDamage);
    }
}
