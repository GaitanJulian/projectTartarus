using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrumAttack : MonoBehaviour
{
    //SpriteRenderer _spriteRenderer;
    //PolygonCollider2D _polygonCollider;
    [SerializeField][Range(0f, 1f)] float _heightRelation = 0.5f; 
    Vector3 _proportions;
    [SerializeField] float _startingSize = 0f, _maxSize = 3f, _growSpeed = 10f, _permanenceTime = 0.1f, _cooldown = 0f, _inputBufferTime = 0.2f;
    float _size, _inputBufferCounter;
    bool _ableToAttack = true;
    PlayerInputActions playerInput;
    InputAction attack;
   
    private void Awake()
    {
        playerInput = new PlayerInputActions();
    }
    private void OnEnable()
    {
        attack = playerInput.Player.Attack;
        attack.Enable();
    }
    private void OnDisable()
    {
        attack.Disable();
    }
    private void Start()
    {
        //_spriteRenderer = GetComponent<SpriteRenderer>();
        //_polygonCollider = GetComponent<PolygonCollider2D>();
        _proportions = new Vector3(1, _heightRelation, 1);
        transform.localScale = _proportions * _startingSize;
        _size = _startingSize;
    }

    private void Update()
    {
        _proportions = new Vector3(1, _heightRelation, 1); // REMOVE THIS LINE WHEN WE ARE HAPPY WITH THE ELIPSE SHAPE
        _inputBufferCounter = attack.ReadValue<float>() > 0 ? _inputBufferTime : 
            (_inputBufferCounter > 0 ? _inputBufferCounter - Time.deltaTime : 0);

        if (_ableToAttack && _inputBufferCounter > 0)
        {
            _ableToAttack = false;
            StartCoroutine(GrowArea());
        }
    }

    IEnumerator GrowArea()
    {
        while (_size < _maxSize)
        {
            _size += Time.deltaTime * _growSpeed;
            transform.localScale = _proportions * _size;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(_permanenceTime);
        _size = _startingSize;
        transform.localScale = _proportions * _size;
        yield return new WaitForSeconds(_cooldown);
        _ableToAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Damagable>() != null)
        {
            collision.gameObject.GetComponent<Damagable>().GetDamaged();
        }
    }
}
