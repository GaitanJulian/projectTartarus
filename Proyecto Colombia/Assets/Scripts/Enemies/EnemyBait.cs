using UnityEngine;

public class EnemyBait : MonoBehaviour
{
    void Start()
    {
        Invoke("SelfDestroy", 0.2f);
    }

    private void Update()
    {
        transform.position += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * Time.deltaTime;
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
