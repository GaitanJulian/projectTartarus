using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsScriptableObject", menuName = "Enemy Stats")]
public class EnemyStatsScriptableObject : ScriptableObject
{
    [Header("Movement system")]
    public float maxSpeed;
    public float acceleration;
    public float deceleration;
    public float freeMovementTime;
    public float wallCheckDistance;

    [Header("Attack system")]
    public float damage;
    public float attackRange;
    public float attackTime;

    [Header("LayerMasks")]
    public LayerMask playerLayerMask;
    public LayerMask wallLayerMask;
}