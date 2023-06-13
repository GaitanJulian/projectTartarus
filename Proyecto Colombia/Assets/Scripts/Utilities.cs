using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public static bool IsObjectInLayerMask(GameObject obj, LayerMask layerMask)
    {
        int objLayer = obj.layer;
        int layerMaskValue = layerMask.value;

        return (layerMaskValue & (1 << objLayer)) != 0;
    }

    public static Vector2 GetRandomPointInCircle(Vector2 center, float radius)
    {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float randomRadius = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;
        float x = center.x + randomRadius * Mathf.Cos(randomAngle);
        float y = center.y + randomRadius * Mathf.Sin(randomAngle);

        return new Vector2(x, y);
    }
}