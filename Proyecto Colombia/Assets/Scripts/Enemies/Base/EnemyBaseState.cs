using UnityEngine;

public abstract class EnemyBaseState : ScriptableObject
{
    public abstract void EnterState(EnemyStateManagerScriptableObject _stateManager, Rigidbody2D _rb);
    public abstract void UpdateState(EnemyStateManagerScriptableObject _stateManager, Rigidbody2D _rb);
}