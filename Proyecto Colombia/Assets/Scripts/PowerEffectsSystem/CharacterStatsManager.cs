using System.Collections;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    public CharacterStatsScriptableObject _characterStats;
    private Damageable _damageableScript;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor; // Store the original color of the sprite

    [HideInInspector] public float _currentSpeed; // Store the current speed of the character
    [HideInInspector] public float _currentAttackDamage; // Store the current attack damage of the character
    [HideInInspector] public float _currentAttackRange; // Store the current attack range of the character
    [HideInInspector] public bool _isPoissoned;
    [HideInInspector] public bool _isStunned;

    private void Awake()
    {
        _damageableScript = transform.parent.GetComponent<Damageable>(); // The damageable script is in the father
        _spriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>(); // The Sprite renderer is a children
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentSpeed = _characterStats._maxSpeed;
        _currentAttackDamage = _characterStats._damage;
        _currentAttackRange = _characterStats._attackRange;
        _originalColor = _spriteRenderer.color;
    }


    public void ApplyStun(float duration)
    {
        if (!_isStunned)
        {
            StartCoroutine(StunCoroutine(duration));

        }
    }

    public void ApplyDamageOverTime(float damageAmount, float interval, float duration)
    {
        if(!_isPoissoned)
        {
            StartCoroutine(DamageOverTimeCoroutine(damageAmount, interval, duration));
        }
    }

    private IEnumerator StunCoroutine(float duration)
    {
        _isStunned = true;
        _currentAttackRange = 0f;
        _currentSpeed = 0f;
        _damageableScript.SetDamageMultiplier(2f);
        // Change sprite color to indicate stun
        _spriteRenderer.color = Color.gray;

        yield return new WaitForSeconds(duration);

        _currentAttackRange = _characterStats._attackRange;
        _currentSpeed = _characterStats._maxSpeed;
        _isStunned = false;
        _damageableScript.SetDamageMultiplier(1f);
        // Reset sprite color to original color
        _spriteRenderer.color = _originalColor;

    }

    private IEnumerator DamageOverTimeCoroutine(float damageAmount, float interval, float duration)
    {
        _isPoissoned = true;
        _currentSpeed *= 0.8f; // Reduction of 20% to the speed
        _currentAttackDamage *= 0.8f; // Reduction of 20% to the attack damage
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            yield return new WaitForSeconds(interval);

            // Apply damage to the player
            _damageableScript.GetDamaged(damageAmount);

            // Change sprite color to indicate damage
            _spriteRenderer.color = Color.red;

            // Wait for a short duration to create the "Poisson" effect
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));

            // Reset sprite color to original color
            _spriteRenderer.color = _originalColor;

            elapsedTime += interval;
        }

        _isPoissoned = false;
        _currentSpeed = _characterStats._maxSpeed;
        _currentAttackDamage = _characterStats._damage;
    }

}
