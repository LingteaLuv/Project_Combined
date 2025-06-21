using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float _invincibilityTime = 0.5f;  //무적시간
    private bool _isInvincible = false; //무적 인지 아닌지

    [Header("Health Settings")]
    [SerializeField] private int _maxHp = 100; // 최대 hp 
    [SerializeField] private int _currentHp;   // 현재 hp

    [Header("Death Settings")]
    [SerializeField] private bool _isDead = false;
    [SerializeField] private float _deathSequenceTime = 3.0f;

    //외부에서 알려줄 이벤트들
    [Header("Events")]
    public UnityEvent OnPlayerDeath;    //플레이어가 주겅ㅆ는지
    public UnityEvent<int> OnHealthChanged; // 체력이 바뀌었는지? -> UI에서 연동하면 좋을 것 같아요
    public UnityEvent<int> OnMaxHealthChanged; // 최대 체력이 바뀌었는지?
    public UnityEvent<int> OnDamageReceived; // 데미지를 얼마나 받았는지 

    //임시로 쓰는 Start()함수입니다. 
    private void Start()
    {
        // 시작 시 체력을 최대체력으로 설정
        _currentHp = _maxHp;
        OnHealthChanged?.Invoke(_currentHp);
    }

    private void Die()
    {
        if (!_isDead) return;

        _isDead = true;

        // 사망 이벤트 발생
        OnPlayerDeath?.Invoke();

        // 사망 처리 해줘야하는데 죽는 애니메이션이 나와야하니깐 해당 값을 변수를 하나 설정해서 코루틴을 통해 해당 죽는 애니메이션를 재생할 여유를 주고 GameManager.GameOver처리를 해야한다.
        StartCoroutine(DeathSequence());
    }

    //외부에서 사용할 hp를 최대로 설정하는 함수  -> 현재 체력을 최대체력으로 
    //처음 초기 플레이어 hp셋팅, 레벨 업 했을 때 풀피 채워주는 용도로 사용? 
    public void SetHp()
    {
        if (_isDead) return;

        _currentHp = _maxHp;
    }

    //플레이어의 최대체력이 늘어 날 수 있으니깐 만들어 놨습니다. _maxHp 변수를 완전히 덮는 형식입니다!
    public void SetMaxHp(int newMaxHp)
    {
        if (_isDead) return;
        _maxHp = newMaxHp;

        OnMaxHealthChanged?.Invoke(_maxHp);     //최대체력변경 이벤트 Invoke
    }

    /// <summary>
    /// 외부에서 사용할 체력 회복 메서드
    /// </summary>
    /// <param name="healAmount"> 회복할 양이 담긴 정수 입니다 </param>
    public void Heal(int healAmount)
    {
        if (_isDead) return;

        _currentHp += healAmount;
        _currentHp = Mathf.Min(_maxHp, _currentHp); // 최대체력을 넘지 않도록

        OnHealthChanged?.Invoke(_currentHp);
    }

    /// <summary>
    /// IDamageable 구현함수
    /// </summary>
    /// <param name="hitDamage"> 입히고 싶은 데미지(정수) 입니다 </param>
    public void Damaged(int hitDamage)
    {
        // 이미 죽었다면 처리 할 필요가 없음
        if (_isDead || _isInvincible) return;

        // 데미지 적용
        _currentHp -= hitDamage;

        // 체력이 0 이하로 떨어지지 않도록 제한
        _currentHp = Mathf.Max(0, _currentHp);

        // 이벤트 발생
        OnDamageReceived?.Invoke(hitDamage);        //데미지를 받은 이벤트 - 나중에 플레이어 피격 효과 같은 이펙트 사용할 때 연결해주면 좋을 듯
        OnHealthChanged?.Invoke(_currentHp);        //체력 변경 이벤트 발생 알리기

        //플레이어 피격 애니메이션 재생

        // 체력이 0 이하가 되면 사망 처리
        if (_currentHp <= 0 && !_isDead)
        {
            Die();
            return;
        }

        StartCoroutine(InvincibilityCoroutine()); //사망하지 않았으면 무적시간 부여 
    }

    /// <summary>
    /// 플레이어 죽으면 잠깐 애니메이션 기다리고 게임 오버 처리하기 위해 만든 코루틴입니다.
    /// </summary>
    /// <param name="_deathSequenceTime"> 기다리는 시간 </param>
    /// <returns></returns>
    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(_deathSequenceTime);

        //플레이어 죽음 애니메이션 재생

        //게임 매니저 게임 오버 처리 함수
        GameManager.Instance.GameOver();

        Debug.Log("플레이어 사망");
    }

    //잠깐 디버깅 용으로 Debug.Log썼습니다.
    private IEnumerator InvincibilityCoroutine()
    {
        _isInvincible = true;
        Debug.Log("무적 시간 시작");

        yield return new WaitForSeconds(_invincibilityTime);

        _isInvincible = false;
        Debug.Log("무적 시간 종료");
    }
}
