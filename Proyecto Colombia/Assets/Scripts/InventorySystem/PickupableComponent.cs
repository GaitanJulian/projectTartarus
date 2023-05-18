using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableComponent : MonoBehaviour
{
    public Item _item;
    public void Pickup()
    {
        Debug.Log("picking up " + _item._name);
        bool wasPickedUp = Inventory._instance.AddItem(_item);
        if (wasPickedUp) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.transform.CompareTag("Player"))
            {
                Pickup();
            }
        }
    }
}
