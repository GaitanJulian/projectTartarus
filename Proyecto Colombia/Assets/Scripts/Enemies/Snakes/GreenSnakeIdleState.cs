using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "GreenSnakeIdleState", menuName = "Enemy State/Green Snake Idle")]
public class GreenSnakeIdleState : EnemyBaseState
{
    private EnemyController _controller;
    protected IEnumerator _freeMovementState;

    public override void EnterState(EnemyStateManagerScriptableObject _stateManager, EnemyController _controller)
    {
        this._controller = _controller;
        _freeMovementState = MoveFreelyCoroutine();
        _controller._rb.velocity = Vector2.zero;
    }

    public override void UpdateState(EnemyStateManagerScriptableObject _stateManager, EnemyController _controller)
    {
        GreenSnakeController _snakeController = (GreenSnakeController)_controller;
        if (_snakeController._isAttacked)
        {
            _snakeController._isMoving = false;
            _controller.StopCoroutine(MoveFreelyCoroutine());
            _stateManager.ChangeCurrentState(_stateManager._attackingState);
        }
        else
        {
            if (!_snakeController._isMoving)
            {
                _snakeController._isMoving=true;
                _controller.StartCoroutine(MoveFreelyCoroutine());
            }
        }
    }

    private IEnumerator MoveFreelyCoroutine()
    {
        while (true)
        {
            // Generate a random direction for the snake
            Vector2 _freeMoveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

            // Check if the snake hits a wall in the desired direction
            RaycastHit2D wallHit = Physics2D.Raycast(_controller.transform.position, _freeMoveDirection, _controller._enemyStats.wallCheckDistance, _controller._enemyStats.wallLayerMask);
            if (wallHit.collider == null)
            {
                // Move the snake in the specified direction
                _controller._rb.velocity = _freeMoveDirection * _controller._enemyStats.maxSpeed;

                yield return new WaitForSeconds(_controller._enemyStats.freeMovementTime);

                _controller._rb.velocity = Vector2.zero;
            }
            else
            {
                // Check if the snake hits a wall in the opposite direction
                RaycastHit2D oppositeWallHit = Physics2D.Raycast(_controller.transform.position, -_freeMoveDirection, _controller._enemyStats.wallCheckDistance, _controller._enemyStats.wallLayerMask);
                if (oppositeWallHit.collider == null)
                {
                    // Calculate the opposite direction of the wall
                    _freeMoveDirection = Vector2.Reflect(-_freeMoveDirection, oppositeWallHit.normal);

                    // Move the snake in the opposite direction
                    _controller._rb.velocity = _freeMoveDirection * _controller._enemyStats.maxSpeed;

                    yield return new WaitForSeconds(_controller._enemyStats.freeMovementTime);

                    _controller._rb.velocity = Vector2.zero;
                }
            }
            yield return new WaitForSeconds(_controller._enemyStats.freeMovementTime);

        }


    }

}