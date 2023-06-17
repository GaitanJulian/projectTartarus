using System.Collections;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour, IEnemyStandarStates
{
    // Stats manager, Responsable for applying different states to characters based on their Stats Scriptable object
    protected CharacterStatsManager _characterStatsManager;

    // Serialized fields accessible in the Unity inspector
    [SerializeField] protected EnemyAiWithContextSteering _contextSteering;
    [SerializeField] protected Animator _animator;
 
    // Protected fields accessible by child classes
    protected Rigidbody2D _rb;
    protected Damageable _damageable;

    // Coroutines for controlling enemy behavior
    protected IEnumerator _attackCoroutine, _chasingCoroutine, _idleCoroutine; // This variables allow to start and stop a certain coroutine

    protected virtual void Awake()
    {
        _damageable = GetComponent<Damageable>();
        _rb = GetComponent<Rigidbody2D>();
        _attackCoroutine = AttackState(); // Each variable must be assigned to its corresponding coroutine, this case the Attack State
        _chasingCoroutine = ChaseState();
        _idleCoroutine = IdleState();
        _animator = GetComponentInChildren<Animator>();
        _characterStatsManager = GetComponentInChildren<CharacterStatsManager>();
    }

    protected virtual void Start()
    {

    }

    private void OnEnable()
    {
        _damageable._onDamageTaken.AddListener(OnDamageTaken); // Listen to the damage taken event, check event from the Damageable script
        _damageable._onDeath.AddListener(OnDeath); // Listen to the death event
    }

    private void OnDisable()
    {
        _damageable._onDamageTaken.RemoveListener(OnDamageTaken); // Stop listening to the damage taken event
        _damageable._onDeath.RemoveListener(OnDeath); // Stop listening to the death event
    }

    protected abstract void OnDamageTaken(float _damage); // Method to handle damage taken by the enemy
    protected abstract void Attack(); // Method to perform the attack action

    // Abstract methods representing the standard enemy states
    public abstract IEnumerator AttackState();
    public abstract IEnumerator ChaseState();
    public abstract IEnumerator IdleState();

    protected void OnDeath()
    {
        Destroy(gameObject); // Destroy the enemy game object upon death
    }

    // This method allows you easily change from one coroutine to another
    protected void ChangeState(IEnumerator _stopState, IEnumerator _startState) 
    {
        StopCoroutine(_stopState);
        StartCoroutine(_startState);
    }

    #region AlteredStates

    float _poisonedTimer, _stunedTimer;
    bool _isPoisoned = false, _isStuned = false;

    public void Poison(float time)
    {
        _isPoisoned = true;
        _poisonedTimer = time;
        Debug.Log("applied poisson");
    }

    public void Stun(float time)
    {
        _isPoisoned = false;
        _poisonedTimer = 0;
        _isStuned = true;
        _stunedTimer = time;
        Debug.Log("applied stun");
    }
    #endregion
}