using Events;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GreenSnakeController : EnemyController
{
    private Vector2 _freeMoveDirection; // A random vector that is used in the IdleState for a free movement direction
    protected bool _isIdle = true; // A boolean to check if the enemy is in idle state, this will allow to change from idle to chasing.
    protected bool _isAttacking = false;
    protected bool _isChasing = false;
    private Vector2 _lastSpeed; // Track the previous speed to determine the last direction
    private string _currentState; // Current animation state

    protected SnakesStatsScriptableObject _otherEnemyStats;

    // The followings states could be done in an ENUM or a ScriptableObject.
    const string SNAKE_IDLE = "Snake_idle";
    const string SNAKE_IDLE_UPWARDS = "Snake_idle_upwards";
    const string SNAKE_IDLE_DOWNWARDS = "Snake_idle_downwards";
    const string SNAKE_HORIZONTAL = "Snake_horizontal";
    const string SNAKE_HORIZONTAL_ATTACK = "Snake_horizontal_attack";
    const string SNAKE_UPWARDS = "Snake_upwards";
    const string SNAKE_UPWARDS_ATTACK = "Snake_upwards_attack";
    const string SNAKE_DOWNWARDS = "Snake_downwards";
    const string SNAKE_DOWNWARDS_ATTACK = "Snake_downwards_attack";


    protected override void Awake()
    {
        base.Awake();
        _otherEnemyStats = (SnakesStatsScriptableObject)_characterStatsManager._characterStats;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(_idleCoroutine); // The snake starts at the Idle Coroutine
    }


    protected void Update()
    {
        if (_rb.velocity.x > 0) transform.localScale = new Vector2(-1, 1);
        if (_rb.velocity.x < 0) transform.localScale = Vector2.one;

        if(_isIdle || _isChasing)
        {
            if (_rb.velocity.magnitude >= 0.1f || _rb.velocity.magnitude <= 0.1f) _lastSpeed = _rb.velocity;
            StartCoroutine(ChangeIdleAnimationCoroutine());

        }
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
            yield return StartCoroutine(ChangeAttackAnimationCoroutine());
            _rb.velocity = Vector2.zero; // In this case, at the start of the attack the snake stops moving
            // Wait for the attack duration
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
            yield return StartCoroutine(ChangeIdleAnimationCoroutine());
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
            // Check if the player is still within attack range, using the context Steering along with the Attack range from _enemyStats Scriptable Object
            if (_contextSteering.DistanceFromTarget() > _characterStatsManager._currentAttackRange)
            {
                _isAttacking = false;
                // If it is out of the attack range, we start chasing again
                ChangeState(_attackCoroutine, _chasingCoroutine);
                yield return new WaitForSeconds(0.5f);
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
                if (_contextSteering.DistanceFromTarget() > _characterStatsManager._currentAttackRange)
                {
                    //if target is further than attack distance
                    _rb.velocity = _contextSteering.GetDirection() * _characterStatsManager._currentSpeed ;
     
                }
                else
                {
                    _isChasing = false;
                    ChangeState(_chasingCoroutine, _attackCoroutine);
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                _isChasing = false;
                ChangeState(_chasingCoroutine, _idleCoroutine);
                yield return new WaitForEndOfFrame();
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
            RaycastHit2D wallHit = Physics2D.Raycast(transform.position, _freeMoveDirection, _otherEnemyStats._wallCheckDistance, _otherEnemyStats._wallLayerMask);
            if (wallHit.collider == null)
            {
                // Move the snake in the specified direction
                _rb.velocity = _freeMoveDirection * _characterStatsManager._currentSpeed;

                yield return new WaitForSeconds(_otherEnemyStats._freeMovementTime);

                _rb.velocity = Vector2.zero;
            }
            else
            {
                // Check if the snake hits a wall in the opposite direction
                RaycastHit2D oppositeWallHit = Physics2D.Raycast(transform.position, -_freeMoveDirection, _otherEnemyStats._wallCheckDistance, _otherEnemyStats._wallLayerMask);
                if (oppositeWallHit.collider == null)
                {
                    // Calculate the opposite direction of the wall
                    _freeMoveDirection = Vector2.Reflect(-_freeMoveDirection, oppositeWallHit.normal);

                    // Move the snake in the opposite direction
                    _rb.velocity = _freeMoveDirection * _characterStatsManager._currentSpeed;

                    yield return new WaitForSeconds(_otherEnemyStats._freeMovementTime);

                    _rb.velocity = Vector2.zero;
                }
            }
            yield return new WaitForSeconds(_otherEnemyStats._freeMovementTime);
        }
    }
    /// <summary>
    /// This method is inherited from EnemyController. 
    /// this method is a listener to the onDamageTaken event from Damageable script
    /// </summary>
    /// <param name="_attacker"></param>
    /// the transform of the attacker, this could be removed later on
    /// <param name="damage"></param>
    protected override void OnDamageTaken(float damage)
    {
        if (_isIdle)
        {
            _isIdle = false;
            ChangeState(_idleCoroutine, _chasingCoroutine);
        }
    }

    protected virtual RaycastHit2D RayHit()
    {
        // Perform a raycast in the attack direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _lastSpeed.normalized, _characterStatsManager._currentAttackRange, _otherEnemyStats._playerLayerMask);

        return hit;
    }
    protected RaycastHit2D _lastHit; // This variable will be used in the child classes
    /// <summary>
    /// Attack method, calls the event to alter player hitpoints
    /// </summary>
    protected override void Attack()
    {
        // Perform a raycast in the attack direction
        RaycastHit2D _hit = RayHit();

        // Check if the raycast hits the player
        if (_hit.collider != null)
        {
            _hit.transform.gameObject.GetComponent<Damageable>().GetDamaged(_characterStatsManager._currentAttackDamage);
            _lastHit = _hit;
        }
        
    }

    protected IEnumerator ChangeAnimationStateCoroutine(string newState)
    {
        // Prevents the animation from interrupting itself
        if (_currentState == newState)
            yield break;

        // Change the animation
        _animator.Play(newState);

        _currentState = newState;
    }

    protected IEnumerator ChangeAttackAnimationCoroutine()
    {
        float horizontalSpeed = Mathf.Abs(_lastSpeed.x);
        float verticalSpeed = Mathf.Abs(_lastSpeed.y);

        if (horizontalSpeed > verticalSpeed)
        {
            yield return StartCoroutine(ChangeAnimationStateCoroutine(SNAKE_HORIZONTAL_ATTACK));
        }
        else if (_lastSpeed.y > 0)
        {
            yield return StartCoroutine(ChangeAnimationStateCoroutine(SNAKE_UPWARDS_ATTACK));
        }
        else if (_lastSpeed.y < 0)
        {
            yield return StartCoroutine(ChangeAnimationStateCoroutine(SNAKE_DOWNWARDS_ATTACK));
        }
        else
        {
            yield break;
        }
    }

    protected IEnumerator ChangeIdleAnimationCoroutine()
    {
        if (Mathf.Abs(_rb.velocity.x) > Mathf.Abs(_rb.velocity.y))
        {
            yield return StartCoroutine(ChangeAnimationStateCoroutine(SNAKE_HORIZONTAL));
        }
        else if (_rb.velocity.y > 0)
        {
            yield return StartCoroutine(ChangeAnimationStateCoroutine(SNAKE_UPWARDS));
        }
        else if (_rb.velocity.y < 0)
        {
            yield return StartCoroutine(ChangeAnimationStateCoroutine(SNAKE_DOWNWARDS));
        }
        else
        {
            float horizontalSpeed = Mathf.Abs(_lastSpeed.x);
            float verticalSpeed = Mathf.Abs(_lastSpeed.y);
            if (horizontalSpeed > verticalSpeed)
            {
                yield return StartCoroutine(ChangeAnimationStateCoroutine(SNAKE_IDLE));
            }
            else if (_lastSpeed.y > 0)
            {
                yield return StartCoroutine(ChangeAnimationStateCoroutine(SNAKE_IDLE_UPWARDS));
            }
            else
            {
                yield return StartCoroutine(ChangeAnimationStateCoroutine(SNAKE_IDLE_DOWNWARDS));
            }
        }
    }

}