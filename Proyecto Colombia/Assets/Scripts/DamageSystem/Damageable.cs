using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] float _hitPoints = 100f;
    [SerializeField] bool _isBoss;

    float _startHitPoints;
    bool _hitByCactus = false;
    bool _applyDoubleDamage = false;
    private Transform _attacker;

    public UnityEvent<Transform, float> _onDamageTaken = new UnityEvent<Transform, float>();
    public UnityEvent _onDeath = new UnityEvent();

    private void Start()
    {
        _startHitPoints = _hitPoints;
    }

    public void GetDamaged(float magnitude)
    {
        if (_applyDoubleDamage)
        {
            magnitude *= 2f;
        }

        if (_hitPoints > magnitude)
        {
            _hitPoints -= magnitude;
            _onDamageTaken?.Invoke(_attacker, magnitude);
        
        }
        else
        {
            _hitPoints = 0;
            _onDeath?.Invoke();
        }
    }

    public void SetDoubleDamage(bool applyDoubleDamage)
    {
        _applyDoubleDamage = applyDoubleDamage;
    }


    public void SetAttacker(Transform _attacker) => this._attacker = _attacker;
    public float GetMaxHitPoints() => _startHitPoints;

    #region For cactus interaction
    public void HitByCactusState(bool state) => _hitByCactus = state;
    public bool GetHitByCactusState() => _hitByCactus;
    public bool GetIsBossBool() => _isBoss;
    #endregion

    #region For Poison
    float _damageMultiplier = 1f;
    
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