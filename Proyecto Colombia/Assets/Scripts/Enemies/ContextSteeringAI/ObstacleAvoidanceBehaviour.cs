using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBehaviour : SteeringBehaviour
{
    [SerializeField] private float _radius = 2f; //the same radius of obstacle detection
    [SerializeField] private float _agentColliderSize = 0.6f; //the size of the object collider, to avoid obstacles at all cost
    //gizmo parameters   
    [SerializeField] private bool _showGizmo = true;
    float[] _dangersResultTemp = null;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, ContextSteeringAIData aiData)
    {
        foreach (Collider2D obstacleCollider in aiData._obstacles)
        {
            Vector2 directionToObstacle = obstacleCollider.ClosestPoint(transform.position) - (Vector2)transform.position;
            float distanceToObstacle = directionToObstacle.magnitude;

            //calculate weight based on the distance between this object and the obstacle
            float weight = distanceToObstacle <= _agentColliderSize ? 1 : (_radius - distanceToObstacle) / _radius;

            Vector2 directionToObstacleNormalized = directionToObstacle.normalized;

            //Add obstacle parameters to the danger array
            for (int i = 0; i < Directions.eightDirections.Count; i++)
            {
                float result = Vector2.Dot(directionToObstacleNormalized, Directions.eightDirections[i]);

                float valueToPutIn = result * weight;

                //override value only if it is higher than the current one stored in the danger array
                if (valueToPutIn > danger[i])
                {
                    danger[i] = valueToPutIn;
                }
            }
        }
        _dangersResultTemp = danger;
        return (danger, interest);
    }

    private void OnDrawGizmos()
    {
        if (_showGizmo == false)
            return;

        if (Application.isPlaying && _dangersResultTemp != null)
        {
            if (_dangersResultTemp != null)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < _dangersResultTemp.Length; i++)
                {
                    Gizmos.DrawRay(
                        transform.position,
                        Directions.eightDirections[i] * _dangersResultTemp[i] * 2
                        );
                }
            }
        }
    }
}

public static class Directions
{
    public static List<Vector2> eightDirections = new List<Vector2>{
            new Vector2(0,1).normalized,
            new Vector2(1,1).normalized,
            new Vector2(1,0).normalized,
            new Vector2(1,-1).normalized,
            new Vector2(0,-1).normalized,
            new Vector2(-1,-1).normalized,
            new Vector2(-1,0).normalized,
            new Vector2(-1,1).normalized
        };
}
