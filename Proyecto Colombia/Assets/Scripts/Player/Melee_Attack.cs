using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Melee_Attack : MonoBehaviour
{
    //Component references:
    PlayerInputActions _playerInputActions;
    CharacterController _characterController;
    //InputAction _attack;
    //Atack variables:
    [SerializeField] float _attackRange, _attackDamage, _attackCooldown;
    float _attackTimer;
    Vector2 _attackPosition;
    //debug:
    [SerializeField] bool _drawGizmos;
    bool _attackingForGizmos;

    void Awake()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
        _playerInputActions = new PlayerInputActions();
    }
    /*
    private void OnEnable()
    {
        _attack = _playerInputActions.Player.Attack;
        _attack.Enable();
    }
    
    private void OnDisable()
    {
        _attack.Disable();
    }
    */
    void Update()
    {
        //_attackPosition = new Vector2(transform.position.x + _characterController.ReturnDirection().x / 2, transform.position.y + _characterController.ReturnDirection().y / 2);
        _attackPosition = (Vector2)transform.position + _characterController.ReturnDirection().normalized * _attackRange;
        /*
        if (_attack.ReadValue<float>() > 0 && _attackTimer <= 0)
        {
            Attack();
            _attackTimer = _attackCooldown;
        }
        if (_attackTimer > 0) _attackTimer -= Time.deltaTime;
        */
    }

    public void Attack()
    {
        if (_drawGizmos) StartCoroutine(GizmosColor());
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackPosition, _attackRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponentInChildren<Damageable>() != null && (collider.gameObject.layer == LayerMask.NameToLayer("Enemy") || collider.gameObject.layer == LayerMask.NameToLayer("Room"))) 
            {
                collider.GetComponentInChildren<Damageable>().GetDamaged(_attackDamage);
            }
        }
    }

        private void OnDrawGizmos()
    {
        if (_attackingForGizmos) Gizmos.color = Color.red;
        else Gizmos.color = Color.green;
        //
        if (_drawGizmos) Gizmos.DrawWireSphere(_attackPosition, _attackRange);
    }

    IEnumerator GizmosColor()
    {
        _attackingForGizmos = true;
        yield return new WaitForSeconds(0.1f);
        _attackingForGizmos = false;
    }
}
