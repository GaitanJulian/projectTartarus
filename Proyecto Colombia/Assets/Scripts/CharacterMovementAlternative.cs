using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovementAlternative : MonoBehaviour
{
    [SerializeField] private MovementStatsScriptableObject movementStats;

    private Rigidbody2D rb;
    private PlayerInputActions playerControls; // New Input system
    private InputAction move; // Input Action for movement

    
    private Vector2 desiredVelocity; // Variable that indicates the max Speed the player can get in any direction
    private Vector2 currentVelocity; // Current speed in a frame
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 playerInput = move.ReadValue<Vector2>();
        if (playerInput != Vector2.zero) 
        {
            desiredVelocity = playerInput * movementStats.maxSpeed;
            currentVelocity = rb.velocity;
            rb.velocity = Vector2.Lerp(currentVelocity, desiredVelocity, movementStats.acceleration * Time.fixedDeltaTime);
        }
        else
        {
            desiredVelocity = Vector2.zero;
            currentVelocity = rb.velocity;
            rb.velocity = Vector2.Lerp(currentVelocity, desiredVelocity, movementStats.deceleration * Time.fixedDeltaTime);
        }
    }
}
