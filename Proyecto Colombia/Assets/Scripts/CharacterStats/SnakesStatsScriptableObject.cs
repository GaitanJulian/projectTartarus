using UnityEngine;

[CreateAssetMenu(fileName = "SnakeStatsScriptableObject", menuName = "Stats/Snake Stats")]
public class SnakesStatsScriptableObject : CharacterStatsScriptableObject
{
    [Header("Movement system")]
    public float _freeMovementTime;
    public float _wallCheckDistance;

    [Header("Evade Mechanic")]
    public float _evasionSpeed;
    public float _evasionChance;
    public float _evasionDuration;
    public float _evasionCooldown;
}
