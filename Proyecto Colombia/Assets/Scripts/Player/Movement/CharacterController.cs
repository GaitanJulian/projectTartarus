using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private MovementStatsScriptableObject _movementStats;

    private Rigidbody2D _rb;
    private PlayerInputActions _playerControls; // New Input system
    public InputAction _move; // Input Action for movement

    private Vector2 _playerInput;
    private Vector2 _desiredVelocity; // Variable that indicates the max Speed the player can get in any direction
    private Vector2 _currentVelocity; // Current speed in a frame
    private Vector2 _lastDireciton; // Last direction the player moved at
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _move = _playerControls.Player.Move;
        _move.Enable();
    }

    private void OnDisable()
    {
        _move.Disable();
    }

    private void Update()
    {
        _playerInput = _move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Apply movement
        if (_playerInput != Vector2.zero)
        {
            _desiredVelocity = _playerInput * _movementStats.maxSpeed;
            _currentVelocity = _rb.velocity;
            _rb.velocity = Vector2.Lerp(_currentVelocity, _desiredVelocity, _movementStats.acceleration * Time.fixedDeltaTime);
            _lastDireciton = _playerInput; // Stores the last direction the player intended to look at
        }
        else
        {
            // if the player is not moving the desired speed is zero
            _desiredVelocity = Vector2.zero;
            _currentVelocity = _rb.velocity;
            _rb.velocity = Vector2.Lerp(_currentVelocity, _desiredVelocity, _movementStats.deceleration * Time.fixedDeltaTime);
        }
    }

    // This function will return the last direction the player was looking at
    public Vector2 ReturnDirection()
    {
        return _lastDireciton;
    }

}
