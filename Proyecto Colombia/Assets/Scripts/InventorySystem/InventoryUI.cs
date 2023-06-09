using Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    Inventory _inventory;
    public Transform _itemsParent;
    SlotOfRadialInventory[] _slots;
    PlayerInputActions _playerControls;
    InputAction _openOrCloseInventory;

    void Start()
    {
        _inventory = Inventory._instance;
        _slots = _itemsParent.GetComponentsInChildren<SlotOfRadialInventory>();
        if(_itemsParent != null) _itemsParent.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _openOrCloseInventory = _playerControls.Player.OpenCloseInventory;
        _openOrCloseInventory.Enable();
    }
    private void OnDisable()
    {
        _openOrCloseInventory.Disable();
    }
    private void Awake()
    {
        _playerControls = new PlayerInputActions();
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
        if (_openOrCloseInventory.WasPressedThisFrame())
        {
            if(_itemsParent.gameObject.activeSelf) _itemsParent.gameObject.SetActive(false);
            else _itemsParent.gameObject.SetActive(true);
        }
    }

    /*
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            UpdateUI();
        }
    }*/
}