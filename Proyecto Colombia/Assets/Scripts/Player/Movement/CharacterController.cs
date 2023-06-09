using System;
using TMPro.EditorUtilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    private CharacterStatsManager _characterStatsManager;
    private PlayerStatsScriptableObject _otherPlayerStats; // We will reference the player stats scriptable object to call unmutable stats.
    [SerializeField] public Animator _animator;
    private Rigidbody2D _rb;
    private PlayerInputActions _playerControls; // New Input system
    private PlayerHealthTrigger _playerHealthTrigger;
    public InputAction _move; // Input Action for movement
    public InputAction _attack;

    private Vector2 _playerInput;
    private Vector2 _desiredVelocity; // Variable that indicates the max Speed the player can get in any direction
    private Vector2 _currentVelocity; // Current speed in a frame

    const string _animParamHorizontal = "Horizontal", _animParamVertical = "Vertical", _animSpeed = "Speed",  _animParamAttack = "Attack";
    private Vector2 _lastDireciton; // Last direction the player moved at

    public event Action OnInteractionKeyPressed;
    private InputAction _interact;
    Queue<Vector2> _vectorQueue; //last 5 "_playerInput" recorded different than zero

    [SerializeField] bool _drawGizmos;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerControls = new PlayerInputActions();
        _characterStatsManager = GetComponentInChildren<CharacterStatsManager>();
        _playerHealthTrigger = GetComponent<PlayerHealthTrigger>();
        // _animator = GetComponent<Animator>();
    }


    private void Start()
    {
        _vectorQueue = new Queue<Vector2>();
        _vectorQueue.Enqueue(Vector2.zero);
        _otherPlayerStats = (PlayerStatsScriptableObject)_characterStatsManager._characterStats;
    }
    private void OnEnable()
    {
        _move = _playerControls.Player.Move;
        _move.Enable();
        _interact = _playerControls.Player.Interact;
        _interact.Enable();
        _playerHealthTrigger._onDeath.AddListener(OnDeath);
        _attack = _playerControls.Player.Attack;
        _attack.Enable();
    }

    private void OnDisable()
    {
        _move.Disable();
        _interact.Disable();
        _attack.Disable();
        _playerHealthTrigger._onDeath.RemoveListener(OnDeath);
    }

    private void Update()
    {
        _playerInput = _move.ReadValue<Vector2>();

        if (_interact.WasPressedThisFrame())
        {
            OnInteractionKeyPressed?.Invoke();
        }


        if (_attack.WasPressedThisFrame())
        {
            print("funciona");
            _animator.SetTrigger(_animParamAttack);
        }


    }

    private void FixedUpdate()
    {
        // Apply movement
        if (_playerInput != Vector2.zero)
        {
            _desiredVelocity = _playerInput * _characterStatsManager._currentSpeed;
            _currentVelocity = _rb.velocity;
            _rb.velocity = Vector2.Lerp(_currentVelocity, _desiredVelocity, _otherPlayerStats._acceleration * Time.fixedDeltaTime);
            if (_animator != null) _animator.SetFloat(_animSpeed, 1);
            Animate();
            _lastDireciton = _playerInput; // Stores the last direction the player intended to look at

            _vectorQueue.Enqueue(_playerInput);
            if (_vectorQueue.Count > 3) _vectorQueue.Dequeue();
        }
        else
        {
            // if the player is not moving the desired speed is zero
            _desiredVelocity = Vector2.zero;
            _currentVelocity = _rb.velocity;
            _rb.velocity = Vector2.Lerp(_currentVelocity, _desiredVelocity, _otherPlayerStats._deceleration * Time.fixedDeltaTime);
            if (_animator != null) _animator.SetFloat(_animSpeed, 0);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            foreach (Vector2 vector in _vectorQueue)
            {
                Debug.Log(vector);
            }
        }

    }

    private void Animate()
    {

        if (_animator != null) _animator.SetFloat(_animParamHorizontal, _playerInput.x);
        if (_animator != null) _animator.SetFloat(_animParamVertical, _playerInput.y);


    }

    // This function will return the last direction the player was looking at
    public Vector2 ReturnDirection()
    {
        Vector2 toReturn;
        if (_playerInput != Vector2.zero) toReturn = _playerInput;
        else toReturn = _vectorQueue != null ? _vectorQueue.Peek() : Vector2.zero;
        return toReturn;
    }

    private void OnDrawGizmos()
    {
        if (_drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + ReturnDirection());
        }


    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }

}
