using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class VitalitySystem : MonoBehaviour
{
    private const int CriticalHPLevel = 20;
    private const int StartHealthPoints = 100;

    private int _healthPoints = StartHealthPoints;
    public bool IsCriticalHP => _healthPoints <= CriticalHPLevel;
    private OnEntityDiesEvent _onEntityDiesEvent;

    private void Awake()
    {
        _onEntityDiesEvent = new OnEntityDiesEvent();
    }


    public void TakeDamage(int damage, string killerName, Weapon.WeaponType killersWeapon) 
    {
        _healthPoints -= damage;
        if (_healthPoints <= 0)
        {
            Death(killerName, killersWeapon);
        }
    }

    private void Death(string killerName, Weapon.WeaponType killersWeapon)
    {
        _onEntityDiesEvent.killerName = killerName;
        _onEntityDiesEvent.victimName = gameObject.name;
        _onEntityDiesEvent.weaponType = killersWeapon;
        EventsAgregator.Post<OnEntityDiesEvent>(this, _onEntityDiesEvent);
        Debug.Log($"Player {killerName} kill {gameObject.name} by {killersWeapon}");
        Destroy(this.gameObject);
    }
}
