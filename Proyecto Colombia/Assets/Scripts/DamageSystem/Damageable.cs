using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] protected float _maxHitPoints;
    [SerializeField] bool _isBoss;

    protected float _currentHitPoints;
    bool _hitByCactus = false;
    private Transform _attacker;

    public float _damageMultiplier;

    public UnityEvent<float> _onDamageTaken = new UnityEvent<float>();
    public UnityEvent _onDeath = new UnityEvent();

    private void Start()
    {
        _currentHitPoints = _maxHitPoints;
        _damageMultiplier = 1f;
    }

    public void SetDamageMultiplier(float multiplier)
    {
        _damageMultiplier = multiplier;
    }

    public void GetDamaged(float magnitude)
    {
        if (_currentHitPoints > magnitude * _damageMultiplier)
        {
            _currentHitPoints -= magnitude * _damageMultiplier;
            _onDamageTaken?.Invoke(magnitude * _damageMultiplier);
        }
        else
        {
            _currentHitPoints = 0;
            _onDeath?.Invoke();
        }
    }
    public void SetAttacker(Transform _attacker) => this._attacker = _attacker;
    public float GetMaxHitPoints() => _maxHitPoints;

    #region For cactus interaction
    public void HitByCactusState(bool state) => _hitByCactus = state;
    public bool GetHitByCactusState() => _hitByCactus;
    public bool GetIsBossBool() => _isBoss;
    #endregion

    #region For Poison

    
    public void Stun(float time)
    {
        _damageMultiplier = 2f;
        Invoke("RevertStun", time);
    }
     void RevertStun()
    {
        _damageMultiplier = 1f;
    }
    #endregion
}