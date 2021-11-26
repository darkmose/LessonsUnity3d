using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsDictionary : MonoBehaviour
{
    [SerializeField] private Dictionary<Weapons, GameObject> weaponPrefabs;
    [SerializeField] private Dictionary<Ammo, GameObject> ammunitionPrefabs;
    [SerializeField] private Dictionary<Spaceships, GameObject> spaceshipPrefabs;
    [SerializeField] private Dictionary<Particles, GameObject> particlesPrefabs;

    public enum Weapons { };
    public enum Ammo { Bullet, Missile, HomingMissile };
    public enum Spaceships { e1,e2,e3 };
    public enum Particles { EnemyExplosion, MissileExplosion }



    static PrefabsDictionary prefabsDictionary;


    private void Awake()
    {
        if (!prefabsDictionary)
        {
            prefabsDictionary = this;
        }

        weaponPrefabs = new Dictionary<Weapons, GameObject>();
        ammunitionPrefabs = new Dictionary<Ammo, GameObject>();
        spaceshipPrefabs = new Dictionary<Spaceships, GameObject>();
        particlesPrefabs = new Dictionary<Particles, GameObject>();

        ammunitionPrefabs.Add(Ammo.Bullet, Resources.Load<GameObject>("Prefabs/Bullet"));
        ammunitionPrefabs.Add(Ammo.Missile, Resources.Load<GameObject>("Prefabs/Missile"));
        ammunitionPrefabs.Add(Ammo.HomingMissile, Resources.Load<GameObject>("Prefabs/HomingMissile"));

        particlesPrefabs.Add(Particles.EnemyExplosion, Resources.Load<GameObject>("Prefabs/Particles/ExplosionEnemyParticles"));
        particlesPrefabs.Add(Particles.MissileExplosion, Resources.Load<GameObject>("Prefabs/Particles/ExplosionMissileParticles"));

    }

    public static GameObject GetWeaponPrefab(Weapons name) 
    {
        if (prefabsDictionary.weaponPrefabs.Count > 0)
        {
            return prefabsDictionary.weaponPrefabs[name];
        }
        return null;
    }
    public static GameObject GetAmmoPrefab(Ammo name) 
    {
        if (prefabsDictionary.ammunitionPrefabs.Count > 0)
        {
            return prefabsDictionary.ammunitionPrefabs[name];
        }
        return null;
    }
    public static GameObject GetSpaceshipPrefab(Spaceships name) 
    {
        if (prefabsDictionary.spaceshipPrefabs.Count > 0)
        {
            return prefabsDictionary.spaceshipPrefabs[name];
        }
        return null;
    }   
    public static GameObject GetParticlesPrefab(Particles name) 
    {
        if (prefabsDictionary.particlesPrefabs.Count > 0)
        {
            return prefabsDictionary.particlesPrefabs[name];
        }
        Debug.Log("Particle not fount to load");
        return null;
    }

}

