using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
 
    [SerializeField] List<GameObject> enemys;
    private void Awake()
    {
        int rand = Random.Range(3, 9);
        for (int i = 0; i < rand; i++)
        {
            Instantiate(enemys[i], transform);
        }
        Destroy(this);
    }

}
