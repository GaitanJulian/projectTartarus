using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwindlerFungusSpores : MonoBehaviour
{
    [SerializeField] float _maxLifeTime = 10;

    private void Start()
    {
        Invoke("SelfDestroy", _maxLifeTime);
    }

    void SelfDestroy()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.transform.GetComponentInChildren<Damageable>() != null)
            {
                Damageable damageScript = collision.transform.GetComponentInChildren<Damageable>();
                damageScript.GetDamaged(damageScript.GetMaxHitPoints() * .1f);
                //ApplyPoisson (reduction of stats):
                if (collision.transform.GetComponentInChildren<AlteredStateIntermediary>() != null) collision.transform.GetComponentInChildren<AlteredStateIntermediary>()._enemyController.Poison();
                else Debug.Log("fail");
                //Add periodical damage component
                if (collision.transform.GetComponentInChildren<PoisonedDamage>() == null) collision.transform.AddComponent<PoisonedDamage>();
            }
        }
        SelfDestroy();
    }
}