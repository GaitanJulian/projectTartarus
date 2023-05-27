using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSteeringAIData : MonoBehaviour
{
    public List<Transform> _targets = null;
    public Collider2D[] _obstacles = null;
    public Transform _currentTarget;

    private void Start()
    {
        Change_Player.action+= setTarget;
    }

    private void setTarget(Transform arg)
    {
        _currentTarget = arg;
    }

    public int GetTargetsCount() => _targets == null ? 0 : _targets.Count;
}
