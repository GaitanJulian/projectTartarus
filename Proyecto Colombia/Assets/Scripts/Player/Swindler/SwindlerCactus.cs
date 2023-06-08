using System.Collections;
using UnityEngine;

public class SwindlerCactus : MonoBehaviour
{
    [SerializeField] float _maxLifeTime;
    Animator _animator;
    float _timer;
    bool _switchForCoroutine = true;
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _timer = _maxLifeTime;
    }
    private void Update()
    {
        if (_timer > 0) _timer -= Time.deltaTime;
        else if (_switchForCoroutine)
        {
            StartCoroutine(DestroyRoutine());
            _switchForCoroutine = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.gameObject.GetComponentInChildren<Damageable>() != null)
        {
            Damageable damage = collision.gameObject.GetComponentInChildren<Damageable>();
            if (damage.GetHitByCactusState() == true) return;
            damage.HitByCactusState(true);
            if (damage.GetIsBossBool() == true) damage.GetDamaged(damage.GetMaxHitPoints() * .1f);
            else damage.GetDamaged(damage.GetMaxHitPoints() * .3f);
            Debug.Log("damaged");
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