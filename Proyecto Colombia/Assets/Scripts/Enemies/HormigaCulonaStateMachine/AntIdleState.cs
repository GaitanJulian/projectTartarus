using UnityEngine;

public class AntIdleState : AntBaseState
{
    public override void EnterState(AntStateManager _context, Rigidbody2D _rb)
    {
        _rb.velocity = Vector2.zero;
    }
    public override void UpdateState(AntStateManager _context, Rigidbody2D _rb)
    {
        if(_context._contextSteering.TargetCount() > 0)
        {
            _context._contextSteering.ChooseTarget(0);
            _context.SwitchState(_context._chasingState);
            //This translates to...
            //If the player(s) enter(s) to the detection zone, asign (the closest one) as target...
            //And change to the "chasing state"
        }
        else
        {
            //Idle stuff:
        }
    }
}
