using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHealthDrops : MonoBehaviour
{
    public GameObject heartPrefab; // Prefab of the Heart object
    public float dropProbability; // Probability of dropping a Heart (between 0 and 1)

    private void OnDestroy()
    {
        #if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) // Prevents this function to be active in the editor mode
                    return;
        #endif

        if (Random.value <= dropProbability)
        {
            Instantiate(heartPrefab, transform.position, Quaternion.identity);
        }
    }
}
