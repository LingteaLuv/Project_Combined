using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerProperty : MonoBehaviour
{
    private Hp _hp;
    private Hunger _hunger;
    private Thirsty _thirsty;
    private Stamina _stamina;

    public Property<float> MoveSpeed;
    public Property<float> AtkSpeed;
    public Property<float> AtkDamage;

    [SerializeField] private float _baseMoveSpeed;
    [SerializeField] private float _baseAtkSpeed;
    [SerializeField] private float _baseAtkDamage;
    
    private float _eatTimer;
    private float _drinkTimer;

    private const float MaxEatTimer = 300f;
    private const float MaxDrinkTimer = 300f;
    
    private WaitForSeconds _delay;

    private bool _isOnCorHunger;
    private bool _isOnCorThirsty;
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
        _isOnLack = (_hunger.State == ParamState.Lack) || 
                         (_thirsty.State == ParamState.Lack) ||
                         (_stamina.State == ParamState.Lack);
        
        _isOnDepletion =  (_hunger.State == ParamState.Depletion) || 
                          (_thirsty.State == ParamState.Depletion) ||
                          (_stamina.State == ParamState.Depletion);
        
        if (_eatTimer > 0) _eatTimer -= Time.deltaTime;
        
        if (_drinkTimer > 0) _drinkTimer -= Time.deltaTime;
        
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
    /*
    public void Eat(Item item)
    {
        // todo : 아이템의 수치 적용, Timer 시간 초기화(5분)
        // _hunger.Recover(item.value);
        _eatTimer = MaxEatTimer;
    }

    public void Drink(Item item)
    {
        // todo : 아이템의 수치 적용, Timer 시간 초기화(5분)
        // _thirsty.Recover(item.value);
        _drinkTimer = MaxDrinkTimer;
    }
    */
    private void Init()
    {
        _hp.Init(100);
        _hunger.Init(100);
        _thirsty.Init(100);
        _stamina.Init(100);

        _delay = new WaitForSeconds(5f);

        _eatTimer = MaxEatTimer;
        _drinkTimer = MaxDrinkTimer;

        AtkSpeed = new Property<float>(_baseAtkSpeed);
        MoveSpeed = new Property<float>(_baseMoveSpeed);
        AtkDamage = new Property<float>(_baseAtkDamage);
    }
}
