using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private MovementStatsScriptableObject _movementStats;

    private Rigidbody2D _rb;
    private PlayerInputActions _playerControls; // New Input system
    private InputAction _move; // Input Action for movement

    
    private Vector2 _desiredVelocity; // Variable that indicates the max Speed the player can get in any direction
    private Vector2 _currentVelocity; // Current speed in a frame
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

    private void FixedUpdate()
    {
        // Apply movement
        Vector2 _playerInput = _move.ReadValue<Vector2>();
        if (_playerInput != Vector2.zero) 
        {
            _desiredVelocity = _playerInput * _movementStats.maxSpeed;
            _currentVelocity = _rb.velocity;
            _rb.velocity = Vector2.Lerp(_currentVelocity, _desiredVelocity, _movementStats.acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // if the player is not moving the desired speed is zero
            _desiredVelocity = Vector2.zero;
            _currentVelocity = _rb.velocity;
            _rb.velocity = Vector2.Lerp(_currentVelocity, _desiredVelocity, _movementStats.deceleration * Time.fixedDeltaTime);
        }
    }
}
