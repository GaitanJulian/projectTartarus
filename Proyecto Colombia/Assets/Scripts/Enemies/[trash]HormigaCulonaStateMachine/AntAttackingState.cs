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
            Vector2 desiredVector = _context._contextSteering.GetDirection();
            Vector2 perpendicularVector = new Vector2(-desiredVector.y, desiredVector.x * Random.Range(0.8f, 1.3f)) * _context.RandomDirection;
            _rb.velocity = perpendicularVector * _context.MoveVelocity;

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
