using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerProperty : MonoBehaviour, IParameterHandler, IConsumeHandler, IDamageable
{
    [SerializeField] public PlayerInfo playerInfoSO;

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

    [SerializeField] private float _baseAtkSpeed;
    [SerializeField] private float _baseAtkDamage;

    // Todo : 외부(강한 행동 메서드)에서 호출하여 행동 제약 조건(PlayerController)에 대입
    public bool IsOnStaminaPenalty { get; private set; }
    public bool IsDied { get; private set; }
    public bool IsInvincible { get; private set; }

    private float _eatTimer;
    private float _drinkTimer;
    private float _staminaTimer;

    private const float MaxEatTimer = 60f;
    private const float MaxDrinkTimer = 60f;
    private const float MaxStaminaTimer = 5f;

    private WaitForSeconds _delay;

    private bool _isOnCorHunger;
    private bool _isOnCorThirsty;
    private bool _isOnCorStamina;
    private bool _isOnCorRecoverHp;
    private bool _isOnCorDecreaseHp;

    private bool _isOnLack;
    private bool _isOnDepletion;

    // SO 연동 추가 부분
    private float _staminaRegen;
    private float _hPRegen;
    private float _hungerDecrease;
    private float _moistureDecrease;
    private float _depletionHp;

    public float CrouchSpeed { get; private set; }
    public float RunSpeed { get; private set; }
    public float StaminaCostRun { get; private set; }
    public float StaminaCostJump { get; private set; }


    public float RunNoise { get; private set; }
    public float CrouchNoise { get; private set; }
    public float MoveNoise { get; private set; }
    public float SafeFallDistance { get; private set; }
    public float DeadFallDistance { get; private set; }
    public float FallDamage { get; private set; }

    public float StaminaCostMelee { get; private set; }

    private float _moistureBuffThreshold;
    private float _moistureBuffMoveSpeed;
    private float _moistureDebuffThreshold;
    private float _moistureDebuffMoveSpeed;
    private float _hungerBuffThreshold;
    private float _hungerBuffAtkSpeed;
    private float _hungerDebuffThreshold;
    private float _hungerDebuffAtkSpeed;

    //사운드 관련
    private float _hungerAndMoisture;
    private float _atkSFXCooldown;
    private float _hitSFXCooldown;


    private AudioClip _atkSFX;
    private AudioClip _jumpSFX;
    private AudioClip _hitSFX;
    private AudioClip _runSFX;
    private AudioClip _staminaDepletionSFX;
    private AudioClip _destroyEquipmentSFX;

    // 사망 처리용 이벤트 추가
    public event Action OnDied;
    // 피격 처리용 이벤트 추가
    public event Action OnHited;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        FieldUpdate();
        ParameterUpdate();
        ParameterAct();
    }

    private void OnTriggerEnter() { }

    private void OnTriggerExit() { }

    private void ParameterAct()
    {
        //Hp.Act(ref _atkDamage, _baseAtkDamage);
        AtkDamage.Value = _atkDamage;
        Hunger.Act(ref _atkSpeed, _baseAtkSpeed, playerInfoSO.HungerBuffAtkSpeed, playerInfoSO.HungerDebuffAtkSpeed);
        if (AtkSpeed.Value != _atkSpeed)
        {
            AtkSpeed.Value = _atkSpeed;
        }
        Thirsty.Act(ref _moveSpeed, playerInfoSO.MoveSpeed, playerInfoSO.MoistureBuffMoveSpeed, playerInfoSO.MoistureDebuffMoveSpeed);
        MoveSpeed.Value = _moveSpeed;
        Stamina.Act();
        IsOnStaminaPenalty = Stamina.IsOnPenalty;
    }

    private void FieldUpdate()
    {
        _isOnLack = (Hunger.State == ParamState.Lack) ||
                    (Thirsty.State == ParamState.Lack) ||
                    (Stamina.State == ParamState.Lack);

        _isOnDepletion = (Hunger.State == ParamState.Depletion) ||
                          (Thirsty.State == ParamState.Depletion);


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
            Hp.Recover(_hPRegen);
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
            Hp.Decrease(_depletionHp);
            IsPcDied();
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
            Hunger.Decrease(_hungerDecrease);
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
            Thirsty.Decrease(_moistureDecrease);
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
            Stamina.Recover(_staminaRegen);
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
    public void StaminaConsume(float amount)
    {
        Stamina.Decrease(amount);
        _staminaTimer = 2f;
    }


    private void Init()
    {
        Hp = new Hp(playerInfoSO.MaxHP);
        Stamina = new Stamina(playerInfoSO.MaxStamaina);
        //  Max Hunger 없음
        Hunger = new Hunger(100);
        Thirsty = new Thirsty(playerInfoSO.MaxMoisture);

        _delay = new WaitForSeconds(1f);
        _eatTimer = MaxEatTimer;
        _drinkTimer = MaxDrinkTimer;
        _staminaTimer = 0f;

        MoveSpeed = new Property<float>(playerInfoSO.MoveSpeed);
        AtkSpeed = new Property<float>(_baseAtkSpeed);
        AtkDamage = new Property<float>(_baseAtkDamage);

        // hp 관련
        _depletionHp = playerInfoSO.DepletionHp;
        _hungerDecrease = playerInfoSO.HungerDecrease;
        _hPRegen = playerInfoSO.HPRegen;
        // stamina 관련
        _staminaRegen = playerInfoSO.StaminaRegen;
        StaminaCostMelee = playerInfoSO.StaminaCostMelee;
        StaminaCostRun = playerInfoSO.StaminaCostRun;
        StaminaCostJump = playerInfoSO.StaminaCostJump;
        // 낙하 관련
        SafeFallDistance = playerInfoSO.SafeFallDistance;
        DeadFallDistance = playerInfoSO.DeadFallDistance;
        FallDamage = playerInfoSO.FallDamage;
        //  이동관련
        RunSpeed = playerInfoSO.RunSpeed;
        RunNoise = playerInfoSO.RunNoise;
        CrouchSpeed = playerInfoSO.CrouchSpeed;
        CrouchNoise = playerInfoSO.CrouchNoise;
        MoveNoise = playerInfoSO.MoveNoise;

        // 수분, 만복 관련
        _moistureDecrease = playerInfoSO.MoistureDecrease;

        _moistureBuffThreshold = playerInfoSO.MoistureBuffThreshold;
        _moistureBuffMoveSpeed = playerInfoSO.MoistureBuffMoveSpeed;
        _moistureDebuffThreshold = playerInfoSO.MoistureDebuffThreshold;
        _moistureDebuffMoveSpeed = playerInfoSO.MoistureDebuffMoveSpeed;
        _hungerBuffThreshold = playerInfoSO.HungerBuffThreshold;
        _hungerBuffAtkSpeed = playerInfoSO.HungerBuffAtkSpeed;
        _hungerDebuffThreshold = playerInfoSO.HungerDebuffThreshold;
        _hungerDebuffAtkSpeed = playerInfoSO.HungerDebuffAtkSpeed;

        //사운드 관련
        _hungerAndMoisture = playerInfoSO.HungerAndMoisture;
        _atkSFXCooldown = playerInfoSO.AtkSFXCooldown;
        _hitSFXCooldown = playerInfoSO.HitSFXCooldown;


        _runSFX = playerInfoSO.RunSFX;
        _atkSFX = playerInfoSO.AtkSFX;
        _jumpSFX = playerInfoSO.JumpSFX;
        _hitSFX = playerInfoSO.HitSFX;
        _staminaDepletionSFX = playerInfoSO.StaminaDepletionSFX;
        _destroyEquipmentSFX = playerInfoSO.DestroyEquipmentSFX;


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
    //아이템 효과를 플레이어에게 적용시키는 함수를 부탁드립니다.
    //ItemConsumeManage 클래스에서 사용될 것 같습니다.  - 김문성
    public void Consume(ItemBase item)
    {
        ConsumableItem consumableItem = item as ConsumableItem;
        Hp.Recover(consumableItem.HpAmount);
        if (consumableItem.HungerAmount != 0)
        {
            Hunger.Recover(consumableItem.HungerAmount);
            _eatTimer = MaxEatTimer;
        }

        if (consumableItem.MoistureAmount != 0)
        {
            Thirsty.Recover(consumableItem.MoistureAmount);
            _drinkTimer = MaxDrinkTimer;
        }

        Stamina.Recover(consumableItem.StaminaAmount);
    }

    public void ApplyFallDamage(float distance)
    {
        float damagePerMeter = 10.0f;
        int damage = 0;

        if (distance > DeadFallDistance)
            damage = 100;
        else if (distance > SafeFallDistance)
        {
            damage = Mathf.RoundToInt((distance - SafeFallDistance) * damagePerMeter);
            if (damage < 30)
                damage = 30;
        }
        if (damage > 0)
        {
            Hp.Decrease(damage);
            IsPcDied();
        }
    }
    public IEnumerator HitInWater(float damage)
    {
        while (true)
        {
            yield return _delay;
            Hp.Decrease(damage);
            IsPcDied();
        }
    }
    private void IsPcDied()
    {
        if (Hp.Value <= 0)
        {
            IsDied = true;
            OnDied?.Invoke();
        }
    }

    public void Damaged(float hitDamage)
    {
        // 이미 죽었다면 처리 할 필요가 없음
        if (IsDied || IsInvincible) return;

        // 루팅중이라면 아이템 파밍 초기화
        LootManager.Instance.CancelBlockHolding();

        //패턴 일치
        if (PlayerWeaponManager.Instance.LeftCurrentWeapon is IDefendable defendableWeapon 
            && PlayerWeaponManager.Instance.LeftCurrentWeapon != null)
        {
            //만약 방패가 있다면 해당 방패의 방어력 만큼 hitDamage감소
            hitDamage -= defendableWeapon.GetDefenseAmount();
            InventoryManager.Instance.DecreaseShieldDurability();
            
        }
        //방어력이 더크면 힐되므로 0보다 작으면 데미지 0처리
        hitDamage = Mathf.Max(0, hitDamage);

        // 데미지 적용
        Hp.Value -= hitDamage;

        StartCoroutine(InvincibilityCoroutine()); //무적시간 부여
        CamaraShaker.Instance.DamageShake();    // 피격시 카메라
        OnHited?.Invoke();
        IsPcDied();
    }

    //return Time -> 무적시간
    private IEnumerator InvincibilityCoroutine()
    {
        IsInvincible = true;

        yield return new WaitForSeconds(1.0f);

        IsInvincible = false;
    }
}
