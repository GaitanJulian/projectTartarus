using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAndOffInventory : MonoBehaviour
{
    [SerializeField] GameObject y;
    bool x;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (x)
            {
                y.SetActive(false);
                x = false;
            }
            else
            {
                y.SetActive(true);
                x = true;
            }
        }

    }
}
