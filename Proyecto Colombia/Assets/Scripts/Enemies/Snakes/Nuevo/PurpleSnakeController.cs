
public class PurpleSnakeController : GreenSnakeController
{
    protected virtual void Update()
    {
        if(_isIdle && _contextSteering.TargetCount() > 0)
        {
            _isIdle = false;
            ChangeState(_idleCoroutine, _chasingCoroutine);
        }
    }
}
