using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsScriptableObject", menuName = "Scriptable objects/Enemy Stats")]
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

    [Header("Evade Mechanic")]
    public float rotationSpeed;
    public float evadeChance;

    [Header("LayerMasks")]
    public LayerMask playerLayerMask;
    public LayerMask wallLayerMask;

    
}