using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAiWithContextSteering : MonoBehaviour
{
    [SerializeField] private List<SteeringBehaviour> _steeringBehaviours;
    [SerializeField] private List<ContextSteeringDetector> _detectors;
    [SerializeField] private ContextSteeringAIData _aiData;
    [SerializeField] private ContextSolver _movementDirectionSolver;
    [SerializeField] private float _detectionDelay = 0.01f; //this could be made greater for performance motifs
    [SerializeField] private bool _gizmos = true;

    private void Start()
    {
        InvokeRepeating("PerformDetection", 0, _detectionDelay); //Detecting Player and Obstacles around
    }

    private void PerformDetection()
    {
        foreach (ContextSteeringDetector detector in _detectors)
        {
            detector.Detect(_aiData);
        }

        if (_gizmos)
        {
            float[] danger = new float[8];
            float[] interest = new float[8];

            foreach (SteeringBehaviour behaviour in _steeringBehaviours)
            {
                (danger, interest) = behaviour.GetSteering(danger, interest, _aiData);
            }
        }
    }

    /// <summary>
    /// Returns true if the target (player) is on sight
    /// </summary>
    /// <returns></returns>
    public bool TargetOnSight()
    {
        return _aiData._currentTarget != null;
    }
    /// <summary>
    /// Returns the number of aviable targets
    /// </summary>
    /// <returns></returns>
    public int TargetCount()
    {
        return _aiData.GetTargetsCount();
    }
    /// <summary>
    /// Chooses a target from the aviable ones, normally you will just choose index 0 (the closest)
    /// </summary>
    /// <param name="index"></param>
    public void ChooseTarget(int index)
    {
        _aiData._currentTarget = _aiData._targets[index];
    }
    /// <summary>
    /// Returns the desired direction of moving based in the Context Steering System as vector2
    /// </summary>
    /// <returns></returns>
    public Vector2 GetDirection()
    {
        return _movementDirectionSolver.GetDirectionToMove(_steeringBehaviours, _aiData);
    }
    /// <summary>
    /// Returns a float of the distance between the object and the selected target
    /// </summary>
    /// <returns></returns>
    public float DistanceFromTarget()
    {
        if(_aiData._currentTarget == null)
        {
            return 0f;
        }
        else
        {
            return Vector2.Distance(_aiData._currentTarget.position, transform.position);
        }
    }

    public Transform GetCurrentTarget()
    {
        return _aiData._currentTarget;
    }

    public Vector2 GetSeekedPosition()
    {
        SeekBehaviour seek = (SeekBehaviour)_steeringBehaviours[1];
        return seek.targetPositionCached;
    }
}

//TRASH BUT I'M AFRAID TO ERASE IT:

//[SerializeField] private float attackDistance = 0.5f, aiUpdateDelay = 0.06f, attackDelay = 1f;
//Inputs sent from the Enemy AI to the Enemy controller
//public UnityEvent OnAttackPressed;
//public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
//[SerializeField] private Vector2 movementInput;
//bool following = false;

/*
private void Update()
{
    if (_aiData._currentTarget != null)
    {
        ChaseAndAttack();
        //CALL CHASE AND ATTACK FUNCTION, TURN ON BOOL
    }
    else if (_aiData.GetTargetsCount() > 0)
    {
        _aiData._currentTarget = _aiData._targets[0]; //chooses the closes target (player), to consider when in cooperative?
    }
    //move to "MOVE_VECTOR"
}

//CHASE AND ATTACK:
void ChaseAndAttack()
{
    if (_aiData._currentTarget == null)
    {
        //"MOVE_VECTOR" = VECTOR2.ZERO
        //TURN CORRESPONDING BOOL OFF
    }
    else
    {
        //we have a target
        //calculate distance to it:
        float distance = Vector2.Distance(_aiData._currentTarget.position, transform.position);

        if (distance < 1)//attack distance
        {
            //ATTACK LOGIC
            //"MOVE_VECTOR" = VECTOR2.ZERO?
            //CALL EXTERNAL EVENT TO ATTACK?
            //CALL THIS CHASE AND ATTACK FUNCTION AGAIN
        }
        else
        {
            //CHASE LOGIC... because we are beyond attack distance
            //"MOVE_VECTOR" = 
            Vector2 move = _movementDirectionSolver.GetDirectionToMove(_steeringBehaviours, _aiData);
            //AGAIN, CALL THIS CHASE AND ATTACK FUNCTION AGAIN
        }
    }
}
*/