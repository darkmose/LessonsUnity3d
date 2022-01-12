using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using GameEvents;

public class NPCController : MonoBehaviour
{
    private const string WeaponItemTag = "WeaponItem";
    private const string AmmoItemTag = "AmmoItem";
    private const string HealthItemTag = "HealthItem";

    [SerializeField] private VitalitySystem _vitalitySystem;
    [SerializeField] private NavigationSystem _navigationSystem;
    [SerializeField] private NPCWeaponSystem _weaponSystem;
    private OnEnemyTakeDamageEvent _onEnemyTakeDamageEvent;

    private void Awake()
    {
        _onEnemyTakeDamageEvent = new OnEnemyTakeDamageEvent();
    }


    public void TakeDamage(int damage, Weapon.WeaponType weapon) 
    {
        _onEnemyTakeDamageEvent.damage = damage;
        EventsAgregator.Post<OnEnemyTakeDamageEvent>(this, _onEnemyTakeDamageEvent);
        _vitalitySystem.TakeDamage(damage, this.gameObject.name, weapon);
    }


    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case WeaponItemTag:
                if (other.TryGetComponent(out Weapon weapon))
                {
                    _weaponSystem?.TakeWeapon(weapon);
                }
                break;
            case AmmoItemTag:
                break;
            case HealthItemTag:
                break;
        }
    }

    private void Update()
    {

    }


}
