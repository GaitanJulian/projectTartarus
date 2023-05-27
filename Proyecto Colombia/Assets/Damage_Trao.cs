using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Trao : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Damageable>() != null)
        {
            collision.gameObject.GetComponent<Damageable>().GetDamaged(10f);
            collision.gameObject.GetComponent<Damageable>().SetAttacker(transform);
        }
    }
}
