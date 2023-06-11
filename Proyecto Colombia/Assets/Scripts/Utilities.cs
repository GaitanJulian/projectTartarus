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
}