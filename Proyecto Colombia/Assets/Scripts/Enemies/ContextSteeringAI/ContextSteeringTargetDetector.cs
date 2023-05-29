using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSteeringTargetDetector : ContextSteeringDetector
{
    [SerializeField] private float _targetDetectionRange = 5;
    [SerializeField] private LayerMask _obstaclesLayerMask, _playerLayerMask;
    [SerializeField] private bool _showGizmos = true;
    //gizmo parameters
    private List<Transform> _colliders;

    public override void Detect(ContextSteeringAIData aiData)
    {
        //Find out if player is close to this object
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, _targetDetectionRange, _playerLayerMask);

        if (playerCollider != null)
        {
            //Check if player is in sight
            Vector2 direction = (playerCollider.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _targetDetectionRange, _obstaclesLayerMask); //obstacles layer mask must also include the player

            //Make sure that the collider we see is on the "_playerLayerMask" layer
            if (hit.collider != null && (_playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Debug.DrawRay(transform.position, direction * _targetDetectionRange, Color.magenta);
                _colliders = new List<Transform>() { playerCollider.transform };
            }
            else
            {
                _colliders = null; //If the object wasn't the player
            }
        }
        else
        {
            _colliders = null; //If nothing was detected
        }
        aiData._targets = _colliders;
    }

    private void OnDrawGizmosSelected()
    {
        if (_showGizmos == false)
            return;

        Gizmos.DrawWireSphere(transform.position, _targetDetectionRange);
        //UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, _targetDetectionRange);

        if (_colliders == null)
            return;
        Gizmos.color = Color.magenta;
        foreach (var item in _colliders)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }
    }
}
