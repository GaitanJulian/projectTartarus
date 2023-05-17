using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSteeringAIData : MonoBehaviour
{
    public List<Transform> _targets = null;
    public Collider2D[] _obstacles = null;
    public Transform _currentTarget;
    
    public int GetTargetsCount() => _targets == null ? 0 : _targets.Count;
}
