using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
 
    [SerializeField] List<GameObject> enemys, agua;
    [SerializeField] GameObject jarron;

    private void Awake()
    {
        
           int randomwater = Random.Range(0, 4);
            if (randomwater == 2)
            {
                WaterSpawn();
            }
        
        int randJarron = Random.Range(0, 4);
        if (randJarron > 1)
        {
            for (int i = 0; i < randJarron; i++)
            {
                Vector2 pos = new Vector2(Random.Range(-10,12),Random.Range(-10,11));
               GameObject jarrones= Instantiate(jarron, transform);
                jarrones.transform.localPosition = pos;
            }
        }
        int rand = Random.Range(3, 9);
        for (int i = 0; i < rand; i++)
        {
            Vector2 pos = new Vector2(Random.Range(-10, 12), Random.Range(-10, 11));
            int a = Random.Range(0, enemys.Count);
          GameObject enemy= Instantiate(enemys[a], transform);
            enemy.transform.localPosition = pos;
        }
       
    }

    public void WaterSpawn()
    {
        int random = Random.Range(0, agua.Count);
        Vector2 pos = new Vector2(Random.Range(-8, 8), Random.Range(-4, 10));
        GameObject water = Instantiate(agua[random], transform);
        water.transform.localPosition = pos;
    }
}
