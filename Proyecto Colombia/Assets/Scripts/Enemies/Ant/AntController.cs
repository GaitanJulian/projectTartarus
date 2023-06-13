using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AntController : EnemyController
{
    Vector2 _desiredVector;
    [SerializeField] public string _currentState, _substate;
    [SerializeField] bool _debug;
    void Start()
    {
        StartCoroutine(_idleCoroutine);
        _currentState = "Idle";
        //_debug = true;
    }
    
    void Update()
    {
        if (_contextSteering.GetDirection() != null) _desiredVector = _contextSteering.GetDirection();
        AnimationManager();
        if (_debug) DebugFunctions();
    }

    public override IEnumerator IdleState()
    {
        while (true)
        {
            _currentState = "Idle";
            //triggers if enemy is almost static
            if (_rb.velocity.magnitude < 0.1f && !_isWondering)
            {
                _isMoving = false;
                _substate = "I've noticed I'm not moving";
            }
            //gets some variables we're gona use rn
            if (!_isMoving)
            {
                _originPos = transform.position;
                _targetPos = GetPointToWonder(_contextSteering.GetSeekedPosition(), 4, 1.5f);
                _isMoving = true;
                _substate = "get those points";
            }
            //constantly checks the tangent of an animation curve based of a point that correspond to the progress
            //of transiting from the point the enemy was located while doing the calculations and the calculated
            //target point, this is so ir follos an arch trayectory, since the animation curve has an arch shape
            //then, if the destiny is reached, it changes to the next part where it slows down
            if (_isMoving)
            {
                float progress = GetProgressInSegment(_originPos, _targetPos, transform.position);
                _justForGizmosPoint = CalculateClosestPointOnSegment(_originPos, _targetPos, transform.position);
                Vector2 direction = GetTangentOnAnimationCurve(curve, progress);
                _rb.AddForce(AddRotationToVector2(direction.normalized));
                if (progress > 0.95) _isMoving = false;
                _substate = "move in an arch";
            }
            //slows down until it gets down a thresshold where the cicle re-starts and a new poit to move to is calculated
            if (!_isMoving)
            {
                _rb.AddForce(_rb.velocity * -0.75f);
                _substate = "I've reached so I slow down";
            }

            //change state checks:
            if (_contextSteering.TargetCount() > 0)
            {
                _contextSteering.ChooseTarget(0);
                ChangeState(_idleCoroutine, _chasingCoroutine);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    #region Wondering Behaviour

    Vector2 _originPos, _targetPos, _justForGizmosPoint;
    bool _isMoving, _isWondering;
    [SerializeField] AnimationCurve curve;

    /// <summary>
    /// <paramref name="distance"/> must be significantly less than the circle <paramref name="radius"/>
    /// </summary>
    /// <returns></returns>
    Vector2 GetPointToWonder(Vector2 center, float radius, float distance)
    {
        Vector2 point = Utilities.GetRandomPointInCircle(center, radius);
        if (Vector2.Distance(transform.position, point) > distance) return point;
        else return GetPointToWonder(center, radius, distance);
    }

    Vector2 GetTangentOnAnimationCurve(AnimationCurve curve, float t)
    {
        float delta = 0.001f; // Small time difference to calculate the slope
        float t1 = Mathf.Clamp01(t - delta);
        float t2 = Mathf.Clamp01(t + delta);
        // Evaluate the curve at two nearby points
        Vector2 point1 = new Vector2(t1, curve.Evaluate(t1));
        Vector2 point2 = new Vector2(t2, curve.Evaluate(t2));
        // Calculate the slope between the two points (tangent)
        Vector2 tangent = (point2 - point1) / (t2 - t1);
        return tangent;
    }

    private Vector2 AddRotationToVector2(Vector2 vector2D)
    {
        // Get the angle of the GameObject's rotation in radians
        float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        // Calculate the rotated vector using trigonometry
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        float x = vector2D.x * cos - vector2D.y * sin;
        float y = vector2D.x * sin + vector2D.y * cos;
        // Create the rotated vector
        Vector2 rotatedVector = new Vector2(x, y);
        return rotatedVector;
    }

    Vector2 CalculateClosestPointOnSegment(Vector2 pointA, Vector2 pointB, Vector2 pointC)
    {
        // Calculate the direction vector of the line segment AB
        Vector2 segmentDirection = pointB - pointA;
        // Calculate the vector from point A to point C
        Vector2 AC = pointC - pointA;
        // Calculate the dot product between AC and the line segment direction
        float dotProduct = Vector2.Dot(AC, segmentDirection);
        // Calculate the length of the line segment AB
        float segmentLength = segmentDirection.magnitude;
        // Calculate the normalized direction vector of the line segment AB
        Vector2 normalizedSegmentDirection = segmentDirection.normalized;
        // Calculate the projection of AC onto the line segment AB
        float t = Mathf.Clamp01(dotProduct / (segmentLength * segmentLength));
        // Calculate the closest point on the line segment AB
        Vector2 closestPoint = pointA + t * segmentDirection;
        return closestPoint;
    }

    float GetProgressInSegment(Vector2 pointA, Vector2 pointB, Vector2 pointC)
    {
        Vector2 pointInSegment = CalculateClosestPointOnSegment(pointA, pointB, pointC);
        float fragment = Vector2.Distance(pointA, pointInSegment) / Vector2.Distance(pointA, pointB);
        return fragment;
    }
    #endregion

    public override IEnumerator ChaseState()
    {
        while (true)
        {
            _currentState = "Chasing";
            _rb.AddForce(_desiredVector * _enemyStats.maxSpeed);
            _rb.AddForce(_rb.velocity * -0.5f);
            //change state checks:
            if (!_contextSteering.TargetOnSight())
            {
                
                ChangeState(_chasingCoroutine, _idleCoroutine);
            }
            if (_contextSteering.DistanceFromTarget() < _enemyStats.attackRange - 0.2f)
            {
                
                ChangeState(_chasingCoroutine, _attackCoroutine);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public override IEnumerator AttackState()
    {
        while (true)
        {
            _currentState = "Attacking";
            _rb.AddForce(_desiredVector * _enemyStats.maxSpeed * 0.5f);
            //change state checks:
            if (_contextSteering.DistanceFromTarget() > _enemyStats.attackRange + 0.2f || _contextSteering.TargetCount() < 1)
            {
                ChangeState(_attackCoroutine, _chasingCoroutine);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    protected override void Attack()
    {

    }

    protected override void OnDamageTaken(Transform _enemy, float _damage)
    {

    }

    void AnimationManager()
    {
        _animator.SetFloat("Xcomp", _desiredVector.normalized.x);
        _animator.SetFloat("Ycomp", _desiredVector.normalized.y);
        if (Input.GetKeyDown(KeyCode.R)) _animator.SetBool("Fly", !_animator.GetBool("Fly"));
        if (Input.GetKeyDown(KeyCode.G)) _animator.SetTrigger("Attack");
    }

    void DebugFunctions()
    {
        Debug.Log(gameObject.name + "| state: " + _currentState + " | substate: " + _substate);
    }

    private void OnDrawGizmos()
    {
        if (_debug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _enemyStats.attackRange);
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + _rb.velocity.normalized);
            if (_targetPos != null) Gizmos.DrawSphere(_targetPos, 0.3f);
            Gizmos.color = Color.green;
            if (_originPos != null) Gizmos.DrawSphere(_originPos, 0.3f);
            Gizmos.color = Color.cyan;
            if (_justForGizmosPoint != null) Gizmos.DrawSphere(_justForGizmosPoint, 0.3f);
            Gizmos.DrawLine(_originPos, _targetPos);
        }
    }
}