using UnityEngine;

public class HealObject : MonoBehaviour
{
    [SerializeField] private float _healAmount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerBigCollider"))
        {
            PlayerHealthTrigger playerHealthTrigger = collision.gameObject.GetComponentInParent<PlayerHealthTrigger>();
            if (playerHealthTrigger != null)
            {
                playerHealthTrigger.RecoverHealth(_healAmount);
            }

            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}