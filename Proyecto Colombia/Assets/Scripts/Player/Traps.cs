using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Traps : MonoBehaviour
{
    [SerializeField] GameObject UIselect;
    PlayerInputActions playerInput;
    InputAction SelectTrap,move;
    [SerializeField] GameObject[] UIobjects;
    [SerializeField] GameObject[] traps;
    [SerializeField] int[] cantidad;
    bool pasive=false,pass=false;
    int position=0;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInputActions();
    }
    private void OnEnable()
    {
        SelectTrap = playerInput.Player.SelectTrap;
        move = playerInput.Player.Move;
        move.Enable();
        SelectTrap.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        SelectTrap.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        PassiveSkill();
    }
    void PassiveSkill()
    {
        if (SelectTrap.ReadValue<float>() > 0)
        {
            pasive = true;
            gameObject.GetComponent<CharacterController>()._move.Disable();
            UIselect.SetActive(true);
            Vector2 mov = move.ReadValue<Vector2>();
            if (mov.x != 0)
            {
                if (!pass)
                {
                    if (mov.x > 0 && position < 3) { position++;  }
                    if (mov.x < 0 && position > -1) { position--;  }
                    pass = true;
                }
            }
            else{pass = false; }
       
            for (int i=0; i<3;i++)
            {
                if (i == position)
                {
                    UIobjects[i].GetComponent<Image>().color = new Color32(57,116,255,187);
                }
                else { UIobjects[i].GetComponent<Image>().color = new Color32(255, 255, 255, 187); }
            }
           
        }
        else if(pasive)
        {
            pasive = false;
            gameObject.GetComponent<CharacterController>()._move.Enable();
            UIselect.SetActive(false);
            if(position>=0 && position <= 2) { 
                GameObject trap= Instantiate(traps[position], transform);
                trap.transform.parent = null;

            }
          
           
        }
    }
}
