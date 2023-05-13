using UnityEngine;

[CreateAssetMenu(fileName = "movementStatsScriptableObject", menuName = "Movement Stats")]
public class movementStatsScriptableObject : ScriptableObject
{
    public string characterName;

    [Header("Movement system")]
    public float maxSpeed;
 

}