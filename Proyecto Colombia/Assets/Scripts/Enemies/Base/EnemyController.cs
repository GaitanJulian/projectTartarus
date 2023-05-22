using System.Collections;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour, IEnemyStandarStates
{

    [SerializeField] protected EnemyStatsScriptableObject _enemyStats;
    [SerializeField] protected EnemyAiWithContextSteering _contextSteering;
    protected Rigidbody2D _rb;
    protected Damageable _damageable;
    private IEnumerator _attacking, _chasing, _idle; // Variables for stopping and starting coroutines


    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
        _rb = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {
        _attacking = AttackState();
        _chasing = ChaseState();
        _idle = IdleState();
        _damageable._onDamageTaken.AddListener(OnDamageTaken); // Event from the Damageable script
    }
    protected abstract void OnDamageTaken(Transform _enemy, float _damage);
    #region StandardCoroutines
    public abstract IEnumerator AttackState();
    public abstract IEnumerator ChaseState();
    public abstract IEnumerator IdleState();

    
    protected void StopAtaccking()
    {
        StopCoroutine(_attacking);
    }

    protected void StopChasing()
    {
        StopCoroutine(_chasing);
    }

    protected void StopIdle()
    {
        StopCoroutine(_idle);
    }

    protected void StartAttacking()
    {
        StartCoroutine(_attacking);
    }

    protected void StartChasing()
    {
        StartCoroutine(_chasing);
    }

    protected void StartIdle()
    {
        StartCoroutine(_idle);
    }

    #endregion

    
   
}