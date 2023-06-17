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
            int a = Random.Range(0, enemys.Count);
            Instantiate(enemys[a], transform);
        }
        Destroy(this);
    }

}
