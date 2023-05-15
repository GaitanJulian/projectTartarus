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
    private const string _playerTag = "Player";
    private IEnumerator _freeMovementState;
    private IEnumerator _attackState;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _freeMovementState = MoveFreelyCoroutine();
        _attackState = AttackCoroutine();
    }

    private void Start()
    {
        StartCoroutine(_freeMovementState);
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
    }

    private IEnumerator MoveFreelyCoroutine()
    {
        while (true)
        {
            // Generate a random direction for the snake
            Vector2 _direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

            // Move the snake in the specified direction
            _rb.velocity = _direction * _snakeStats.maxSpeed;

            // Wait for the specified move time
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
                    continue;
                }
            }

            yield return null;
        }
    }
}
