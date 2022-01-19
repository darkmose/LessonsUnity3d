using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;


public class PlayerWeaponSystem : BaseWeaponSystem
{
    public override bool HasWeapon => _weaponList?.Count > 0;
    public override bool HasAmmo => _currentWeapon?.BulletsCount > 0;
    public int WeaponCount => _weaponList.Count;
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

    private const string AK74ScriptPrefabName = "AK74_ScriptOnly";
    private const string M1911ScriptPrefabName = "M1911_ScriptOnly";
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
        _currentWeapon?.AddAmmunition(Ammunition.BulletsCount);
        Destroy(ammunition.gameObject);
    }

    public override void TakeWeapon(Weapon weapon)
    {
        foreach (Weapon weaponItem in _weaponList)
        {
            if (weaponItem.WeaponModel.Equals(weapon.WeaponModel))
            {
                return;
            }
        }
        GameObject weaponPrefab = null;
        switch (weapon.WeaponModel)
        {
            case Weapon.WeaponType.AK74:
                {
                    weaponPrefab = PrefabDictionary.GetWeaponPrefab(AK74ScriptPrefabName);
                }
                break;
            case Weapon.WeaponType.M1911:
                {
                    weaponPrefab = PrefabDictionary.GetWeaponPrefab(M1911ScriptPrefabName);
                }
                break;
        }
        var weaponObject = Instantiate(weaponPrefab, this.transform) as GameObject;
        weaponObject.transform.localPosition = Vector3.zero;
        weaponObject.transform.rotation = transform.rotation;
        var weaponScript = weaponObject.GetComponent<Weapon>();
        weaponScript.InitializeWeapon(weapon.WeaponModel);
        weaponScript.ownerName = gameObject.name;
        _weaponList.Add(weaponScript);
        Destroy(weapon.gameObject);
        weaponScript.index = _weaponList.Count - 1;
        var lastWeaponIndex = _weaponList.Count - 1;
        ChangeWeapon(lastWeaponIndex);
    }

    public void ChangeWeapon(int weaponIndex) 
    {
        _currentWeapon = _weaponList[weaponIndex];
        _currentWeapon.npcIsOwner = false;
        _onWeaponChangedEvent.bulletCount = _currentWeapon.BulletsCount;
        _onWeaponChangedEvent.bulletInMagazine = _currentWeapon.BulletsInMagazine;
        _onWeaponChangedEvent.weaponType = _currentWeapon.WeaponModel;
        EventsAgregator.Post<OnWeaponChangedEvent>(this, _onWeaponChangedEvent);
    }

    public void NextWeapon() 
    {
        if (_currentWeapon != null)
        {
            int nextIndex = _currentWeapon.index + 1;
            if (_weaponList.Count - 1 >= nextIndex)
            {
                ChangeWeapon(nextIndex);
            }
            else
            {
                ChangeWeapon(0);
            }
        }        
    }

    public void PrevWeapon() 
    {
        if (_currentWeapon != null)
        {
            int prevIndex = _currentWeapon.index - 1;
            if (prevIndex >= 0)
            {
                ChangeWeapon(prevIndex);
            }
            else
            {
                ChangeWeapon(WeaponCount-1);
            }
        }
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
