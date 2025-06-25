using UnityEngine;

public class PlayerHealthEdit : MonoBehaviour
{
    [SerializeField] private int _maxHp = 100;
    private int _currentHp;
    private bool _isDead = false;
    private bool _isInvincible = false;

    public int CurrentHp => _currentHp;
    public bool IsDead => _isDead;
    public bool IsInvincible => _isInvincible;

    public void Init()
    {
        _currentHp = _maxHp;
        _isDead = false;
    }

    public void ApplyDamage(int damage)
    {
        if (_isDead || _isInvincible) return;
        _currentHp = Mathf.Max(_currentHp - damage, 0);
        Debug.Log($"[HP] 피격! 남은 체력: {_currentHp}");
    }
    public void ApplyDamageForced(int damage)
    {
        if (_isDead) return;
        _currentHp = Mathf.Max(_currentHp - damage, 0);
        Debug.Log($"[HP] 강제 데미지 적용! 남은 체력: {_currentHp}");
    }
    public bool ApplyDamageAndCheckDeath(int damage)
    {
        if (_isDead || _isInvincible) return false;

        _currentHp = Mathf.Max(_currentHp - damage, 0);
        Debug.Log($"[HP] 데미지 적용 후 체력: {_currentHp}");

        if (_currentHp <= 0)
        {
            _isDead = true;
            return true;
        }
        return false;
    }
    public void SetInvincible(bool value)
    {
        _isInvincible = value;
    }

    public void MarkDead()
    {
        _isDead = true;
    }
}
