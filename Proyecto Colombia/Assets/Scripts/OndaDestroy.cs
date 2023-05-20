using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndaDestroy : MonoBehaviour
{
     public float distance;
    void Start()
    {
      
        
        StartCoroutine(die(distance));
    }
    IEnumerator die(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
