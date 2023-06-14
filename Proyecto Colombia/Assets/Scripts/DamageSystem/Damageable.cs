using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] float _HitPoints = 100f;
    [SerializeField] bool _isBoss;

    float _startHitPoints;
    bool _hitByCactus = false;
    private Transform _attacker;

    public float _damageMultiplier;

    public UnityEvent<Transform, float> _onDamageTaken = new UnityEvent<Transform, float>();
    public UnityEvent _onDeath = new UnityEvent();

    private void Start()
    {
        _startHitPoints = _HitPoints;
    }

    public void SetDamageMultiplier(float multiplier)
    {
        _damageMultiplier = multiplier;
    }

    public void GetDamaged(float magnitude)
    {
        if (_HitPoints > magnitude * _damageMultiplier)
        {
            _HitPoints -= magnitude * _damageMultiplier;
            _onDamageTaken?.Invoke(_attacker, magnitude * _damageMultiplier);
        }
        else
        {
            _HitPoints = 0;
            _onDeath?.Invoke();
        }
    }
    public void SetAttacker(Transform _attacker) => this._attacker = _attacker;
    public float GetMaxHitPoints() => _startHitPoints;

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