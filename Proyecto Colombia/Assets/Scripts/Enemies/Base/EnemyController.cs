using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    protected EnemyStateManagerScriptableObject stateManager;
    protected EnemyBaseState currentState;

    protected virtual void Start()
    {
        stateManager = GetComponent<EnemyStateManagerScriptableObject>();

    }

    protected virtual void OnStateEnter(EnemyController enemy)
    {
        // Handle state enter event for the current state
    }

    protected virtual void OnStateUpdate(EnemyController enemy)
    {
        // Handle state update event for the current state
    }

  
}