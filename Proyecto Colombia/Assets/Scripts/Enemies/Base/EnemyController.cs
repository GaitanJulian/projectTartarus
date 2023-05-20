using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    protected EnemyStateManagerScriptableObject _stateManager;

    [SerializeField] private EnemyStatsScriptableObject _enemyStats;
    [SerializeField] public EnemyAiWithContextSteering _contextSteering;

    private Rigidbody2D _rb;

    protected virtual void Start()
    {
        _stateManager = GetComponent<EnemyStateManagerScriptableObject>();
        _rb = GetComponent<Rigidbody2D>();
        _stateManager.ChangeCurrentState(_stateManager._idleState);
    }
    protected void Update()
    {
        _stateManager._currentState.UpdateState(_stateManager, _rb);
    }

}