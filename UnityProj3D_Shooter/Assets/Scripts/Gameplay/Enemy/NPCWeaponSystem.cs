using UnityEngine;
using GameEvents;
using System;

public class NPCWeaponSystem : BaseWeaponSystem
{

    [SerializeField] private Transform _weaponInventory;
    [SerializeField] private IKWeaponControl _iKWeaponControl;
    [SerializeField] private WeaponAnimator _weaponAnimator;
    public override bool HasWeapon
    {
        get 
        {
            if (_currentWeapon != null)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
    
    
    } 

    public override bool HasAmmo => _currentWeapon?.BulletsCount > 0;

    public override bool IsBurstShooting
    {
        get
        {
            if (_currentWeapon)
            {
                return _currentWeapon.BurstShooting;
            }
            else
            {
                return false;
            }
        }
    }
    public Weapon.WeaponType CurrentWeaponModel
    {
        get 
        {
            if (_currentWeapon != null)
            {
                return _currentWeapon.WeaponModel;
            }
            else
            {
                return Weapon.WeaponType.None;
            }
        }
    }


    public void ClearWeapons() 
    {
        _iKWeaponControl?.IKControlOff();
        _currentWeapon = null;
    }


    public override void TakeAmmo(Ammunition ammunition)
    {
        _currentWeapon?.AddAmmunition(Ammunition.BulletsCount);
        Destroy(ammunition);
    }


    public override void FireTarget(Transform target) 
    {
        if (target != null)
        {
            transform.LookAt(target);
            _currentWeapon?.FireTarget(target);
        }
    }


    public override void TakeWeapon(Weapon weapon)
    {
        if (weapon.WeaponModel.Equals(_currentWeapon?.WeaponModel))
        {
            Destroy(weapon.gameObject);
            return;
        }

        _currentWeapon = weapon;
        weapon.transform.SetParent(_weaponInventory);
        weapon.transform.SetPositionAndRotation(_weaponInventory.position, _weaponInventory.rotation);
        weapon.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        if (weapon.TryGetComponent(out SphereCollider sphereCollider))
        {
            sphereCollider.enabled = false;
        }
        weapon.InitializeWeaponAnimator(_weaponAnimator);
        weapon.ownerName = gameObject.name;
        _iKWeaponControl.ReinitializeIKs(_currentWeapon.iKLHand, _currentWeapon.iKRHand);
        _iKWeaponControl.IKControlOn();
    }
}