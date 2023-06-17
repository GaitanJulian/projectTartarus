using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyJar : MonoBehaviour
{
    private Damageable _damageable;

    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        _damageable._onDeath.AddListener(OnDeath); // Listen to the death event
    }

    private void OnDisable()
    {
        _damageable._onDeath.RemoveListener(OnDeath);   
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }
}
