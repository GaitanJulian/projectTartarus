using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Traps : MonoBehaviour
{
    PlayerInputActions playerInput;
    InputAction SelectTrap;
    [SerializeField] Canvas info;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInputActions();
    }
    private void OnEnable()
    {
        SelectTrap = playerInput.Player.SelectTrap;
        SelectTrap.Enable();
    }
    private void OnDisable()
    {
        SelectTrap.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PassiveSkill()
    {
    
    }
}
