using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContextSteeringDetector : MonoBehaviour
{
    public abstract void Detect(ContextSteeringAIData aiData);
}
