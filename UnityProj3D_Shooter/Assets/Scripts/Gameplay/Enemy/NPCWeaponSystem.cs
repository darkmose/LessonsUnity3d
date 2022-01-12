using UnityEngine;

public class NPCWeaponSystem : BaseWeaponSystem
{
    private const int BulletsTakenFromSameWeapon = 20;

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
    public override void TakeAmmo(Ammunition ammunition)
    {
        throw new System.NotImplementedException();
    }


    public override void FireTarget(Transform target) 
    {
        if (target != null)
        {
            transform.LookAt(target);
            _currentWeapon.FireTarget(target);
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
        //weapon.transform.localScale = new Vector3(2, 2, 2);
        if (weapon.TryGetComponent(out SphereCollider sphereCollider))
        {
            sphereCollider.enabled = false;
        }
        weapon.InitializeWeaponAnimator(_weaponAnimator);
        _iKWeaponControl.ReinitializeIKs(_currentWeapon.iKLHand, _currentWeapon.iKRHand);
        _iKWeaponControl.IKControlOn();
    }
}