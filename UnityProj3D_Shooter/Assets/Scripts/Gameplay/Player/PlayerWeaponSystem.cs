using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;


public class PlayerWeaponSystem : BaseWeaponSystem
{
    private const int BulletsTakenFromSameWeapon = 20;
    public override bool HasWeapon => _weaponList?.Count > 0;
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
    [SerializeField] private Transform _weaponInventory;
    [SerializeField] private WeaponAnimator _weaponAnimator;
    [SerializeField] private IKWeaponControl _iKWeaponControl; 
    private List<Weapon> _weaponList;
    private OnWeaponChangedEvent _onWeaponChangedEvent;

    private void Awake()
    {
        _onWeaponChangedEvent = new OnWeaponChangedEvent();
        _weaponList = new List<Weapon>();
        _currentWeapon = null;
    }

    public override void TakeAmmo(Ammunition ammunition)
    {
        
    }

    public override void TakeWeapon(Weapon weapon)
    {
        foreach (Weapon weaponItem in _weaponList)
        {
            if (weaponItem.WeaponModel.Equals(weapon.WeaponModel))
            {
                weaponItem.AddAmmunition(BulletsTakenFromSameWeapon);
                Destroy(weapon.gameObject);
                return;
            }
        }

        weapon.gameObject.transform.SetParent(_weaponInventory);
        weapon.transform.SetPositionAndRotation(_weaponInventory.position, _weaponInventory.rotation);
        if (weapon.TryGetComponent(out SphereCollider sphereCollider))
        {
            sphereCollider.enabled = false;
        }
        weapon.InitializeWeaponAnimator(_weaponAnimator);
        _weaponList.Add(weapon);
        var lastWeaponIndex = _weaponList.Count - 1;
        ChangeWeapon(lastWeaponIndex);
    }

    public void ChangeWeapon(int weaponIndex) 
    {
        _currentWeapon?.gameObject.SetActive(false);
        var index = Mathf.Clamp(weaponIndex, 0, (_weaponList.Count-1));
        _currentWeapon = _weaponList[weaponIndex];
        _currentWeapon.gameObject.SetActive(true);
        _currentWeapon.npcIsOwner = false;
        _iKWeaponControl.ReinitializeIKs(_currentWeapon.iKLHand, _currentWeapon.iKRHand);
        _iKWeaponControl.IKControlOn();
        _onWeaponChangedEvent.bulletCount = _currentWeapon.BulletsCount;
        _onWeaponChangedEvent.bulletInMagazine = _currentWeapon.BulletsInMagazine;
        _onWeaponChangedEvent.weaponType = _currentWeapon.WeaponModel;
        EventsAgregator.Post<OnWeaponChangedEvent>(this, _onWeaponChangedEvent);

    }


    public void ReloadWeapon() 
    {
        _currentWeapon?.ReloadWeapon();
    }


    public override void Fire()
    {
        _currentWeapon?.Fire();
    }
}
