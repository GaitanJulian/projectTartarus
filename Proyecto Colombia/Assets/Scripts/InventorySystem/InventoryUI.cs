using Events;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory _inventory;
    public Transform _itemsParent;
    InventorySlot[] _slots;
    void Start()
    {
        _inventory = Inventory._instance;
        _slots = _itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    private void Awake()
    {
        EventManager.AddListener(ENUM_Inventory.actualizeUI, UpdateUI);
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_Inventory.actualizeUI, UpdateUI);
    }

    void Update()
    {
        
    }

    void UpdateUI()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < _inventory._items.Count)
            {
                _slots[i].AddItemToSlot(_inventory._items[i]);
            }
            else
            {
                _slots[i].ClearItemSlot();
            }
        }
    }
}
