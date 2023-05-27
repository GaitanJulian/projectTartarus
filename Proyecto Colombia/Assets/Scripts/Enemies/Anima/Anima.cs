using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anima : EnemyController
{
    public override IEnumerator AttackState()
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator ChaseState()
    {
        throw new System.NotImplementedException();
    }

 

    public override IEnumerator IdleState()
    {
        throw new System.NotImplementedException();
    }



    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnDamageTaken(Transform _enemy, float _damage)
    {
        throw new System.NotImplementedException();
    }
}
