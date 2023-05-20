using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] public EnemyStateManagerScriptableObject _stateManager;

    [SerializeField] public EnemyStatsScriptableObject _enemyStats;
    [SerializeField] public EnemyAiWithContextSteering _contextSteering;

    public Rigidbody2D _rb;

    protected virtual void Start()
    {
        _stateManager = GetComponent<EnemyStateManagerScriptableObject>();
        _rb = GetComponent<Rigidbody2D>();
        _stateManager.ChangeCurrentState(_stateManager._idleState);
    }
    protected void Update()
    {
        _stateManager._currentState.UpdateState(_stateManager, this);
    }

}