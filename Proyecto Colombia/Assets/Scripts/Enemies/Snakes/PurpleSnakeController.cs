
using Unity.VisualScripting;

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
        _playerStats.ApplyDamageOverTime(_otherEnemyStats._poissonDamage, _otherEnemyStats._poissonInterval, _otherEnemyStats._poissonDuration);
    }

}
