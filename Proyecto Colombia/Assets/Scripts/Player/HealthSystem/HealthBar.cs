using Events;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider _slider;
    public Gradient _gradient;
    public Image _fill;
    private void Awake()
    {
        EventManager.AddListener<float>(ENUM_Player.alterHitpoints, SetHealth);
    }

    private void Start()
    {
        SetMaxHealth(20f);
    }
    public void SetMaxHealth(float health)
    {
        _slider.maxValue = health;
        _slider.value = health;

        _fill.color = _gradient.Evaluate(1f);

        
    }

    public void SetHealth(float health)
    {
        _slider.value += health;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }

}
