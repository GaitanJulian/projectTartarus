using System.Collections;

public interface IEnemyStandarStates
{
        IEnumerator IdleState();
        IEnumerator ChaseState();
        IEnumerator AttackState();
}
