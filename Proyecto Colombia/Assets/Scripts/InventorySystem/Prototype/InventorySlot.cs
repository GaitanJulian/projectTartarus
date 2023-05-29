using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
    Item _item;
    public Image _icon;

    public void AddItemToSlot (Item newItem)
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
}
