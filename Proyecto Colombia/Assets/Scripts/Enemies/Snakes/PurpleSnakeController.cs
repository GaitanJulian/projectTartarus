
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
}
