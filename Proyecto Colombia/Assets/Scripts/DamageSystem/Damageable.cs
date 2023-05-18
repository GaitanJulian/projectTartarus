using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] float _HitPoints = 100f;

    public void GetDamaged(float magnitude)
    {
        if(_HitPoints > magnitude)
        {
            _HitPoints -= magnitude;
        }
        else
        {
            _HitPoints = 0;
        }
    }
}