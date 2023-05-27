using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndaDestroy : MonoBehaviour
{
     public float distance;
     [SerializeField]float damage=10f;
    void Start()
    {
        StartCoroutine(die(distance));
    }
    IEnumerator die(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Damageable>() != null)
        {
            collision.gameObject.GetComponent<Damageable>().GetDamaged(damage);
            collision.gameObject.GetComponent<Damageable>().SetAttacker(transform);
        }
    }
}
