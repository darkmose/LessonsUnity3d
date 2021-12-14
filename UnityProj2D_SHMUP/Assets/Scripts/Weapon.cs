using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType { SimpleTurret, SpreadTurret, HomingMissle, SingleMissle };
    public WeaponType type;
    public float baseDamage;
    public float damage = 100f;
    public float bulletSpeed = 10f;
    public float fireRate = 20f;
    private float timeCounter = 0f;
    private List<Transform> firePoints;
    private Transform firePointsRoot;

    private void Awake()
    {
        firePointsRoot = transform.Find("FirePoints");
        firePoints = new List<Transform>();
    }

    private void Start()
    {
        var firePointsRoot = transform.Find("FirePoints");
        for (int i = 0; i < firePointsRoot.childCount; i++)
        {
            firePoints.Add(firePointsRoot.GetChild(i)); 
        }

        damage = baseDamage;
    }


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
                baseDamage = 80f;
                bulletSpeed =80f;
                fireRate = 5f;
                type = WeaponType.SimpleTurret;
                break;
            case WeaponType.SpreadTurret:
                baseDamage = 100f;
                bulletSpeed = 80f;
                fireRate = 4f;
                type = WeaponType.SpreadTurret;
                break;
            case WeaponType.HomingMissle:
                baseDamage = 240f;
                bulletSpeed = 30f;
                fireRate = 2f;
                type = WeaponType.HomingMissle;
                break;
            case WeaponType.SingleMissle:
                baseDamage = 250f;
                bulletSpeed = 40f;
                fireRate = 2f;
                type = WeaponType.SingleMissle;
                break;
        }
    }

    private void FireTurret() 
    {
        timeCounter += Time.deltaTime;

        if (timeCounter >= 1 / fireRate)
        {
            switch (type)
            {
                case WeaponType.SimpleTurret:
                    for (int i = 0; i < firePoints.Count; i++)
                    {
                        var _ammo = ObjectPooler.GetPooledGameObject("Bullet_Blue");
                        _ammo.transform.position = firePoints[i].position;
                        _ammo.transform.rotation = transform.rotation;
                        _ammo.GetComponent<Projectile>().damage = damage;
                        _ammo.GetComponent<Rigidbody2D>().velocity = (transform.up).normalized * bulletSpeed;
                    }
                    timeCounter = 0f;
                    break;
                case WeaponType.SpreadTurret:
                    for (int i = 0; i < firePoints.Count; i++)
                    {
                        Vector2 localFireDirection = (firePoints[i].position - firePointsRoot.position).normalized;
                        var quaternion = Quaternion.LookRotation(Vector3.forward, localFireDirection);
                        var _ammo = ObjectPooler.GetPooledGameObject("Bullet_Blue");
                        _ammo.transform.position = firePoints[i].position;
                        _ammo.transform.rotation = quaternion;
                        _ammo.GetComponent<Projectile>().damage = damage;
                        _ammo.GetComponent<Rigidbody2D>().velocity = localFireDirection * bulletSpeed;
                    }
                    timeCounter = 0f;
                    break;
            }

        }
    }

    public void Fire()
    {

            FireTurret();
        
    }
}
