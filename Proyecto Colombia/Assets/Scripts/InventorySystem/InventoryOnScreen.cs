using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryOnScreen : MonoBehaviour
{
    [SerializeField] GameObject _slotPrefab;
    [SerializeField] GameObject[] _aviableSlots;
    Inventory _inventory;
    PlayerInputActions _playerControls;
    InputAction _inventoryLeft, _inventoryRight, _useItem;
    float _radius = 1f, _rotationSpeed = 2f;
    int _numberVisible = 7;
    [Range(0f, 1f)] float _fractionOfCircunference = 0.45f;
    float _angle, _thisAngle, _betweenPointsDistance;
    Vector3[] _slotPositions;
    float[] _sizes = new float[] { 0.07f, 0.12f, 0.16f, 0.2f, 0.16f, 0.12f, 0.07f };
    int[] _sortingOrder = new int[] { 0, 2, 4, 6, 4, 2, 0 };
    int[] _childIndexes;
    bool _ableToRotate = true;

    Queue<InputAction> inputQueue = new Queue<InputAction>();

    private void Start()
    {
        _inventory = Inventory._instance;
        InitializeVariables();
    }
    private void Awake()
    {
        _playerControls = new PlayerInputActions();
        EventManager.AddListener(ENUM_Inventory.actualizeInventory, InitializeVariables);
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_Inventory.actualizeInventory, InitializeVariables);
    }
    private void OnEnable()
    {
        _inventoryLeft = _playerControls.Player.InventoryLeft;
        _inventoryLeft.Enable();
        _inventoryRight = _playerControls.Player.InventoryRight;
        _inventoryRight.Enable();
        _useItem = _playerControls.Player.UseItem;
        _useItem.Enable();
    }
    private void OnDisable()
    {
        _inventoryLeft.Disable();
        _inventoryRight.Disable();
        _useItem.Disable();
    }
    private void Update()
    {
        CalculateAngle();

        if (_inventoryLeft.WasPressedThisFrame())
        {
            inputQueue.Enqueue(_inventoryLeft);
        }
        else if (_inventoryRight.WasPressedThisFrame())
        {
            inputQueue.Enqueue(_inventoryRight);
        }

        if (_ableToRotate && inputQueue.Count > 0)
        {
            _ableToRotate = false;
            var action = inputQueue.Peek();
            Debug.Log(action);
            if ( action == _inventoryLeft )
            {
                StartCoroutine(RotateLeft());
            }
            else if ( action == _inventoryRight)
            {
                StartCoroutine(RotateRight());
            }
            inputQueue.Dequeue();
        }

    }
    private void InitializeVariables()
    {
        _slotPositions = new Vector3[_numberVisible];
        _childIndexes = new int[_numberVisible];
        for (int i = 0; i < _numberVisible; i++)
        {
            _childIndexes[i] = i;
        }
        CalculateAngle();
        DestroyAllSlots();
        GenerateSlots(_inventory);
        HideSlots();
        PlaceSlots();
        EventManager.Dispatch(ENUM_Inventory.actualizeUI);
    }
    void DestroyAllSlots()
    {
        for (int i = 0; i < _aviableSlots.Length; i++)
        {
            _aviableSlots[i].GetComponent<SlotOfRadialInventory>().SelfDestroy();
        }
    }
    void GenerateSlots(Inventory inventory)
    {
        int numberOfPlaces = inventory._space;

        
        //I'll hardcode the values considering 7 visible | Also this is so ugly, works, but needs refactorization
        if (numberOfPlaces >= 7)
        {
            _aviableSlots = new GameObject[numberOfPlaces];
            for (int i = 0; i < numberOfPlaces; i++)
            {
                _aviableSlots[i] = Instantiate(_slotPrefab, transform);
                _aviableSlots[i].GetComponent<SlotOfRadialInventory>()._id = i;
            }
        }
        //============================================================
        else if (numberOfPlaces >= 4)
        {
            _aviableSlots = new GameObject[numberOfPlaces * 2];
            for (int i = 0; i < numberOfPlaces; i++)
            {
                _aviableSlots[i] = Instantiate(_slotPrefab, transform);
                _aviableSlots[i].GetComponent<SlotOfRadialInventory>()._id = i;
            }
            for (int i = 0; i < numberOfPlaces; i++)
            {
                _aviableSlots[i + numberOfPlaces] = Instantiate(_slotPrefab, transform);
                _aviableSlots[i + numberOfPlaces].GetComponent<SlotOfRadialInventory>()._id = i;
            }
        }
        //============================================================
        else if (numberOfPlaces >= 2)
        {
            _aviableSlots = new GameObject[numberOfPlaces * 4];
            for (int i = 0; i < numberOfPlaces; i++)
            {
                _aviableSlots[i] = Instantiate(_slotPrefab, transform);
                _aviableSlots[i].GetComponent<SlotOfRadialInventory>()._id = i;
            }
            for (int i = 0; i < numberOfPlaces; i++)
            {
                _aviableSlots[i + numberOfPlaces] = Instantiate(_slotPrefab, transform);
                _aviableSlots[i + numberOfPlaces].GetComponent<SlotOfRadialInventory>()._id = i;
            }
            for (int i = 0; i < numberOfPlaces; i++)
            {
                _aviableSlots[i + numberOfPlaces * 2] = Instantiate(_slotPrefab, transform);
                _aviableSlots[i + numberOfPlaces * 2].GetComponent<SlotOfRadialInventory>()._id = i;
            }
            for (int i = 0; i < numberOfPlaces; i++)
            {
                _aviableSlots[i + numberOfPlaces * 3] = Instantiate(_slotPrefab, transform);
                _aviableSlots[i + numberOfPlaces * 3].GetComponent<SlotOfRadialInventory>()._id = i;
            }
        }
        else if (numberOfPlaces == 1)
        {
            _aviableSlots = new GameObject[numberOfPlaces * 7];
            for (int i = 0; i < numberOfPlaces * 7; i++)
            {
                _aviableSlots[i] = Instantiate(_slotPrefab, transform);
                _aviableSlots[i].GetComponent<SlotOfRadialInventory>()._id = 0;//i super hard coded this shit
            }
        }
        HideSlots();
        PlaceSlots();
    }
    void CalculateAngle()
    {
        _angle = 360 * _fractionOfCircunference / _numberVisible;
        float _startingAngle = 90 - 360 * _fractionOfCircunference / 2 + _angle / 2;

        for (int i = 0; i < _numberVisible; i++)
        {
            _thisAngle = _startingAngle + _angle * i;
            float x = transform.position.x + _radius * Mathf.Cos(_thisAngle * Mathf.Deg2Rad);
            float y = transform.position.y + _radius * Mathf.Sin(_thisAngle * Mathf.Deg2Rad);
            _slotPositions[i] = new Vector3(x, y, 0);
        }
        //_slotPositions[3].y -= 0.05f;
        _slotPositions[2].y -= 0.07f;
        _slotPositions[4].y -= 0.07f;
        _betweenPointsDistance = Vector2.Distance(_slotPositions[0], _slotPositions[1]);
    }
    void HideSlots()
    {
        foreach (GameObject gameObject in _aviableSlots)
        {
            gameObject.SetActive(false);
        }
    }
    void PlaceSlots()
    {
        HideSlots();
        for (int i = 0; i < _slotPositions.Length; i++)
        {
            _aviableSlots[_childIndexes[i]].SetActive(true);
            _aviableSlots[_childIndexes[i]].transform.position = _slotPositions[i];
            _aviableSlots[_childIndexes[i]].transform.localScale = _sizes[i] * Vector3.one;
            _aviableSlots[_childIndexes[i]].transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingOrder[i];
            _aviableSlots[_childIndexes[i]].transform.GetChild(1).GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingOrder[i] + 1;
        }
    }
    void ChangePos(int number)
    {
        for (int i = 0; i < _childIndexes.Length; i++)
        {
            if (_childIndexes[i] + number < 0)
            {
                _childIndexes[i] = _aviableSlots.Length - 1;
            }
            else if (_childIndexes[i] + number > _aviableSlots.Length - 1)
            {
                _childIndexes[i] = 0;
            }
            else
            {
                _childIndexes[i] += number; 
            }
        }
    }
    IEnumerator RotateRight()
    {
        float timer = _betweenPointsDistance;

        while (timer > 0)
        {
            for (int i = 1; i < _slotPositions.Length; i++)
            {
                _aviableSlots[_childIndexes[i]].transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingOrder[i - 1];
                _aviableSlots[_childIndexes[i]].transform.GetChild(1).GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingOrder[i - 1] + 1;
                _aviableSlots[_childIndexes[i]].transform.position = Vector2.MoveTowards(_aviableSlots[_childIndexes[i]].transform.position, _slotPositions[i - 1], Time.deltaTime * _rotationSpeed);
                _aviableSlots[_childIndexes[i]].transform.localScale = Vector2.MoveTowards(_aviableSlots[_childIndexes[i]].transform.localScale, _sizes[i - 1] * Vector3.one, Time.deltaTime * _rotationSpeed / 3);
            }
            timer = timer - Time.deltaTime * _rotationSpeed;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        EventManager.Dispatch(ENUM_Inventory.actualizeUI);
        ChangePos(1);
        PlaceSlots();

        _ableToRotate = true;
    }
    IEnumerator RotateLeft()
    {
        float timer = _betweenPointsDistance;

        while (timer > 0)
        {
            for (int i = 0; i < _slotPositions.Length - 1; i++)
            {
                _aviableSlots[_childIndexes[i]].transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingOrder[i + 1];
                _aviableSlots[_childIndexes[i]].transform.GetChild(1).GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingOrder[i + 1] + 1;
                _aviableSlots[_childIndexes[i]].transform.position = Vector2.MoveTowards(_aviableSlots[_childIndexes[i]].transform.position, _slotPositions[i + 1], Time.deltaTime * _rotationSpeed);
                _aviableSlots[_childIndexes[i]].transform.localScale = Vector2.MoveTowards(_aviableSlots[_childIndexes[i]].transform.localScale, _sizes[i + 1] * Vector3.one, Time.deltaTime * _rotationSpeed / 3);
            }
            timer = timer - Time.deltaTime * _rotationSpeed;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        EventManager.Dispatch(ENUM_Inventory.actualizeUI);
        ChangePos(-1);
        PlaceSlots();

        _ableToRotate = true;
    }
}