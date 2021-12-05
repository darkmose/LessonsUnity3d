using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDelegate : MonoBehaviour
{
    static EventDelegate instance;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    public static event System.Action OnPlayerDeathEvent;
    public static event System.Action OnPlayerTakeDamageEvent;
    public static event System.Action<float> OnEnemyTakeDamageEvent;
    public static event System.Action OnEnemyDeathEvent;
    public static event System.Action OnEnemySpawn;


    public static void RaiseOnEnemySpawn() 
    {
        OnEnemySpawn?.Invoke();
    }

    public static void RaiseOnEnemyDeathEvent() 
    {
        OnEnemyDeathEvent?.Invoke();
    }
    public static void RaiseOnPlayerDeathEvent() 
    {
        OnPlayerDeathEvent?.Invoke();
    }
    public static void RaiseOnPlayerTakeDamageEvent() 
    {
        OnPlayerTakeDamageEvent?.Invoke();
    }
    public static void RaiseOnEnemyTakeDamageEvent(float damage) 
    {
        OnEnemyTakeDamageEvent?.Invoke(damage);
    }


}
