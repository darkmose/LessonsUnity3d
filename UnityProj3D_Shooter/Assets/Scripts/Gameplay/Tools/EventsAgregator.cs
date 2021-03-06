using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace GameEvents
{
    public class OnWeaponChangedEvent
    {
        public Weapon.WeaponType weaponType;
        public int bulletCount;
        public int bulletInMagazine;
    }

    public class OnWeaponShootEvent
    {
        public int bulletsInMagazine;
    }
    public class OnWeaponReloadEvent
    {
        public int bulletsCount;
        public int bulletsInMagazine;
    }

    public class OnPlayerTakeDamageEvent
    {
        public int damage;
        public int currentHP;
    }

    public class OnPlayerHealthRefreshEvent
    {
        public int currentHealth;
    }

    public class OnEnemyTakeDamageEvent 
    {
        public int damage; 
    }
    public class OnEntityDiesEvent
    {
        public string tag;
        public string killerName;
        public string victimName;
        public Weapon.WeaponType weaponType;
    }
}




public static class EventsAgregator 
{
    public static void Subscribe<TEvent>(System.Action<object, TEvent> eventHandler) 
    {
        EventHelper<TEvent>.Event += eventHandler;
    }
    public static void Unsubscribe<TEvent>(System.Action<object, TEvent> eventHandler) 
    {
        EventHelper<TEvent>.Event -= eventHandler;
    }

    public static void Post<TEvent>(object sender, TEvent eventData) 
    {
        EventHelper<TEvent>.Post(sender, eventData);
    }
    
    private static class EventHelper <TEvent>
    {
        public static event System.Action<object, TEvent> Event;

        public static void Post(object sender, TEvent eventData) 
        {
            Event?.Invoke(sender, eventData);
	    }
    
    }
}
