
using UnityEngine;

public abstract class CharacterStatsScriptableObject : ScriptableObject
{
    public float _maxSpeed;
    public float _damage;
    public float _attackRange;

    [Header("LayerMasks")]
    public LayerMask _playerLayerMask;
    public LayerMask _wallLayerMask;
    public LayerMask _enemyLayerMask;
}
