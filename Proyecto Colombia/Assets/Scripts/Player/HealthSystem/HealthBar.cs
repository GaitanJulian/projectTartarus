using Events;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

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
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }

}
