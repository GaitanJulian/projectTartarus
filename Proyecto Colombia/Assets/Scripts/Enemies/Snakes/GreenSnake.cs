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
                    _freeMoveDirection = Vector2.Reflect(_freeMoveDirection, oppositeWallHit.normal);

                    // Move the snake in the opposite direction
                    _rb.velocity = _freeMoveDirection * _snakeStats.maxSpeed;

                    yield return new WaitForSeconds(_snakeStats.freeMovementTime);

                    _rb.velocity = Vector2.zero;
                }
            }
       
        }


    }
    

    protected override IEnumerator AttackCoroutine()
    {
        // Implement the attack logic for the green snake
        // ...

        yield return null;
    }
}