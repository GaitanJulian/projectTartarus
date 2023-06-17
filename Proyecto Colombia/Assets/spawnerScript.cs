using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
 
    [SerializeField] List<GameObject> enemys;
    [SerializeField] GameObject jarron;
    private void Awake()
    {
        int randJarron = Random.Range(0, 4);
        if (randJarron > 1)
        {
            for (int i = 0; i < randJarron; i++)
            {
                Vector2 pos = new Vector2(transform.position.x +Random.Range(-10,12),transform.position.y+Random.Range(-10,11));
                Instantiate(jarron, pos,Quaternion.identity);
            }
        }
        int rand = Random.Range(3, 9);
        for (int i = 0; i < rand; i++)
        {
            int a = Random.Range(0, enemys.Count);
          GameObject enemy= Instantiate(enemys[a], transform);
            enemy.transform.localPosition = new Vector2(0, 0);
        }
       
    }

}
