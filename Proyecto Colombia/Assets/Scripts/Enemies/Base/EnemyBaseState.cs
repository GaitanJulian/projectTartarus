using UnityEngine;

public abstract class EnemyBaseState : ScriptableObject
{
    public abstract void EnterState(EnemyController enemy);
    public abstract void UpdateState(EnemyController enemy);
}