using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oclussion : MonoBehaviour
{
    private void Update()
    {
        // Obtiene todos los objetos en la escena con el componente Renderer.
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            if (renderer.isVisible)
            {
                Debug.Log("visible");
               
                renderer.gameObject.SetActive(true);
            }
            else
            {
                // El objeto es visible en la cámara, se activa.
                Debug.Log("no visible");
                renderer.gameObject.SetActive(false);
            }
        }
    }
}
