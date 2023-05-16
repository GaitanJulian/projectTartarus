using System.Collections;
using UnityEngine;

public abstract class SnakeEnemyBase : MonoBehaviour
{
    [SerializeField] protected EnemyStatsScriptableObject _snakeStats;
    protected Transform _player;
    protected Rigidbody2D _rb;
    protected bool _isAggressive = false;
    protected Vector2 _lastAttackDirection;
    protected Vector2 _freeMoveDirection;
    protected const string _playerTag = "Player";
    protected IEnumerator _freeMovementState;
    protected IEnumerator _attackState;
    protected Animator _animator;

    // Animation States
    const string SNAKE_IDLE = "snake_idle";
    const string SNAKE_ATTACK = "snake_attack";

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _freeMovementState = MoveFreelyCoroutine();
        _attackState = AttackCoroutine();
    }

    protected virtual void Start()
    {
        StartCoroutine(_freeMovementState);
        ChangeAnimationState(SNAKE_IDLE);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(_playerTag))
        {
            StopCoroutine(_freeMovementState);
            _player = collision.gameObject.transform;
            _isAggressive = true;
            StartCoroutine(_attackState);
        }
    }

    protected abstract IEnumerator MoveFreelyCoroutine();
    protected abstract IEnumerator AttackCoroutine();

    protected void Attack()
    {
        Debug.Log("Snake attacks in the direction: " + _lastAttackDirection);
        ChangeAnimationState(SNAKE_ATTACK);
    }

    protected void ChangeAnimationState(string newState)
    {
        _animator.Play(newState);
    }
}
