using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerProperty : MonoBehaviour, IParameterHandler
{
    public Hp Hp;
    public Hunger Hunger;
    public Thirsty Thirsty;
    public Stamina Stamina;
    
    public Property<float> MoveSpeed;
    public Property<float> AtkSpeed;
    public Property<float> AtkDamage;
    
    private List<StatModifier> _statMods;

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

    private const float MaxEatTimer = 3f;
    private const float MaxDrinkTimer = 3f;
    private const float MaxStaminaTimer = 2f;
    
    private WaitForSeconds _delay;

    private bool _isOnCorHunger;
    private bool _isOnCorThirsty;
    private bool _isOnCorStamina;
    private bool _isOnCorRecoverHp;
    private bool _isOnCorDecreaseHp;
    private bool _isWater;
    private bool _isOnLack;
    private bool _isOnDepletion;
    
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        FieldUpdate();
        ParameterUpdate();
        ParameterAct();

        if (Input.GetKeyDown(KeyCode.J))
        {
            ExpendAction();
        }
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            Hunger.Recover(20f);
            _eatTimer = MaxEatTimer;
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            Thirsty.Recover(30f);
            _drinkTimer = MaxDrinkTimer;
        }
    }

    private void ParameterAct()
    {
        Hp.Act(ref _atkDamage, _baseAtkDamage, _atkDamageOffset);
        AtkDamage.Value = _atkDamage;
        Hunger.Act(ref _atkSpeed, _baseAtkSpeed, _atkSpeedOffset);
        if(AtkSpeed.Value != _atkSpeed)
        {
            AtkSpeed.Value = _atkSpeed;
        }
        
        Thirsty.Act(ref _moveSpeed, _baseMoveSpeed, _moveSpeedOffset);
        MoveSpeed.Value = _moveSpeed;
        Stamina.Act();
        IsOnStaminaPenalty = Stamina.IsOnPenalty;
    }
    
    private void FieldUpdate()
    {
        _isOnLack = (Hunger.State == ParamState.Lack) || 
                    (Thirsty.State == ParamState.Lack) ||
                    (Stamina.State == ParamState.Lack);
        
        _isOnDepletion =  (Hunger.State == ParamState.Depletion) || 
                          (Thirsty.State == ParamState.Depletion) ||
                          (Stamina.State == ParamState.Depletion);
        
        
        if (_eatTimer > 0) _eatTimer -= Time.deltaTime;
        if (_drinkTimer > 0) _drinkTimer -= Time.deltaTime;
        if (_staminaTimer > 0) _staminaTimer -= Time.deltaTime;
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
            if (_isOnLack || _isOnDepletion) break;
            Hp.Recover(1f);
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
            Hp.Decrease(1f);
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
            Hunger.Decrease(1f);
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
            Thirsty.Decrease(1f);
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
            Stamina.Recover(5f);
            yield return _delay;
        }
        _isOnCorStamina = false;
    }
    
    // 외부에서 음식을 먹을 경우 호출하는 메서드
    /*public void Eat(Item item)
    {
        // todo : 아이템의 수치 적용, Timer 시간 초기화(5분)
        // Hunger.Recover(item.value);
        _eatTimer = MaxEatTimer;
    }
    
    // 외부에서 물을 마실 경우 호출하는 메서드
    public void Drink(Item item)
    {
        // todo : 아이템의 수치 적용, Timer 시간 초기화(5분)
        // Thirsty.Recover(item.value);
        _drinkTimer = MaxDrinkTimer;
    }
    */

    // 강한 행동에서 호출되는 이벤트에 구독되는 메서드
    public void ExpendAction()
    {
        Stamina.Decrease(15f);
        _staminaTimer = 2f;
    }
    
    private void Init()
    {
        Hp = new Hp(100);
        Hunger = new Hunger(100);
        Thirsty = new Thirsty(100);
        Stamina = new Stamina(100);

        _delay = new WaitForSeconds(1f);

        _eatTimer = MaxEatTimer;
        _drinkTimer = MaxDrinkTimer;
        _staminaTimer = 0f;

        AtkSpeed = new Property<float>(_baseAtkSpeed);
        MoveSpeed = new Property<float>(_baseMoveSpeed);
        AtkDamage = new Property<float>(_baseAtkDamage);
    }

    public void ApplyModifier(StatModifier modifier)
    {
        _statMods.Add(modifier);
        StatModify();
    }

    public void RemoveModifier(StatModifier modifier)
    {
        _statMods.Remove(modifier);
        StatModify();
    }
    
    private void StatModify()
    {
        foreach (var mod in _statMods)
        {
            MoveSpeed.Value += mod.MoveSpeedChange;
            AtkSpeed.Value += mod.AtkSpeedChange;
            AtkDamage.Value += mod.AtkDamageChange;
        }
    }
    //Todo : public void Consume(Itembase item){} 아이템 효과를 플레이어에게 적용시키는 함수를 부탁드립니다.
    //ItemConsumeManage 클래스에서 사용될 것 같습니다.  - 김문성
}
