using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonedDamage : MonoBehaviour
{
    Damageable _damageableScript;
    [SerializeField] float _betweenDamageTime = 0.5f, _damageAmount = 1f, _thisComponentLifeTime = 6f;
    void Start()
    {
        _damageableScript = GetComponentInChildren<Damageable>();
        InvokeRepeating("DoDamage", _betweenDamageTime, _betweenDamageTime);
        Invoke("RemoveThisComponent", _thisComponentLifeTime);
    }

    void DoDamage()
    {
        _damageableScript.GetDamaged(_damageAmount);
        Debug.Log("done damage");
    }

    void RemoveThisComponent()
    {
        PoisonedDamage _this;
        _this = this;
        Destroy(_this);
    }
}
