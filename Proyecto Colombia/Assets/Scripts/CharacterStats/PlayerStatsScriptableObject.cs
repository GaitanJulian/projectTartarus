using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsScriptableObject", menuName = "Stats/Player Stats")]
public class PlayerStatsScriptableObject : CharacterStatsScriptableObject
{
    [Header("Movement system")]
    public float _acceleration;
    public float _deceleration;
}
