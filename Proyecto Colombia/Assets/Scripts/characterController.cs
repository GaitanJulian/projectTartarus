using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private MovementStatsScriptableObject movementStats;

    private PlayerInputActions playerControls;

    private float timeCounter;
    private float aceleration;


    private InputAction move;

    private void Awake()
    {
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

    // Start is called before the first frame update
    void Start()
    {
        timeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerInput = move.ReadValue<Vector2>();
        Vector3 movement = new Vector3(playerInput.x, playerInput.y, 0);
        movement.Normalize();
        if (movement != Vector3.zero) 
        {      
            if (timeCounter >= 5)
            {
                timeCounter = 5;
            }
            else
            {
                timeCounter += Time.deltaTime * 5;
            }

            aceleration = acelerationModifier(timeCounter);
            transform.position += movement * aceleration * movementStats.maxSpeed * Time.deltaTime;

        }
        else
        {
            timeCounter = 0;
        }



    }



    private float acelerationModifier(float time)
    {

        return -Mathf.Exp(-time) + 1;
    }


}
