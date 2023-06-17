using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
 
    public GameObject closedRoom;

    public List<GameObject> rooms, dialogos;

    public GameObject boss;
    public GameObject simpleEnemy;


    private void Start()
    {
        Invoke("SpawnEnemy", 3f);
    }

    void SpawnEnemy()
    {
        Instantiate(boss, rooms[rooms.Count - 1].transform.position, Quaternion.identity);
        Debug.Log("rooms" + rooms.Count);
        int rand = Random.Range(2, rooms.Count - 1);
        int p = 0;
        Debug.Log("rand:" + rand);
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            if (i >= rand - 1 && i <= rand + 1)
            {
            
                GameObject dialogo = Instantiate(dialogos[p], rooms[i].transform);
                dialogo.transform.localPosition = new Vector2(0, 0);
                p++;
            }

           GameObject simple= Instantiate(simpleEnemy, rooms[i].transform);
            if (p == 1) { simple.GetComponent<spawnerScript>().WaterSpawn(); }
            simple.transform.localPosition = new Vector2(0, 0);
         
        }
    }
}
