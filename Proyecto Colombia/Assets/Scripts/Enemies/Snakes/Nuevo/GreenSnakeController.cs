using Events;
using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
public class GreenSnakeController : EnemyController
{
    private Vector2 _freeMoveDirection; // A random vector that is used in the IdleState for a free movement direction
    protected bool _isIdle = true; // A boolean to check if the enemy is in idle state, this will allow to change from idle to chasing.
    protected bool _isAttacking = false;
    protected bool _isChasing = false;
    protected void Start()
    {
        StartCoroutine(_idleCoroutine);
    }

    public override IEnumerator AttackState()
    {
        while (true)
        {
            _isAttacking = true;
            _rb.velocity = Vector2.zero;

            // Attack the player
            Attack();

            // Wait for the attack duration
            yield return new WaitForSeconds(_enemyStats.attackTime);

            // Check if the player is still within attack range
            if (_contextSteering.DistanceFromTarget() > _enemyStats.attackRange)
            {
                _isAttacking = false;
                ChangeState(_attackCoroutine, _chasingCoroutine);
                //ChangeAnimationState(SNAKE_IDLE);
            }
        }
        
    }

    public override IEnumerator ChaseState()
    {
        while (true)
        {
            _isChasing = true;
            if (_contextSteering.TargetOnSight()) //if the target is on sight
            {
                if (_contextSteering.DistanceFromTarget() > _enemyStats.attackRange)
                {
                    //if target is further than attack distance
                    _rb.velocity = _contextSteering.GetDirection() * _enemyStats.maxSpeed;
     
                }
                else
                {
                    _isChasing = false;
                    ChangeState(_chasingCoroutine, _attackCoroutine);
                }
            }
            else
            {
                _isChasing = false;
                ChangeState(_chasingCoroutine, _idleCoroutine);
            }
            yield return null;
        }
    }

    public override IEnumerator IdleState()
    {
        while (true)
        {
            _isIdle = true;
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
            yield return new WaitForSeconds(_enemyStats.freeMovementTime);
        }
    }

    protected override void OnDamageTaken(Transform _attacker, float damage)
    {
        if (_isIdle)
        {
            _isIdle = false;
            ChangeState(_idleCoroutine, _chasingCoroutine);
        }
    }

    protected override void Attack()
    {
        EventManager.Dispatch(ENUM_Player.alterHitpoints, -_enemyStats.damage);
    }


}