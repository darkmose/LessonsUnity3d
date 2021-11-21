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
                fireRate = 20f;
                ammo = PrefabsDictionary.GetAmmoPrefab("Bullet");
                break;
            case WeaponType.SpreadTurret:
                damage = 120f;
                bulletSpeed = 10f;
                fireRate = 18f;
                ammo = PrefabsDictionary.GetAmmoPrefab("Bullet");
                break;
            case WeaponType.HomingMissle:
                damage = 240f;
                bulletSpeed = 10f;
                fireRate = 8f;
                ammo = PrefabsDictionary.GetAmmoPrefab("Missile");
                break;
            case WeaponType.SingleMissle:
                damage = 250f;
                bulletSpeed = 10f;
                fireRate = 8f;
                ammo = PrefabsDictionary.GetAmmoPrefab("HomingMissile");
                break;
        }
    }

    public void Fire()
    {
        var bullet = Instantiate(ammo, transform.position + Vector3.up, Quaternion.identity);
        //bullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * 50f;
        Destroy(bullet, 3);
        Debug.Log("Pew");
    }
}
