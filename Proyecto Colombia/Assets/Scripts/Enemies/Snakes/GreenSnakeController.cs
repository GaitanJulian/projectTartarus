using Events;
using System.Collections;
using UnityEngine;
public class GreenSnakeController : EnemyController
{
    private Vector2 _freeMoveDirection; // A random vector that is used in the IdleState for a free movement direction
    protected bool _isIdle = true; // A boolean to check if the enemy is in idle state, this will allow to change from idle to chasing.
    protected bool _isAttacking = false;
    protected bool _isChasing = false;

    protected virtual void Start()
    {
        StartCoroutine(_idleCoroutine); // The snake starts at the Idle Coroutine
    }

    /// <summary>
    /// Executes the attack behavior of the snake.
    /// Right when the snake enters in attack state it stops moving, then attacks the player
    /// this behavior is defined in the code, it could be different like moving slower while attacking.
    /// Then the snake checks if the player is still in its attack range, if so, continues attacking.
    /// </summary>
    public override IEnumerator AttackState()
    {
        while (true)
        {
            _isAttacking = true;
            _rb.velocity = Vector2.zero; // In this case, at the start of the attack the snake stops moving

            // Attack the player
            Attack();

            // Wait for the attack duration
            yield return new WaitForSeconds(_enemyStats.attackTime);

            // Check if the player is still within attack range, using the context Steering along with the Attack range from _enemyStats Scriptable Object
            if (_contextSteering.DistanceFromTarget() > _enemyStats.attackRange)
            {
                _isAttacking = false;
                // If it is out of the attack range, we start chasing again
                ChangeState(_attackCoroutine, _chasingCoroutine); 
                //ChangeAnimationState(SNAKE_IDLE);
            }
        }
    }
    /// <summary>
    /// Executes the chase state
    /// This method uses the context steering for working.
    /// if the target is on sight, it checks first if is out of attack range, if so, moves towards the player
    /// if the target is on attack range, change its state to attack state.
    /// </summary>
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
                    _rb.velocity = _contextSteering.GetDirection() * _enemyStats.maxSpeed * _speedModifier;
     
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
    /// <summary>
    /// Basically, if the random direction in which it chooses to move has a wall, the snake will try to move just in the opposite direction,
    /// but if it has a wall too, it will do nothing but wait for the next random vector. 
    /// You can see that this state has no trigger for a chage of state, this happens in the OnDamageTaken method.
    /// </summary>
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
    /// <summary>
    /// This method is inherited from EnemyController. 
    /// this method is a listener to the onDamageTaken event from Damageable script
    /// </summary>
    /// <param name="_attacker"></param>
    /// the transform of the attacker, this could be removed later on
    /// <param name="damage"></param>
    protected override void OnDamageTaken(Transform _attacker, float damage)
    {
        if (_isIdle)
        {
            _isIdle = false;
            ChangeState(_idleCoroutine, _chasingCoroutine);
        }
    }
    /// <summary>
    /// Attack method, calls the event to alter player hitpoints
    /// </summary>
    protected override void Attack()
    {
        print("Done " + _enemyStats.damage + " dmg");
        EventManager.Dispatch(ENUM_Player.alterHitpoints, -_enemyStats.damage * _attackModifier);
    }
}