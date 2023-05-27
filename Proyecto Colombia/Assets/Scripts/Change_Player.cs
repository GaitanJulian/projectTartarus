using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Events;


public class Change_Player : MonoBehaviour
{
    static public UnityAction<Transform> action;
    [SerializeField] GameObject[] players;
     Transform pos;
    [SerializeField] Camera camera;

     
    public void spawnplayer(int i)
    {
       
        foreach (var a in players)
        {
            if (a.activeSelf)
            {
                pos= a.transform;
            }
            a.SetActive(false);
        }
        players[i].SetActive(true);
        EventManager.Dispatch(ENUM_Player.alterHitpoints, 100f);
        players[i].transform.position = pos.position;
        camera.transform.parent = players[i].transform;
        action?.Invoke(players[i].transform); 
    }

}
