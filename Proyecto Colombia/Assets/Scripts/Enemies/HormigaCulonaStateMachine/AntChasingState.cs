using UnityEngine;

public class AntChasingState : AntBaseState
{
    public override void EnterState(AntStateManager _context, Rigidbody2D _rb) { }
    public override void UpdateState(AntStateManager _context, Rigidbody2D _rb)
    {
        if (_context._contextSteering.TargetOnSight()) //if the target is on sight
        {
            if(_context._contextSteering.DistanceFromTarget() > _context.AttackDistance)
            {
                //if target is further than attack distance
                //_rb.velocity = _context._contextSteering.GetDirection() * _context.MoveVelocity;
                _rb.AddForce(_context._contextSteering.GetDirection() * _context.MoveVelocity);
                Debug.Log(_context._contextSteering.DistanceFromTarget());
            }
            else
            {
                //if target gets closer than attack distance
                _context.SwitchState(_context._attackState);
            }
        }
        else //if the player is not on sight
        {
            _context.SwitchState(_context._idleState);
        }
    }
}
