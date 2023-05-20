using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AntStateManager : MonoBehaviour
{
    AntBaseState _currentState;
    public AntIdleState _idleState = new AntIdleState();
    public AntChasingState _chasingState = new AntChasingState();
    public AntAttackingState _attackState = new AntAttackingState();
    public AntUndergroundState _undergroundState = new AntUndergroundState();
    [SerializeField] public EnemyAiWithContextSteering _contextSteering;
    Rigidbody2D _rb;
    [SerializeField] bool _debugState = false;

    [SerializeField]
    float _attackDistance = 1.1f, _moveVelocity = 1.5f, _moveWhileAttackingVelocity = 0.2f,
        _attackWaitTime = 0.1f, _attackMagnitude = 1f;

    #region Getters & Setters
    public float AttackDistance { get { return _attackDistance; } }
    public float MoveVelocity { get { return _moveVelocity; } }
    public float MoveWhileAttackingVelocity { get { return _moveWhileAttackingVelocity; } }
    public float AttackWaitTime { get { return _attackWaitTime; } }
    public float AttackMagnitude { get { return _attackMagnitude; } }
    public bool DebugState { get { return _debugState; } }
    #endregion

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _currentState = _idleState;
        _currentState.EnterState(this, _rb);
    }

    void FixedUpdate()
    {
        _currentState.UpdateState(this, _rb);
        if (_debugState)
        {
            Debug.Log(transform.name + ": " + _currentState);
            //GIZMOS:

        }

    }

    public void SwitchState(AntBaseState state)
    {
        _currentState = state;
        state.EnterState(this, _rb);
    }


    private void OnDrawGizmosSelected()
    {
        if (_debugState)
        {
            //UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, _attackDistance);
        }
    }
}
