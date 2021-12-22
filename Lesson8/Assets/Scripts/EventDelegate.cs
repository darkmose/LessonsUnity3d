using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDelegate : MonoBehaviour
{
    public static EventDelegate instance;
    public static event System.Action OnCannonballHitEvent;
    public static event System.Action OnPlatformButtonPressEvent;
    
    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            instance = this;
        }        
    }

    public static void RaiseOnCannonballHit() 
    {
        OnCannonballHitEvent?.Invoke();
    }
    
    public static void RaiseOnPlatformButtonPress() 
    {
        OnPlatformButtonPressEvent?.Invoke();
    }







}
