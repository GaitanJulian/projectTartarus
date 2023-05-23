
public class PurpleSnakeController : GreenSnakeController
{
    private void Update()
    {
        if(_isIdle && _contextSteering.TargetCount() > 0)
        {
            _isIdle = false;
            ChangeState(_idleCoroutine, _chasingCoroutine);
        }
    }
}
