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
                activar(renderer.gameObject);
                //renderer.gameObject.SetActive(true);
            }
            else
            {
                // El objeto es visible en la cámara, se activa.
                desactivar(renderer.gameObject);
                //renderer.gameObject.SetActive(false);
            }

            void activar(GameObject a)
            {
                // Obtén todos los componentes del objeto
                Component[] components = a.GetComponents<Component>();

                // Recorre todos los componentes y desactívalos
                foreach (var component in components)
                {
                    // No desactives los componentes Transform o GameObject
                    if (component.GetType() != typeof(Transform) && component.GetType() != typeof(GameObject))
                    {
                        // Desactiva el componente
                        component.gameObject.SetActive(true);
                    }
                }
            }
            void desactivar(GameObject a)
            {
                // Obtén todos los componentes del objeto
                Component[] components = a.GetComponents<Component>();

                // Recorre todos los componentes y desactívalos
                foreach (var component in components)
                {
                    // No desactives los componentes Transform o GameObject
                    if (component.GetType() != typeof(Transform) && component.GetType() != typeof(GameObject))
                    {
                        // Desactiva el componente
                       
                    }
                }
            }
        }
    }
}

