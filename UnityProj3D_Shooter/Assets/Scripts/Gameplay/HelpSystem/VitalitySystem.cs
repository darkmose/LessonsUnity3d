using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class VitalitySystem : MonoBehaviour
{
    private const int CriticalHPLevel = 20;
    public const int FullHealthPoints = 100;
    private int _healthPoints = FullHealthPoints;
    public bool IsCriticalHP => _healthPoints <= CriticalHPLevel;
    public int Health 
    {
        get 
        {
            return _healthPoints;
        }
        set
        {
            _healthPoints = value;
            if (_healthPoints > FullHealthPoints)
            {
                _healthPoints = FullHealthPoints;
            }
        }
            
    }
    private OnEntityDiesEvent _onEntityDiesEvent;
    private OnPlayerHealthRefreshEvent _onPlayerHealthRefreshEvent;
    private bool _isDead;

    private void Awake()
    {
        _onEntityDiesEvent = new OnEntityDiesEvent();
        _onPlayerHealthRefreshEvent = new OnPlayerHealthRefreshEvent();
    }

    public void TakeHealth(MedicinePack medicinePack) 
    {
        Health += MedicinePack.HealthPoints;
        _onPlayerHealthRefreshEvent.currentHealth = Health;
        EventsAgregator.Post<OnPlayerHealthRefreshEvent>(this, _onPlayerHealthRefreshEvent);
        Destroy(medicinePack.gameObject);
    }

    public void TakeDamage(int damage, string killerName, Weapon.WeaponType killersWeapon) 
    {
        if (!_isDead)
        {
            _healthPoints -= damage;
            if (_healthPoints <= 0)
            {
                _isDead = true;
                Death(killerName, killersWeapon);
            }
        }
    }

    private void Death(string killerName, Weapon.WeaponType killersWeapon)
    {
        _onEntityDiesEvent.killerName = killerName;
        _onEntityDiesEvent.victimName = gameObject.name;
        _onEntityDiesEvent.weaponType = killersWeapon;
        _onEntityDiesEvent.tag = gameObject.tag;
        EventsAgregator.Post<OnEntityDiesEvent>(this, _onEntityDiesEvent);
        Debug.Log($"Player {killerName} kill {gameObject.name} by {killersWeapon}");
    }
}
