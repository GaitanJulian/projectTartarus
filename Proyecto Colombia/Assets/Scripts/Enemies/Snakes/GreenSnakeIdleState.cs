using UnityEngine;
public class GreenSnakeIdleState : EnemyBaseState
{
    public override void EnterState(EnemyStateManagerScriptableObject _stateManager, EnemyController _controller)
    {
        _controller._rb.velocity = Vector2.zero;
    }

    public override void UpdateState(EnemyStateManagerScriptableObject _stateManager, EnemyController _controller)
    {
        
    }
}