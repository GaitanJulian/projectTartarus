using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsScriptableObject", menuName = "Enemy Stats")]
public class EnemyStatsScriptableObject : ScriptableObject
{
    [Header("Movement system")]
    public float maxSpeed;
    public float acceleration;
    public float deceleration;

    [Header("Attack system")]
    public float damage;

}