using System.Collections;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour, IEnemyStandarStates
{
    // Serialized fields accessible in the Unity inspector
    [SerializeField] protected EnemyStatsScriptableObject _enemyStats;
    [SerializeField] protected EnemyAiWithContextSteering _contextSteering;

    // Protected fields accessible by child classes
    protected Rigidbody2D _rb;
    protected Damageable _damageable;

    // Coroutines for controlling enemy behavior
    protected IEnumerator _attackCoroutine, _chasingCoroutine, _idleCoroutine;

    protected virtual void Awake()
    {
        _damageable = GetComponent<Damageable>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _damageable._onDamageTaken.AddListener(OnDamageTaken); // Listen to the damage taken event
        _damageable._onDeath.AddListener(OnDeath); // Listen to the death event
    }

    private void OnDisable()
    {
        _damageable._onDamageTaken.RemoveListener(OnDamageTaken); // Stop listening to the damage taken event
        _damageable._onDeath.RemoveListener(OnDeath); // Stop listening to the death event
    }

    protected abstract void OnDamageTaken(Transform _enemy, float _damage); // Method to handle damage taken by the enemy
    protected abstract void Attack(); // Method to perform the attack action

    #region StandardCoroutines

    // Abstract methods representing the standard enemy states
    public abstract IEnumerator AttackState();
    public abstract IEnumerator ChaseState();
    public abstract IEnumerator IdleState();

    // Method to stop the attacking coroutine
    protected internal void StopAtaccking()
    {
        StopCoroutine(_attackCoroutine);
    }

    // Method to stop the chasing coroutine
    protected internal void StopChasing()
    {
        StopCoroutine(_chasingCoroutine);
    }

    // Method to stop the idle coroutine
    protected internal void StopIdle()
    {
        StopCoroutine(_idleCoroutine);
    }

    // Method to start the attacking coroutine
    protected internal void StartAttacking()
    {
        StartCoroutine(_attackCoroutine);
    }

    // Method to start the chasing coroutine
    protected internal void StartChasing()
    {
        StartCoroutine(_chasingCoroutine);
    }

    // Method to start the idle coroutine
    protected void StartIdle()
    {
        StartCoroutine(_idleCoroutine);
    }

    #endregion

    protected void OnDeath()
    {
        Destroy(gameObject); // Destroy the enemy game object upon death
    }
}