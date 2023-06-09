using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwindlerTraps : MonoBehaviour
{
    [SerializeField] GameObject _instantiationReference;
    [Header("Inventory references")]
    [SerializeField] GameObject _graphicsParent;
    [SerializeField] GameObject _noTrapsAvaibleGraphic;
    [SerializeField] GameObject _cactusGraphic, _bindweedGraphic, _fungusGraphic;
    [Header("Max quantity")]
    [SerializeField] float _maxCactus;
    [SerializeField] float _maxBindweed;
    [SerializeField] float _maxFungus;
    [Header("Actual number")]
    [SerializeField] float _cactusNumber;
    [SerializeField] float _bindweedNumber;
    [SerializeField] float _fungusNumber;
    [Header("Prefabs")]
    [SerializeField] GameObject _cactusPrefab;
    [SerializeField] GameObject _bindweedPrefab;
    [SerializeField] GameObject _fungusPrefab;

    PlayerInputActions _inputActions;
    InputAction _trapButton;

    bool _hasBeenActivated = false, _expectingToSelectTrap = false;

    List<GameObject> _avaibleTraps = new List<GameObject>();

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        //
        _graphicsParent.SetActive(false);
        _noTrapsAvaibleGraphic.SetActive(false);
    }

    private void OnEnable()
    {
        _trapButton = _inputActions.Player.SelectTrap;
        _trapButton.Enable();
        //
        _expectingToSelectTrap = false;
        i = 0;
    }

    private void OnDisable()
    {
        _trapButton.Disable();
        StopAllCoroutines();
    }

    private void Update()
    {
        ManageSwindlerInventory();
        //if (_trapButton.WasReleasedThisFrame()) Debug.Log("PRESSED BUTTON");
    }

    void ManageSwindlerInventory()
    {
        if (_trapButton.WasPressedThisFrame() && !_graphicsParent.activeSelf)
        {
            _graphicsParent.SetActive(true);
            HideAllItems();
            CreateList();
            _hasBeenActivated = true;
        }

        if (_trapButton.WasReleasedThisFrame() && _hasBeenActivated) _expectingToSelectTrap = true;

        if (_trapButton.WasPressedThisFrame() && _expectingToSelectTrap)
        {
            if (_avaibleTraps.Count > 0) PlaceTrap(_avaibleTraps[i]);
                //Debug.Log(_avaibleTraps[i]);
            HideAllItems();
            _hasBeenActivated = false;
            _expectingToSelectTrap = false;
            _graphicsParent.SetActive(false);
            StopAllCoroutines();
        }
    }

    void CreateList()
    {
        _avaibleTraps.Clear();
        if (_cactusNumber > 0) _avaibleTraps.Add(_cactusGraphic);
        if (_bindweedNumber > 0) _avaibleTraps.Add(_bindweedGraphic);
        if (_fungusNumber > 0) _avaibleTraps.Add(_fungusGraphic);

        if (_avaibleTraps.Count <= 0)
        {
            _expectingToSelectTrap = false;
            _noTrapsAvaibleGraphic.SetActive(true);
            StartCoroutine(WaitThenClose());
            return;
        }
        else if (_avaibleTraps.Count == 1)
        {
            i = 0;
            _graphicsParent.SetActive(true);
            _avaibleTraps[0].SetActive(true);
            return;
        }
        else
        {
            _graphicsParent.SetActive(true);
            StartCoroutine(ShowVariousItems());
        }     
    }

    int i = 0;
    float _waitTime = 1;
    IEnumerator ShowVariousItems()
    {
        HideAllItems();
        _avaibleTraps[i].SetActive(true);
        yield return new WaitForSeconds(_waitTime);
        i++;
        if (i >= _avaibleTraps.Count) i = 0;
        StartCoroutine(ShowVariousItems());
    }

    void HideAllItems()
    {
        _cactusGraphic.SetActive(false);
        _bindweedGraphic.SetActive(false);
        _fungusGraphic.SetActive(false);
        _noTrapsAvaibleGraphic.SetActive(false);
    }

    IEnumerator WaitThenClose()
    {
        yield return new WaitForSeconds(_waitTime);
        _graphicsParent.SetActive(false);
        _hasBeenActivated = false;
        HideAllItems();
        StopAllCoroutines();
    }

    void PlaceTrap(GameObject trap)
    {
        if (trap == _cactusGraphic)
        {
            Instantiate(_cactusPrefab, _instantiationReference.transform.position, Quaternion.identity, null);
            _cactusNumber--;
        }
        else if (trap == _fungusGraphic)
        {
            Instantiate(_fungusPrefab, _instantiationReference.transform.position, Quaternion.identity, null);
            _fungusNumber--;
        }
        else if (trap == _bindweedGraphic)
        {
            Instantiate(_bindweedPrefab, _instantiationReference.transform.position, Quaternion.identity, null);
            _bindweedNumber--;
        }
        else Debug.Log("ERROR");
    }
}