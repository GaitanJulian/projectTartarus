using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwindlerBindweed : MonoBehaviour
{
    [SerializeField] float _maxLifeTime = 10;
    [SerializeField] float _stunTime = 5f;

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
                //Make susceptible to damage:
                Damageable damageScript = collision.transform.GetComponentInChildren<Damageable>();
                damageScript.Stun(_stunTime);
                //ApplyStun (reduction of stats):
                if (collision.transform.GetComponentInChildren<AlteredStateIntermediary>() != null) collision.transform.GetComponentInChildren<AlteredStateIntermediary>()._enemyController.Stun(_stunTime);
                else Debug.Log("fail");
                SelfDestroy();
            }
        }
    }
}
