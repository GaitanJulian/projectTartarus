using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwindlerCactusGraceArea : MonoBehaviour
{
    [SerializeField] bool _gizmos;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.gameObject.GetComponentInChildren<Damageable>() != null)
        {
            Damageable damage = collision.gameObject.GetComponentInChildren<Damageable>();
            damage.HitByCactusState(false);
        }
    }

    private void OnDrawGizmos()
    {
        if (_gizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, transform.GetComponent<CircleCollider2D>().radius);
        }
    }
}