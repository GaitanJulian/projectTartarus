using System.Collections;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    [SerializeField] private CharacterStatsScriptableObject _characterStats;
    private Damagable _damageableScript;
    private SpriteRenderer _spriteRenderer;

    public float _currentSpeed; // Store the current speed of the character
    public float _currentAttackDamage; // Store the current attack damage of the character
    public float _currentAttackRange; // Store the current attack range of the character
    private Color _originalColor; // Store the original color of the sprite


    public bool _isParalized;
    public bool _isStunned;

    private void Awake()
    {
        _damageableScript = GetComponent<Damagable>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

    private IEnumerator StunCoroutine(float duration)
    {
        _isStunned = true;
        _currentAttackRange = 0f;
        _currentSpeed = 0f;
        // Change sprite color to indicate stun
        _spriteRenderer.color = Color.gray;

        yield return new WaitForSeconds(duration);

        _currentAttackRange = _characterStats._attackRange;
        _currentSpeed = _characterStats._maxSpeed;
        _isStunned = false;

        // Reset sprite color to original color
        _spriteRenderer.color = _originalColor;

    }

}
