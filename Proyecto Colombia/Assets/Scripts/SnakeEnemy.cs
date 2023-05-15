using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemy : MonoBehaviour
{
    [SerializeField] private EnemyStatsScriptableObject _snakeStats;
    public Transform _player; // Reference to the player's transform
    private Rigidbody2D _rb;
    private bool _isAggressive = false; // Boolean that tells if the snake has been attacked
    private Vector3 _lastAttackDirection; // The attack direction of the snake
    private Vector2 _freeMoveDirection; // movement of the snake when is not attacing
    private const string _playerTag = "Player";
    private IEnumerator _freeMovementState; // Coroutine for free movement in the map
    private IEnumerator _attackState; // Coroutine for chasing and attacking the player
    private Animator _animator;

    // Animation States
    const string SNAKE_IDLE = "snake_idle";
    const string SNAKE_ATTACK = "snake_attack";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _freeMovementState = MoveFreelyCoroutine();
        _attackState = AttackCoroutine();
    }

    private void Start()
    {
        StartCoroutine(_freeMovementState);
        ChangeAnimationState(SNAKE_IDLE);
    }

    private void Update()
    {
        if(_lastAttackDirection == Vector3.zero)
        {
            Debug.DrawRay(transform.position, _freeMoveDirection * _snakeStats.wallCheckDistance, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, _lastAttackDirection * _snakeStats.attackRange, Color.red);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(_playerTag))
        {
            StopCoroutine(_freeMovementState);
            // Set the player as the attacker
            _player = collision.gameObject.transform;

            // Make the snake aggressive
            _isAggressive = true;
            StartCoroutine(_attackState);
        }
    }

    private void Attack()
    {
        // Perform the attack in the last attack direction
        // You can implement the attack logic here, such as spawning projectiles or triggering an animation
        Debug.Log("Snake attacks in the direction: " + _lastAttackDirection);
        ChangeAnimationState(SNAKE_ATTACK);
    }

    private IEnumerator MoveFreelyCoroutine()
    {
        while (true)
        {
            // Generate a random direction for the snake
            _freeMoveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

            // Check if the snake hits a wall in the desired direction
            RaycastHit2D wallHit = Physics2D.Raycast(transform.position, _freeMoveDirection , _snakeStats.wallCheckDistance, _snakeStats.wallLayerMask);
            if (wallHit.collider == null)
            {
                print("Me puedo mover, no hubo collider");
                // Move the snake in the specified direction
                _rb.velocity = _freeMoveDirection * _snakeStats.maxSpeed;

                // Wait for the specified move time
                yield return new WaitForSeconds(_snakeStats.freeMovementTime);

                // Stop the snake's movement
                _rb.velocity = Vector2.zero;
            }
            else
            {
                // Check if the snake hits a wall in the opposite direction
                RaycastHit2D oppositeWallHit = Physics2D.Raycast(transform.position, -_freeMoveDirection, _snakeStats.wallCheckDistance, _snakeStats.wallLayerMask);
                if (oppositeWallHit.collider == null)
                {
                    print("Me movi en direccion contraria");
                    // Calculate the opposite direction of the wall
                    _freeMoveDirection = Vector2.Reflect(-_freeMoveDirection, oppositeWallHit.normal);

                    // Move the snake in the opposite direction
                    _rb.velocity = _freeMoveDirection * _snakeStats.maxSpeed;
                }
            }
            
            // Wait for a short duration to move away from the wall
            yield return new WaitForSeconds(_snakeStats.freeMovementTime);

            // Stop the snake's movement
            _rb.velocity = Vector2.zero;

            // Wait for the specified move time again
            yield return new WaitForSeconds(_snakeStats.freeMovementTime);
        }
    }

    private IEnumerator AttackCoroutine()
    {
        while (_isAggressive)
        {
            // Calculate direction towards the player
            _lastAttackDirection = (_player.position - transform.position).normalized;

            // Move towards the player
            _rb.velocity = _lastAttackDirection * _snakeStats.maxSpeed;

            // Check if the player is within attack range
            if (Vector3.Distance(transform.position, _player.position) <= _snakeStats.attackRange)
            {
                // Store direction

                // Stop the snake's movement
                _rb.velocity = Vector2.zero;

                // Attack the player
                Attack();

                // Wait for the attack duration
                yield return new WaitForSeconds(_snakeStats.attackTime);

                // Check if the player is still within attack range
                if (Vector3.Distance(transform.position, _player.position) > _snakeStats.attackRange)
                {
                    // Resume chasing the player
                    ChangeAnimationState(SNAKE_IDLE);
                    continue;
                }
            }

            yield return null;
        }
    }

    private void ChangeAnimationState(string newState)
    {
        _animator.Play(newState);
    }
}
