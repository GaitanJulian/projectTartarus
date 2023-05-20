using Events;
using System.Collections;
using System.Linq;
using UnityEngine;

public class InventoryOnScreen : MonoBehaviour
{
    Inventory _inventory;
    [SerializeField] GameObject _slotPrefab;
    [SerializeField] GameObject[] _aviableSlots;
    [SerializeField] float _radius = 1f, _rotationSpeed = 3;
    [SerializeField] int _numberVisible = 7;
    [SerializeField][Range(0f, 1f)] float _fractionOfCircunference = 0.45f;
    [SerializeField] Animator _arrowAnimator;
    float _angle, _thisAngle, _betweenPointsDistance;
    Vector3[] _slotPositions;
    float[] _sizes = new float[] { 0.0f, 0.11f, 0.14f, 0.2f, 0.14f, 0.11f, 0.0f };
    int[] _sortingOrder = new int[] { 0, 1, 2, 3, 2, 1, 0 };
    int[] _childIndexes;

    private void Start()
    {
        _inventory = Inventory._instance;
        InitializeVariables();
    }

    private void Awake()
    {
        EventManager.AddListener(ENUM_Inventory.actualizeUI, PlaceSlots);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_Inventory.actualizeUI, PlaceSlots);
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
    }
    private void Update()
    {
        CalculateAngle();
        if (Input.GetKeyDown(KeyCode.P))
        {
            _slotPositions = new Vector3[(int)_numberVisible];
            for (int i = transform.childCount - 1; i >= 0; i--) { Destroy(transform.GetChild(i).gameObject); }
            CalculateAngle();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RotateRight());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(RotateLeft());
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            InitializeVariables();
        }
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

        
        //I'll hardcode the values considering 7 visible
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
            _aviableSlots[_childIndexes[i]].GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingOrder[i];
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
        _arrowAnimator.SetTrigger("arrowRight");

        float timer = _betweenPointsDistance;

        while (timer > 0)
        {
            for (int i = 1; i < _slotPositions.Length; i++)
            {
                _aviableSlots[_childIndexes[i]].transform.position = Vector2.MoveTowards(_aviableSlots[_childIndexes[i]].transform.position, _slotPositions[i - 1], Time.deltaTime * _rotationSpeed);
                _aviableSlots[_childIndexes[i]].transform.localScale = Vector2.MoveTowards(_aviableSlots[_childIndexes[i]].transform.localScale, _sizes[i - 1] * Vector3.one, Time.deltaTime * _rotationSpeed);
                _aviableSlots[_childIndexes[i]].GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingOrder[i - 1];
            }
            timer = timer - Time.deltaTime * _rotationSpeed;
            yield return new WaitForEndOfFrame();
        }
        EventManager.Dispatch(ENUM_Inventory.actualizeUI);
        ChangePos(1);
        PlaceSlots();
    }

    IEnumerator RotateLeft()
    {
        _arrowAnimator.SetTrigger("arrowLeft");

        float timer = _betweenPointsDistance;

        while (timer > 0)
        {
            for (int i = 0; i < _slotPositions.Length - 1; i++)
            {
                _aviableSlots[_childIndexes[i]].transform.position = Vector2.MoveTowards(_aviableSlots[_childIndexes[i]].transform.position, _slotPositions[i + 1], Time.deltaTime * _rotationSpeed);
                _aviableSlots[_childIndexes[i]].transform.localScale = Vector2.MoveTowards(_aviableSlots[_childIndexes[i]].transform.localScale, _sizes[i + 1] * Vector3.one, Time.deltaTime * _rotationSpeed);
                _aviableSlots[_childIndexes[i]].GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingOrder[i + 1];
            }
            timer = timer - Time.deltaTime * _rotationSpeed;
            yield return new WaitForEndOfFrame();
        }
        EventManager.Dispatch(ENUM_Inventory.actualizeUI);
        ChangePos(-1);
        PlaceSlots();
    }
}


