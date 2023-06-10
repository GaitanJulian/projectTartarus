using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwindlerFungus : MonoBehaviour
{
    [SerializeField] float _maxLifeTime;
    Animator _animator;
    float _timer;
    float _shootTimer, _shootCooldown = 2;
    bool _switchForCoroutine = true;
    Vector2[] _directions =
        {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right,
            new Vector2 (1, 1).normalized,
            new Vector2 (-1, 1).normalized,
            new Vector2 (1, -1).normalized,
            new Vector2 (-1, -1).normalized,
        };
    [SerializeField] GameObject _sporesPrefab;
    [SerializeField] float _sporeSpeed = 2f;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _timer = _maxLifeTime;
        _shootTimer = _shootCooldown;
    }
    private void Update()
    {
        if (_timer > 0) _timer -= Time.deltaTime;
        else if (_switchForCoroutine)
        {
            StartCoroutine(DestroyRoutine());
            _switchForCoroutine = false;
        }
        Shoot();
    }

    void Shoot()
    {
        if (_shootTimer > 0) _shootTimer -= Time.deltaTime;
        else
        {
            foreach (var direction in _directions)
            {
                var spore = Instantiate(_sporesPrefab, (Vector2)transform.position + direction, Quaternion.identity, null);
                if (spore.GetComponentInChildren<Rigidbody2D>() != null)
                {
                    spore.GetComponentInChildren<Rigidbody2D>().velocity = direction * _sporeSpeed;
                }
            }
            _shootTimer = _shootCooldown;
        }
    }

    IEnumerator DestroyRoutine()
    {
        _animator.SetTrigger("Fade");
        yield return new WaitForEndOfFrame();
        float time = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
