using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType { SimpleTurret, SpreadTurret, HomingMissle, SingleMissle };
    public WeaponType type;
    public float damage = 100f;
    public float bulletSpeed = 10f;
    public float fireRate = 20f;
    private float timeCounter = 0f;
    private bool fireSide;
    GameObject ammo;

    public Weapon()
    {
        //Стандартный конструктор
    }

    public void InitializeWeapon(WeaponType type)
    {
        this.type = type;
        switch (type)
        {
            case WeaponType.SimpleTurret:
                damage = 100f;
                bulletSpeed = 10f;
                fireRate = 5f;
                type = WeaponType.SimpleTurret;
                ammo = PrefabsDictionary.GetAmmoPrefab(PrefabsDictionary.Ammo.Bullet);
                break;
            case WeaponType.SpreadTurret:
                damage = 120f;
                bulletSpeed = 10f;
                fireRate = 4f;
                type = WeaponType.SpreadTurret;
                ammo = PrefabsDictionary.GetAmmoPrefab(PrefabsDictionary.Ammo.Bullet);
                break;
            case WeaponType.HomingMissle:
                damage = 240f;
                bulletSpeed = 10f;
                fireRate = 2f;
                type = WeaponType.HomingMissle;
                ammo = PrefabsDictionary.GetAmmoPrefab(PrefabsDictionary.Ammo.HomingMissile);
                break;
            case WeaponType.SingleMissle:
                damage = 250f;
                bulletSpeed = 10f;
                fireRate = 2f;
                type = WeaponType.SingleMissle;
                ammo = PrefabsDictionary.GetAmmoPrefab(PrefabsDictionary.Ammo.Missile);
                break;
        }
    }


    private void FireMissile() 
    {
        timeCounter += Time.deltaTime;
        var side = fireSide ? Vector3.right/2 : Vector3.left/2;
        var startPoint = transform.position + Vector3.up + side;
        fireSide = !fireSide;

        if (timeCounter >= 1 / fireRate)
        {
            var missile = Instantiate(ammo, startPoint, transform.rotation);
            missile.GetComponent<Rigidbody2D>().velocity = Vector2.up * bulletSpeed * 4;
            Destroy(missile, 2);
            timeCounter = 0f;
        }
    }

    private void FireTurret() 
    {
        timeCounter += Time.deltaTime;

        var startPoint = transform.position + Vector3.up;

        if (timeCounter >= 1 / fireRate)
        {
            var missile = Instantiate(ammo, startPoint, transform.rotation);
            missile.GetComponent<Rigidbody2D>().velocity = Vector2.up * bulletSpeed * 4;
            Destroy(missile, 2);
            timeCounter = 0f;
        }
    }

    public void Fire()
    {

        if (type == WeaponType.SingleMissle || type == WeaponType.HomingMissle)
        {
            FireMissile();
        }
        else
        {
            FireTurret();
        }
        Debug.Log("Pew");
    }
}
