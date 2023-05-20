using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] float _HitPoints = 100f;
    private Transform _attacker;

    public UnityEvent<Transform, float> onDamageTaken = new UnityEvent<Transform, float>();
    public UnityEvent onDeath = new UnityEvent();

    public void GetDamaged(float magnitude)
    {
        if(_HitPoints > magnitude)
        {
            _HitPoints -= magnitude;
            onDamageTaken.Invoke(_attacker, magnitude);
        }
        else
        {
            _HitPoints = 0;
            onDeath.Invoke();
        }

    }

    public void SetAttacker(Transform _attacker)
    {
        this._attacker = _attacker;
    }

}