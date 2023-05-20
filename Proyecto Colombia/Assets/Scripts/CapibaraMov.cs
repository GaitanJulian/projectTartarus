using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapibaraMov : MonoBehaviour
{
    public GameObject capibara;
    bool navigation = false;
   
    public Canvas canvas;
   
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Water"))
        {
            gameObject.GetComponent<Collider2D>().isTrigger = false;
            Destroy(transform.GetChild(1).gameObject);
            navigation = false;

        }
    }
    private void message()
    {
        canvas.gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Water"))
        {

            message();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        canvas.gameObject.SetActive(false);
    }

    void Update()
    {
            if (Input.GetKeyDown(KeyCode.M)&& canvas.isActiveAndEnabled){ si(); }
    }
   void si()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = true;
        canvas.gameObject.SetActive(false);
        GameObject capibaraobject = Instantiate(capibara, transform.position, transform.rotation);
        capibaraobject.transform.position = new Vector2(capibaraobject.transform.position.x, capibaraobject.transform.position.y - 0.5f);
        
        capibaraobject.transform.SetParent(transform);
        navigation = true;
    }
    
}
