using Events;
using UnityEngine;

enum ENUM_Player { alterHitpoints, reachedZero, reachedMax }

public class PlayerHitPoints : MonoBehaviour
{
    [SerializeField] float _HitPoints = 10f;
    [SerializeField] float _MaxHitPoints = 15f;

    private void Awake()
    {
        EventManager.AddListener<float>(ENUM_Player.alterHitpoints, ModifyHitPoints);
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener<float>(ENUM_Player.alterHitpoints, ModifyHitPoints);
    }
    /// <summary>
    /// Modifies player hitpoints either up or down, sends a message if reaches a limit
    /// </summary>
    /// <param name="magnitude"></param>
    private void ModifyHitPoints(float magnitude)
    {
        float _temp = _HitPoints + magnitude;
        if (_temp <= 0)
        {
            EventManager.Dispatch(ENUM_Player.reachedZero);
            _HitPoints = 0;
        }
        else if (_temp >= _MaxHitPoints)
        {
            EventManager.Dispatch(ENUM_Player.reachedMax, _temp - _MaxHitPoints);
            _HitPoints = _MaxHitPoints;
        }
        else
        {
            _HitPoints = _temp;
        }
    }
}