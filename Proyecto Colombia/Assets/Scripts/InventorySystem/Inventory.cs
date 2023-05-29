using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ENUM_Inventory { actualizeUI, actualizeInventory }
public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory _instance;
    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("More than one instance of inventory was found.");
            return;
        }
        _instance = this;
    }
    #endregion

    public List<Item> _items = new List<Item>();
    public int _space = 1;
    public int _maxSpace = 12;

    public bool AddItem (Item item)
    {
        if(_items.Count >= _space)
        {
            if(_space + 1 <= _maxSpace)
            {
                _space++;
            }
            else
            {
                Debug.Log("not enought space");
                return false;
            }

        }
        _items.Add(item);
        EventManager.Dispatch(ENUM_Inventory.actualizeInventory);
        return true;
    }

    public void RemoveItem (Item item)
    {
        _items.Remove(item);
        EventManager.Dispatch(ENUM_Inventory.actualizeInventory);
    }
}
