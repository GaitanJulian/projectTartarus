using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStateManager", menuName = "Enemy/State Manager")]
public class EnemyStateManagerScriptableObject : ScriptableObject
{
    public EnemyBaseState idleState;
    public EnemyBaseState chasingState;
    public EnemyBaseState attackingState;
    // Add more states as needed

    // Any additional data or properties specific to the state manager can be defined here

    public EnemyBaseState GetInitialState()
    {
        return idleState; // Return the initial state for the enemy
    }
}