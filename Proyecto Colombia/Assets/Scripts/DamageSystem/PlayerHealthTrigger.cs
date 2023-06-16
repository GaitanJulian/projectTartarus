using UnityEngine.Events;

public class PlayerHealthTrigger : Damageable
{
    public UnityEvent<float> onHealTaken = new UnityEvent<float>();
    
    public void RecoverHealth(float amount)
    {
        _currentHitPoints += amount;
        onHealTaken?.Invoke(amount);
    }

    public float GetCurrentHealth()
    {
        return _currentHitPoints;
    }

    public float GetMaxHealth()
    {
        return _maxHitPoints;
    }
}
