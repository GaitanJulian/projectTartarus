
using Unity.VisualScripting;
using UnityEngine;

public class PurpleSnakeController : GreenSnakeController
{
    protected new virtual void Update()
    {
        base.Update();
        if(_isIdle && _contextSteering.TargetCount() > 0)
        {
            _isIdle = false;
            ChangeState(_idleCoroutine, _chasingCoroutine);
        }
    }

    protected override void Attack()
    {
        base.Attack();
        if(_lastHit.collider != null)
        {
            _lastHit.transform.gameObject.GetComponentInChildren<CharacterStatsManager>().ApplyDamageOverTime(_otherEnemyStats._poissonDamage, _otherEnemyStats._poissonInterval, _otherEnemyStats._poissonDuration);
            
        }
    }

}
