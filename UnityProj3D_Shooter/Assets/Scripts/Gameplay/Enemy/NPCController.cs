using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using GameEvents;
using System;

public class NPCController : MonoBehaviour
{
    private const string WeaponItemTag = "WeaponItem";
    private const string AmmoItemTag = "AmmoItem";
    private const string HealthItemTag = "HealthItem";
    private const string AnimationForwardVelocityParam = "ForwardVelocity";
    private const float TimeToDestroyThisObject = 3f;
    private const string DeathAnimationBoolParam = "IsDead";
    [SerializeField] private VitalitySystem _vitalitySystem;
    [SerializeField] private NavigationSystem _navigationSystem;
    [SerializeField] private NPCWeaponSystem _weaponSystem;
    [SerializeField] private Animator _animator;
    private OnEnemyTakeDamageEvent _onEnemyTakeDamageEvent;

    private void Awake()
    {
        _onEnemyTakeDamageEvent = new OnEnemyTakeDamageEvent();
        EventsAgregator.Subscribe<OnEntityDiesEvent>(OnEntityDiesHandler);
    }

    private void OnEntityDiesHandler(object sender, OnEntityDiesEvent data)
    {
        _weaponSystem.ClearWeapons();
        DeathAnimation();
        Destroy(this.gameObject, TimeToDestroyThisObject);
    }

    public void TakeDamage(int damage,string killerName, Weapon.WeaponType weapon) 
    {
        _onEnemyTakeDamageEvent.damage = damage;
        EventsAgregator.Post<OnEnemyTakeDamageEvent>(this, _onEnemyTakeDamageEvent);
        _vitalitySystem.TakeDamage(damage, killerName, weapon);
    }

    private void DeathAnimation() 
    {
        _animator?.SetBool(DeathAnimationBoolParam, true);
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
                if (other.TryGetComponent(out Ammunition ammo))
                {
                    _weaponSystem?.TakeAmmo(ammo);
                }
                break;
            case HealthItemTag:
                if (other.TryGetComponent(out MedicinePack medicine))
                {
                    _vitalitySystem.TakeHealth(medicine);
                }
                break;
        }
    }


    private void Animation() 
    {
        Vector3 velocity = _navigationSystem.CurrentVelocity;

        float forwardVelocity = 0f;

        if (Mathf.Abs(velocity.z) > Mathf.Abs(velocity.x))
        {
            forwardVelocity = Mathf.Abs(velocity.z);
        }
        else
        {
            forwardVelocity = Mathf.Abs(velocity.x);
        }
        _animator.SetFloat(AnimationForwardVelocityParam, forwardVelocity);
    }


    private void FixedUpdate()
    {
        Animation();
    }

}
