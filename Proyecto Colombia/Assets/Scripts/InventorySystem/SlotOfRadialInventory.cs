using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotOfRadialInventory : MonoBehaviour
{
    public int _id;
    Item _item;
    public SpriteRenderer _icon;

    public void AddItemToSlot(Item newItem)
    {
        _item = newItem;
        _icon.sprite = _item._icon;
        _icon.enabled = true;
    }

    public void ClearItemSlot()
    {
        _item = null;
        _icon.sprite = null;
        _icon.enabled = false;
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
