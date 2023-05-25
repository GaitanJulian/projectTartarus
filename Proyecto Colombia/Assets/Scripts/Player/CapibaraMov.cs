using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CapibaraMov : MonoBehaviour
{
    public GameObject capibara;
    bool navigation = false;
    PlayerInputActions playerInput;
    InputAction nav;

    public Canvas canvas;
    private void Awake()
    {
        playerInput = new PlayerInputActions();
    }
    private void OnEnable()
    {
        nav = playerInput.Player.Swim;
        nav.Enable();
    }
    private void OnDisable()
    {
        nav.Disable();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Water"))
        //cuando deja de colisionar con el objeto que tenga el tag de water llama la funcion para ocultar el capibara
        {
            salirAgua();

        }
    }

    private void salirAgua()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = false;
        capibara.SetActive(false);
        navigation = false;
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
       
            if (nav.ReadValue<float>() > 0&& canvas.isActiveAndEnabled){ si(); }
    }
   void si()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = true;
        canvas.gameObject.SetActive(false);
        capibara.SetActive(true);
        /*instanciar capibara en el player
        GameObject capibaraobject = Instantiate(capibara, transform.position, transform.rotation);
         capibaraobject.transform.position = new Vector2(capibaraobject.transform.position.x, capibaraobject.transform.position.y - 0.5f);

         capibaraobject.transform.SetParent(transform);*/
        navigation = true;
    }
    
}
