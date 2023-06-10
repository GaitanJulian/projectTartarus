using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openSide;

    //1 Button
    //2 Top
    //3 Left
    //4 Right

    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    void Spawn(){
        if (spawned == false){
            if (openSide == 1){
                //B Door
                rand = Random.Range(0, templates.bottomRooms.Length);

                if (transform.position != Vector3.zero)
                {
                    Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                }

            }
            else if (openSide == 2){
                //T Door
                rand = Random.Range(0, templates.topRooms.Length);

                if (transform.position != Vector3.zero)
                {
                    Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
                }
            }
            else if (openSide == 3){
                //L Door
                rand = Random.Range(0, templates.leftRooms.Length);

                if (transform.position != Vector3.zero)
                {
                    Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
                }
            }
            else if (openSide == 4){
                //R Door
                rand = Random.Range(0, templates.rightRooms.Length);

                if (transform.position != Vector3.zero)
                {
                    Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
                }
            }
            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint")){
            if (other.GetComponent<RoomSpawner>().spawned==false && spawned==false){
                Destroy(this);
            }
            spawned = true;
        }
    }
}
