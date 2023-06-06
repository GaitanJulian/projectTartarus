using System.Collections;
using UnityEngine;


public class BlackSnakeController : PurpleSnakeController
{
    private IEnumerator _evadeCoroutine;
    private bool _isEvading;
    private float _evadingTime;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private float _currentDirection;
    protected override void Awake()
    {
        base.Awake();
        _evadeCoroutine = EvasionState();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _evadingTime = _enemyStats.evasionDuration;
    }

    protected override void Start()
    {
        base.Start();
        _currentDirection = 1;
    }

    protected override void Update()
    {
        base.Update();
        print(_isAttacking);
        // Check if the snake should start evading
        if (!_isEvading && !_isAttacking && Random.value < _enemyStats.evasionChance)
        {
                _isChasing = false;
                ChangeState(_chasingCoroutine, _evadeCoroutine);
        }

        // If in evasion state, keep control of the time
        if (_isEvading)
        {
            _evadingTime -= Time.deltaTime;
        }

    }

    private IEnumerator EvasionState()
    {
        while (true)
        {
            _isAttacking = false;
            _isEvading = true;
            _collider.enabled = false;
            Vector2 direction = _contextSteering.GetDirection();
            Vector2 perpendicular = new Vector2(-direction.y, direction.x);
            Vector2 movement = perpendicular * _enemyStats.evasionSpeed;

            // Perform a raycast to check for obstacles
            RaycastHit2D hit = Physics2D.Raycast(transform.position, movement, movement.magnitude, _enemyStats.wallLayerMask);

            if (hit.collider != null)
            {
                _currentDirection *= -1;
            }

            _rb.velocity = movement * _currentDirection;

            if (_evadingTime <= 0)
            {
                _evadingTime = _enemyStats.evasionDuration;
                _isEvading = false;
                ChangeState(_evadeCoroutine, _chasingCoroutine);
            }
            
            yield return null;
        }

    }
}
