using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Melee_Attack : MonoBehaviour
{
    PlayerInputActions playerControls;
    CharacterController movControll;
    InputAction Attack;
    string enemyTag = "Enemy";
    [SerializeField] float rangeAttack,damage;//radio del area de ataque 
    Vector2 posAttack;//vector donde estara el area de ataque
    float timeAttack;
    void Awake()
    {
        movControll = gameObject.GetComponent<CharacterController>();
        playerControls = new PlayerInputActions();
    }
    private void OnEnable()
    {
        Attack = playerControls.Player.Attack;
        Attack.Enable();
    }

    private void OnDisable()
    {
        Attack.Disable();
    }
    private void OnDrawGizmos()
    {  
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posAttack,rangeAttack);
    }
    // Update is called once per frame
    void Update()
    {
        posAttack = new Vector2(transform.position.x + movControll.ReturnDirection().x / 2, transform.position.y + movControll.ReturnDirection().y / 2);
        if (Attack.ReadValue<float>() > 0 && timeAttack<=0) { atack(); timeAttack = Attack.ReadValue<float>(); }
        if (timeAttack >= 0) { timeAttack -= Time.deltaTime; }
    }

    private void atack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(posAttack,rangeAttack);
        foreach(Collider2D colider in colliders)
        {
            if (colider.gameObject.GetComponent<Damageable>() != null)
            {
                colider.gameObject.GetComponent<Damageable>().GetDamaged(damage);
                colider.gameObject.GetComponent<Damageable>().SetAttacker(transform);
            }
        }
    }
}
