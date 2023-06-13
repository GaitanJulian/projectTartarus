using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover2D : MonoBehaviour
{

    public AnimationCurve curve; // The curve that defines the arch
    float timer;
    float amount = 0.001f;
    [SerializeField] GameObject start, end;

    private void Start()
    {
        timer = 0;

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.O)) timer += amount;
        if (Input.GetKey(KeyCode.L)) timer -= amount;
        //Debug.Log(timer + " | " + GetTangentOnCurve(timer).y);
        //Debug.Log(CalculateProgressOnLine(start.transform.position, end.transform.position, gameObject.transform));
    }

    private Vector2 GetTangentOnCurve(float t)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Vector2 thing = GetTangentOnCurve(timer).normalized;
        //Gizmos.DrawLine(transform.position, (Vector2)transform.position + AddRotationToVector2D(thing));
        Vector2 pointer = CalculateClosestPointOnSegment(start.transform.position, end.transform.position, transform.position);

        Gizmos.DrawSphere(pointer, 0.2f);
    }


    private Vector2 AddRotationToVector2D(Vector2 vector2D)
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

    private float CalculateProgressOnLine(Vector2 start, Vector2 target, Transform toFollow)
    {
        // Calculate the distance between the start and target positions
        float totalDistance = Vector2.Distance(start, target);
        // Calculate the distance between the start and objectToTrack positions
        float objectDistance = Vector2.Distance(start, toFollow.position);
        // Calculate the progress based on the distances
        float progress = objectDistance / totalDistance;
        // Clamp the progress between 0 and 1
        progress = Mathf.Clamp01(progress);
        return progress;
    }

    private Vector2 CalculatePerpendicularPoint(Vector2 pointA, Vector2 pointB, Vector2 pointC)
    {
        // Calculate the direction vector of the line segment AB
        Vector2 segmentDirection = pointB - pointA;

        // Calculate the vector from point A to point C
        Vector2 AC = pointC - pointA;

        // Calculate the dot product between AC and the line segment direction
        float dotProduct = Vector2.Dot(AC, segmentDirection);

        // Calculate the normalized direction vector of the line segment AB
        Vector2 normalizedSegmentDirection = segmentDirection.normalized;

        // Calculate the projection of AC onto the line segment AB
        Vector2 projection = normalizedSegmentDirection * dotProduct;

        // Calculate the perpendicular point on the line segment AB
        Vector2 perpendicularPoint = pointA + projection;

        // Clamp the perpendicular point to ensure it is within the line segment AB
        perpendicularPoint = Vector2.ClampMagnitude(perpendicularPoint - pointA, segmentDirection.magnitude) + pointA;

        return perpendicularPoint;
    }

    private Vector2 CalculateClosestPointOnSegment(Vector2 pointA, Vector2 pointB, Vector2 pointC)
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
}
