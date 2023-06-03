using Events;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory _inventory;
    public Transform _itemsParent;
    SlotOfRadialInventory[] _slots;
    void Start()
    {
        _inventory = Inventory._instance;
        _slots = _itemsParent.GetComponentsInChildren<SlotOfRadialInventory>();
    }

    private void Awake()
    {
        EventManager.AddListener(ENUM_Inventory.actualizeUI, UpdateUI);
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_Inventory.actualizeUI, UpdateUI);
    }

    void UpdateUI()
    {
        _slots = _itemsParent.GetComponentsInChildren<SlotOfRadialInventory>();

        if (_inventory == null) return;

        for (int i = 0; i < _inventory._items.Count; i++)
        {
            for (int j = 0; j < _slots.Length; j++)
            {
                if (_slots[j]._id == i)
                {
                    _slots[j].AddItemToSlot(_inventory._items[i]);
                }
            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            UpdateUI();
        }
    }
}