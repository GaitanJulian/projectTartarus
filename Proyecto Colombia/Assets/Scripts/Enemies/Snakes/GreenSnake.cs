using System.Collections;
using UnityEngine;

public class GreenSnake : SnakeEnemyBase
{

    protected override IEnumerator MoveFreelyCoroutine()
    {
        while (true)
        {
            // Generate a random direction for the snake
            _freeMoveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

            // Check if the snake hits a wall in the desired direction
            RaycastHit2D wallHit = Physics2D.Raycast(transform.position, _freeMoveDirection, _snakeStats.wallCheckDistance, _snakeStats.wallLayerMask);
            if (wallHit.collider == null)
            {
                // Move the snake in the specified direction
                _rb.velocity = _freeMoveDirection * _snakeStats.maxSpeed;

                yield return new WaitForSeconds(_snakeStats.freeMovementTime);

                _rb.velocity = Vector2.zero;
            }
            else
            {
                // Check if the snake hits a wall in the opposite direction
                RaycastHit2D oppositeWallHit = Physics2D.Raycast(transform.position, -_freeMoveDirection, _snakeStats.wallCheckDistance, _snakeStats.wallLayerMask);
                if (oppositeWallHit.collider == null)
                {
                    // Calculate the opposite direction of the wall
                    _freeMoveDirection = Vector2.Reflect(-_freeMoveDirection, oppositeWallHit.normal);

                    // Move the snake in the opposite direction
                    _rb.velocity = _freeMoveDirection * _snakeStats.maxSpeed;

                    yield return new WaitForSeconds(_snakeStats.freeMovementTime);

                    _rb.velocity = Vector2.zero;
                }
            }
            yield return new WaitForSeconds(_snakeStats.freeMovementTime);

        }


    }

  

    protected override IEnumerator AttackCoroutine()
    {
        while (_isAggressive)
        {
            // Calculate direction towards the player
            _lastAttackDirection = (_playerTransform.position - transform.position).normalized;

            // Move towards the player
            _rb.velocity = _lastAttackDirection * _snakeStats.maxSpeed;

            // Check if the player is within attack range
            if (Vector3.Distance(transform.position, _playerTransform.position) <= _snakeStats.attackRange)
            {
                // Store direction

                // Stop the snake's movement
                _rb.velocity = Vector2.zero;

                // Attack the player
                Attack();

                // Wait for the attack duration
                yield return new WaitForSeconds(_snakeStats.attackTime);

                // Check if the player is still within attack range
                if (Vector3.Distance(transform.position, _playerTransform.position) > _snakeStats.attackRange)
                {
                    // Resume chasing the player
                    ChangeAnimationState(SNAKE_IDLE);
                    continue;
                }
            }

            yield return null;
        }
    }
}