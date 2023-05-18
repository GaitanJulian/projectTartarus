using Events;
using System.Collections;
using System.Threading;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AntAttackingState : AntBaseState
{
    float _timer = 0;
    bool _ableToAttack = true;


    public override void EnterState(AntStateManager _context, Rigidbody2D _rb)
    {
        _timer = 0;

        _ableToAttack = true;
    }
    public override void UpdateState(AntStateManager _context, Rigidbody2D _rb)
    {
        if (_context._contextSteering.DistanceFromTarget() <= _context.AttackDistance)
        {
            //_rb.velocity = _context._contextSteering.GetDirection() * _context.MoveWhileAttackingVelocity;

            if (_ableToAttack)
            {
                EventManager.Dispatch(ENUM_Player.alterHitpoints, -_context.AttackMagnitude);
                _timer = 0;
                _ableToAttack = false;
            }
            else
            {
                Clock(_context);
            }
        }
        else
        {
            //go chasing again
            _context.SwitchState(_context._chasingState);
        }



    }

    void Clock(AntStateManager _context)
    {
        _timer += Time.deltaTime;
        if (_timer >= _context.AttackWaitTime)
        {
            _ableToAttack = true;
        }
    }
}
