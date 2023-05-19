using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStateManager", menuName = "Enemy/State Manager")]
public class EnemyStateManagerScriptableObject : ScriptableObject
{
    public EnemyBaseState _idleState;
    public EnemyBaseState _chasingState;
    public EnemyBaseState _attackingState;

    [SerializeField] private EnemyStatsScriptableObject _enemyStats;
    
    // Any additional data or properties specific to the state manager can be defined here

    public EnemyBaseState GetInitialState()
    {
        return _idleState; // Return the initial state for the enemy
    }
}