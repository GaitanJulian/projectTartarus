using System.Collections;
using UnityEngine;


public class BlackSnakeController : PurpleSnakeController
{
    private IEnumerator _evadeCoroutine;

    protected override void Start()
    {
        base.Start();
        _evadeCoroutine = RandomEvade();
    }

    protected override void Update()
    {
        base.Update();

        if (_isAttacking)
        {
            float randomValue = Random.value;

            // Check if the random value is less than the evade chance
            if (randomValue < _enemyStats.evadeChance)
            {
                // Perform the evade coroutine
                _isAttacking = false;
                ChangeState(_attackCoroutine, _evadeCoroutine);
            }
           
        }
    }

    IEnumerator RandomEvade()
    {
        Transform _playerTransform = _contextSteering.GetCurrentTarget();
        // Store the initial position and rotation of the snake
        Vector2 initialPosition = _rb.position;
        Quaternion initialRotation = transform.rotation;
         
        // Calculate the target position for the evasive arc movement
        Vector2 targetPosition =  new Vector2(_playerTransform.position.x, _playerTransform.position.y) ; // Implement this method to determine the target position

        // Rotate the snake slightly in the opposite direction to telegraph the movement
        Quaternion telegraphRotation = Quaternion.LookRotation(Vector3.forward, targetPosition - initialPosition);
        Quaternion oppositeRotation = Quaternion.Inverse(telegraphRotation);
        float telegraphDuration = 0.5f;
        float telegraphTimer = 0f;

        while (telegraphTimer < telegraphDuration)
        {
            // Rotate the snake gradually towards the telegraph rotation
            transform.RotateAround(_playerTransform.position, Vector3.forward, _enemyStats.rotationSpeed * Time.deltaTime);

            // Rotate the snake around the player position
            transform.rotation = Quaternion.Lerp(initialRotation, oppositeRotation, telegraphTimer / telegraphDuration);

            telegraphTimer += Time.deltaTime;
            yield return null;
        }

        // Reset the rotation to the initial rotation
        transform.rotation = initialRotation;

        // Move the snake in an arc-shaped path around the player
        float arcDuration = 1.0f;
        float arcTimer = 0f;

        while (arcTimer < arcDuration)
        {
            // Calculate the current position along the arc path
            float t = arcTimer / arcDuration;
            Vector2 currentPos = CalculateArcPosition(initialPosition, targetPosition, t); // Implement this method to calculate the current position along the arc

            // Rotate the snake around the player position
            transform.RotateAround(_playerTransform.position, Vector3.forward, _enemyStats.rotationSpeed * Time.deltaTime);

            // Move the snake by setting the position
            _rb.MovePosition(currentPos);

            arcTimer += Time.deltaTime;
        }

        // Transition to AttackState
        ChangeState(_evadeCoroutine, _attackCoroutine);
    }

    private Vector2 CalculateArcPosition(Vector2 initialPosition, Vector2 targetPosition, float t)
    {
        // Calculate the center point of the arc as the average of the initial and target positions
        Vector2 centerPoint = (initialPosition + targetPosition) * 0.5f;

        // Calculate the radius of the arc as the distance between the center point and the initial position
        float radius = Vector2.Distance(centerPoint, initialPosition);

        // Calculate the angle based on the current time value 't' and the total angle of the arc
        float angle = Mathf.Lerp(0f, Mathf.PI, t);

        // Calculate the current position along the arc using polar coordinates
        float x = centerPoint.x + radius * Mathf.Cos(angle);
        float y = centerPoint.y + radius * Mathf.Sin(angle);

        return new Vector2(x, y);
    }

}
