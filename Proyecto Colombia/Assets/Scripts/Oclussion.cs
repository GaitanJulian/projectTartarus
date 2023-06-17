using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oclussion : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Transform player;
    public float distance,d;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (player != null)
        {
         d  = Vector2.Distance(transform.position,player.position);
            if (d > distance) { invisible(); }
            else { visible(); }
        }
    }
    private void invisible()
    {
        Debug.Log("no visible");
        DisableAllChildren();
    }

    private void visible()
    {
        EnableAllChildren();
    }

    private void DisableAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void EnableAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}

