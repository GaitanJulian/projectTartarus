using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EnemyStateManager", menuName = "Enemy/State Manager")]
public class EnemyStateManagerScriptableObject : ScriptableObject
{
    public EnemyBaseState _idleState;
    public EnemyBaseState _chasingState;
    public EnemyBaseState _attackingState;

    public UnityEvent<EnemyBaseState> _stateChangeEvent;
    // Any additional data or properties specific to the state manager can be defined here

    private void OnEnable()
    {
        if (_stateChangeEvent == null)
            _stateChangeEvent = new UnityEvent<EnemyBaseState>();
    }

    public void ChangeCurrentState(EnemyBaseState _state)
    {
        _stateChangeEvent.Invoke(_state);
    }
}