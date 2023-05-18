using UnityEngine;

public class ContextSteeringObstacleDetector : ContextSteeringDetector
{
    [SerializeField] private float _detectionRadius = 2;
    [SerializeField] private LayerMask _obstacleLayerMask;
    [SerializeField] private bool _showGizmos = true;
    Collider2D[] _colliders;

    public override void Detect(ContextSteeringAIData aiData)
    {
        _colliders = Physics2D.OverlapCircleAll(transform.position, _detectionRadius, _obstacleLayerMask);
        aiData._obstacles = _colliders;
    }

    private void OnDrawGizmos()
    {
        if (_showGizmos == false)
            return;
        if (Application.isPlaying && _colliders != null)
        {
            Gizmos.color = Color.red;
            foreach (Collider2D obstacleCollider in _colliders)
            {
                Gizmos.DrawSphere(obstacleCollider.transform.position, 0.2f);
            }
        }
    }
}