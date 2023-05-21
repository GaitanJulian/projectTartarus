using UnityEngine;
public class GreenSnakeController : EnemyController
{
    
    [HideInInspector] public Transform _attacker;
    [HideInInspector] public bool _isAttacked;
    [HideInInspector] public bool _isMoving;
    private void OnEnable()
    {
        
        _damageable._onDeath.AddListener(OnDeath);
        _damageable._onDamageTaken.AddListener(OnDamageTaken);
    }

    private void OnDisable()
    {
        _damageable._onDeath.RemoveListener(OnDeath);    
        _damageable._onDamageTaken.RemoveListener(OnDamageTaken);
        _damageable = null;
    }

    private void OnDamageTaken(Transform _attacker, float damage)
    {
        this._attacker = _attacker;
        _isAttacked = true;

    }

    private void OnDeath()
    {
       Destroy(gameObject);
    }
}