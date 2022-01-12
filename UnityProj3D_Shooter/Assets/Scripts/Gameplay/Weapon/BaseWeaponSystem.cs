using UnityEngine;



public abstract class BaseWeaponSystem : MonoBehaviour
{
    public abstract bool HasWeapon { get; }
    public abstract bool HasAmmo { get; }
    public abstract bool IsBurstShooting { get;}

    protected Weapon _currentWeapon;
    public abstract void TakeAmmo(Ammunition ammunition);
    public abstract void TakeWeapon(Weapon weapon);

    public virtual void Fire() 
    {
    }
    public virtual void FireTarget(Transform target) 
    {
    }
}
