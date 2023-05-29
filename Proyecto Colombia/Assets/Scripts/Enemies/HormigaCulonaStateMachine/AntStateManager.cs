using UnityEngine;
using DG.Tweening;

public class AntStateManager : MonoBehaviour
{
    AntBaseState _currentState;
    public AntIdleState _idleState = new AntIdleState();
    public AntChasingState _chasingState = new AntChasingState();
    public AntAttackingState _attackState = new AntAttackingState();
    public AntUndergroundState _undergroundState = new AntUndergroundState();
    [SerializeField] public EnemyAiWithContextSteering _contextSteering;
    [SerializeField] GameObject _sprite;
    Rigidbody2D _rb;
    
    [SerializeField] bool _debugState = false;

    [SerializeField]
    float _attackDistance = 1.1f, _moveVelocity = 1.5f, _moveWhileAttackingVelocity = 0.2f,
        _attackWaitTime = 0.1f, _attackMagnitude = 1f, _c = 3f;
    
    int _randomDirection = 1;
    Vector3 _startingPosition;

    #region Getters & Setters
    public float AttackDistance { get { return _attackDistance; } }
    public float MoveVelocity { get { return _moveVelocity; } }
    public float MoveWhileAttackingVelocity { get { return _moveWhileAttackingVelocity; } }
    public float AttackWaitTime { get { return _attackWaitTime; } }
    public float AttackMagnitude { get { return _attackMagnitude; } }
    public bool DebugState { get { return _debugState; } }
    public int RandomDirection { get { return _randomDirection;} }
    public Vector3 StartingPosition { get { return _startingPosition; } }
    public float C { get { return _c; } }
    #endregion

    void Start()
    {
        DOTween.Init();
        _rb = GetComponent<Rigidbody2D>();
        _currentState = _idleState;
        _currentState.EnterState(this, _rb);
        _randomDirection = Random.Range(0, 2) == 0 ? -1 : 1;
    }

    private void Awake()
    {
        _startingPosition = transform.position;
    }

    void FixedUpdate()
    {
        Debug.Log(_rb.velocity);
        float angle = Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg;
        //_sprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _sprite.transform.DOLocalRotateQuaternion(Quaternion.AngleAxis(angle, Vector3.forward), 0.1f);

        _currentState.UpdateState(this, _rb);
        if (_debugState)
        {
            Debug.Log(transform.name + ": " + _currentState);
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
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _attackDistance);
        }
    }
}
