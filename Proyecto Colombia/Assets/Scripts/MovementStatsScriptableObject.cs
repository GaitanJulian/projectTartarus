using UnityEngine;

[CreateAssetMenu(fileName = "MovementStatsScriptableObject", menuName = "Scriptable objects/Movement Stats")]
public class MovementStatsScriptableObject : ScriptableObject
{ 
    [Header("Movement system")]
    public float maxSpeed;
    public float acceleration;
    public float deceleration;

}
