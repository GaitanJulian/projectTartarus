using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
public class GreenSnakeController : EnemyController
{
    
    private Transform _attacker;
    private bool _isAttacked;
    private Vector2 _freeMoveDirection;
    public override IEnumerator AttackState()
    {
        while (true)
        {
            _rb.velocity = Vector2.zero;

            // Attack the player
            Attack();

            // Wait for the attack duration
            yield return new WaitForSeconds(_enemyStats.attackTime);

            // Check if the player is still within attack range
            if (_contextSteering.DistanceFromTarget() > _enemyStats.attackRange)
            {
                StartChasing();
                StopAtaccking();
                //ChangeAnimationState(SNAKE_IDLE);
            }
        }
        
    }

    public override IEnumerator ChaseState()
    {
        while (true)
        {
            if (_contextSteering.TargetOnSight()) //if the target is on sight
            {
                if (_contextSteering.DistanceFromTarget() > _enemyStats.attackRange)
                {
                    //if target is further than attack distance
                    //_rb.velocity = _context._contextSteering.GetDirection() * _context.MoveVelocity;
                    _rb.velocity = _contextSteering.GetDirection() * _enemyStats.maxSpeed;
                }
                else
                {
                    StartAttacking();
                    StopChasing();
                }
            }
            else
            {
                StartIdle();
                StopChasing();
            }
        }
        
    }

    public override IEnumerator IdleState()
    {
        while (true)
        {
            // Generate a random direction for the snake
            _freeMoveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

            // Check if the snake hits a wall in the desired direction
            RaycastHit2D wallHit = Physics2D.Raycast(transform.position, _freeMoveDirection, _enemyStats.wallCheckDistance, _enemyStats.wallLayerMask);
            if (wallHit.collider == null)
            {
                // Move the snake in the specified direction
                _rb.velocity = _freeMoveDirection * _enemyStats.maxSpeed;

                yield return new WaitForSeconds(_enemyStats.freeMovementTime);

                _rb.velocity = Vector2.zero;
            }
            else
            {
                // Check if the snake hits a wall in the opposite direction
                RaycastHit2D oppositeWallHit = Physics2D.Raycast(transform.position, -_freeMoveDirection, _enemyStats.wallCheckDistance, _enemyStats.wallLayerMask);
                if (oppositeWallHit.collider == null)
                {
                    // Calculate the opposite direction of the wall
                    _freeMoveDirection = Vector2.Reflect(-_freeMoveDirection, oppositeWallHit.normal);

                    // Move the snake in the opposite direction
                    _rb.velocity = _freeMoveDirection * _enemyStats.maxSpeed;

                    yield return new WaitForSeconds(_enemyStats.freeMovementTime);

                    _rb.velocity = Vector2.zero;
                }
            }
        }
    }

    protected override void OnDamageTaken(Transform _attacker, float damage)
    {
        this._attacker = _attacker;
        StopIdle();
        StartChasing();

    }

    protected override void Attack()
    {
        print("Attacking");
    }

}