using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndaDestroy : MonoBehaviour
{
    public float distance;
    public float _attackDamage;
    [SerializeField] LayerMask _obstaclesLayerMask, _damageableLayerMask;
    [HideInInspector] public Vector3 _direction;

    bool hit;
    void Start()
    {
        StartCoroutine(die(distance));
        hit = false;
    }
    IEnumerator die(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(gameObject);
    }

    private bool IsInLayer(GameObject gameObject, LayerMask layerMask)
    {
        int gameObjectLayer = gameObject.layer;
        return layerMask == (layerMask | (1 << gameObjectLayer));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsInLayer(collision.gameObject, _obstaclesLayerMask))
        {
            Debug.Log("hit wall");
            Destroy(this.gameObject);
        }
        else if (IsInLayer(collision.gameObject, _damageableLayerMask))
        {
            if (collision.gameObject.GetComponentInChildren<Damageable>() != null)
            {
                collision.gameObject.GetComponentInChildren<Damageable>().GetDamaged(_attackDamage);
            }
        }
    }
}
