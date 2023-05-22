using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] public EnemyStateManagerScriptableObject _stateManager;
    [HideInInspector] public EnemyBaseState _currentState;

    [SerializeField] public EnemyStatsScriptableObject _enemyStats;
    [SerializeField] public EnemyAiWithContextSteering _contextSteering;

    [HideInInspector]public Rigidbody2D _rb;

    [HideInInspector] public Damageable _damageable;


    private void OnEnable()
    {
        _stateManager._stateChangeEvent.AddListener(OnStateChange);
    }

    private void OnDisable()
    {
        _stateManager._stateChangeEvent.RemoveListener(OnStateChange);
    }

    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
    }
    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentState = _stateManager._idleState;
        _currentState.EnterState(_stateManager, this);
    }
    protected void Update()
    {
        _currentState.UpdateState(_stateManager, this);
        print(_currentState);
    }

    private void OnStateChange(EnemyBaseState newState)
    {
        print("Cambio de estado !!!!!!");
        _currentState = newState;
        _currentState.EnterState(_stateManager, this);
    }


}