using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CapibaraMov : MonoBehaviour
{
    [SerializeField] GameObject capibara, TxInfo;
    private bool navigation = false;
    PlayerInputActions playerInput;
    InputAction nav;
    CharacterController character;
    Animator animator;

    private void Awake()
    {
        character = gameObject.GetComponent<CharacterController>();
        playerInput = new PlayerInputActions();
        animator = capibara.GetComponent<Animator>();
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
        TxInfo.gameObject.SetActive(true);
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
        TxInfo.gameObject.SetActive(false);
    }

    void Update()
    {
        if (capibara.activeSelf)
        {
            character._animator.SetFloat("Speed", 0);
        }
        if (character.ReturnDirection() == new Vector2(0, 0))
        {
            animator.SetBool("move", false);
        }
        else
        {
            animator.SetBool("move", true);

        }
        if (character.ReturnDirection().x != 0 && character.ReturnDirection().y != 0)
        {
            if (character.ReturnDirection().x > 0)
            {
                animator.SetFloat("Horizontal", character.ReturnDirection().x);
            }
            if (character.ReturnDirection().y > 0 && character.ReturnDirection().x <= 0)
            {
                animator.SetFloat("Vertical", character.ReturnDirection().y);

            }
        }
        else
        {
            animator.SetFloat("Horizontal", character.ReturnDirection().x);
            animator.SetFloat("Vertical", character.ReturnDirection().y);
        }
        if (nav.ReadValue<float>() > 0 && TxInfo.activeInHierarchy) { si(); }
        if (nav.ReadValue<float>() > 0 && TxInfo.activeInHierarchy) { si(); }
    }
    void si()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = true;
        TxInfo.gameObject.SetActive(false);
        capibara.SetActive(true);
        /*instanciar capibara en el player
        GameObject capibaraobject = Instantiate(capibara, transform.position, transform.rotation);
         capibaraobject.transform.position = new Vector2(capibaraobject.transform.position.x, capibaraobject.transform.position.y - 0.5f);

         capibaraobject.transform.SetParent(transform);*/
        navigation = true;
    }

}