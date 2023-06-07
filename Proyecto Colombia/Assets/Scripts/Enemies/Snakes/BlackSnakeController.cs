using System.Collections;
using UnityEngine;


public class BlackSnakeController : PurpleSnakeController
{
    private IEnumerator _evadeCoroutine;
    private bool _isEvading;
    private float _evadingTime;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private float _currentEvasionDirection; // a variable that helps to define the direction of the evasion movement
    private float _changeDirectionCooldown; // Keep control of the change of direction cooldown to prevent the enemy from jittering
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
        _currentEvasionDirection = 1;
        _changeDirectionCooldown = _enemyStats.evasionCooldown;
    }

    protected override void Update()
    {
        base.Update();
        // Check if the snake should start evading
        if (!_isEvading && ShouldTriggerEvasion())
        {
                _isChasing = false;
                ChangeState(_chasingCoroutine, _evadeCoroutine);
        }

        // If in evasion state, keep control of the time
        if (_isEvading)
        {
            _evadingTime -= Time.deltaTime;
        }

        
        if (_changeDirectionCooldown > 0) 
        {
            _changeDirectionCooldown -= Time.deltaTime;
        }

    }

    bool ShouldTriggerEvasion()
    {
        // Generate a random probability
        float randomProbability = Random.Range(0f, 1f);

        // Check if the random probability is less than or equal to the mean probability
        return randomProbability <= _enemyStats.evasionChance;
    }

    private IEnumerator EvasionState()
    {
        while (true)
        {
            _isEvading = true;
            _collider.enabled = false;
            _spriteRenderer.material.color = new Color(1f, 1f, 1f, 0.5f);
            Vector2 direction = _contextSteering.GetDirection();
            Vector2 perpendicular = new Vector2(-direction.y, direction.x);
            Vector2 movement = perpendicular * _enemyStats.evasionSpeed;

            // Perform a raycast to check for obstacles
            RaycastHit2D hit = Physics2D.Raycast(transform.position, movement, movement.magnitude, _enemyStats.wallLayerMask);

            if (hit.collider != null && _changeDirectionCooldown <= 0 )
            {
                _currentEvasionDirection *= -1;
                _changeDirectionCooldown = _enemyStats.evasionCooldown;

            }

            _rb.velocity = movement * _currentEvasionDirection;

            if (_evadingTime <= 0)
            {
                _evadingTime = _enemyStats.evasionDuration;
                _isEvading = false;
                _spriteRenderer.material.color = new Color(1f, 1f, 1f, 1f);
                ChangeState(_evadeCoroutine, _chasingCoroutine);
            }
            
            yield return null;
        }

    }
}
