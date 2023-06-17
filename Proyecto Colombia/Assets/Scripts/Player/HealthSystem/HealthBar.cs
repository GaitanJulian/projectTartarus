using Events;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider _slider;
    public Gradient _gradient;
    public Image _fill;

    private PlayerHealthTrigger _healthTrigger;

      private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _healthTrigger = player.GetComponentInChildren<PlayerHealthTrigger>();
            if (_healthTrigger != null)
            {
                _healthTrigger._onDamageTaken.AddListener(RemoveHealth);
                _healthTrigger.onHealTaken.AddListener(AddHealth);
                SetMaxHealth(_healthTrigger.GetMaxHitPoints());
            }
        };
    }
    public void SetMaxHealth(float health)
    {
        _slider.maxValue = health;
        _slider.value = health;

        _fill.color = _gradient.Evaluate(1f);
        
    }

    public void AddHealth(float health)
    {
        _slider.value += health;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }

    public void RemoveHealth(float health)
    {
        _slider.value -= health;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }

}
