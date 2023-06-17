using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oclussion : MonoBehaviour
{
    private  SpriteRenderer spriteRenderer; 

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnBecameInvisible()
    {
        Debug.Log("no visible");
        DisableAllChildren();
    }

    private void OnBecameVisible()
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
