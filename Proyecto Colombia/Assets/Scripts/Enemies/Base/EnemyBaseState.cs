using UnityEngine;

public abstract class EnemyBaseState : ScriptableObject
{
    public abstract void EnterState(EnemyStateManagerScriptableObject _stateManager, EnemyController _controller);
    public abstract void UpdateState(EnemyStateManagerScriptableObject _stateManager, EnemyController _controller);
}