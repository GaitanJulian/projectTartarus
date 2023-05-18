using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleSnake : GreenSnake
{
    [SerializeField] private float _detectionRange = 3f;
    private bool playerFound = false;
    private bool currentAttacking = false;
    

    private void Update()
    {
        // Check if there is no current target or the current target has exited the circle
        if (_playerTransform == null || !IsPlayerWithinRange(_playerTransform))
        {
            // Find the closest player within the detection range
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _detectionRange, _snakeStats.playerLayerMask);

            float closestDistance = Mathf.Infinity;
            foreach (Collider2D collider in colliders)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, collider.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    _playerTransform = collider.transform;
                    playerFound = true;
                }
            }
        }
     
        if (!IsPlayerWithinRange(_playerTransform) && playerFound)
        {
            playerFound = false;
            _isAggressive = false;
            _playerTransform = null;
            currentAttacking = false;
            StopCoroutine(_attackState);
            ChangeAnimationState(SNAKE_IDLE);
            StartCoroutine(_freeMovementState);
        }

        // Attack the target player if one is found
        if (_playerTransform != null && !currentAttacking)
        {
            currentAttacking = true;
            StopCoroutine(_freeMovementState);
            _isAggressive = true;
            StartCoroutine(_attackState);
        }
    }

    private bool IsPlayerWithinRange(Transform playerTransform)
    {
        if (playerTransform == null)
            return false;

         // Check if the player is within the detection range
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        return distanceToPlayer <= _detectionRange;
        
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere gizmo to represent the detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }

}
