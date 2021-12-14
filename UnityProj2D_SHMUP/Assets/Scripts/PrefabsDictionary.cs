using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsDictionary : MonoBehaviour
{
    [SerializeField] private Dictionary<Weapons, GameObject> weaponPrefabs;
    [SerializeField] private Dictionary<Ammo, GameObject> ammunitionPrefabs;
    [SerializeField] private Dictionary<Spaceships, GameObject> spaceshipPrefabs;
    [SerializeField] private Dictionary<Particles, GameObject> particlesPrefabs;
    [SerializeField] private Dictionary<Enemies, GameObject> enemiesPrefabs;

    public enum Enemies { Bluster, RedKiller, BigBoy };
    public enum Weapons { };
    public enum Ammo { Bullet_Blue, Bullet_Red, Missile, HomingMissile };
    public enum Spaceships { Andromeda, Spaceglader, Deltashifter };
    public enum Particles { EnemyExplosion, MissileExplosion };



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
        enemiesPrefabs = new Dictionary<Enemies, GameObject>();

        ammunitionPrefabs.Add(Ammo.Bullet_Blue, Resources.Load<GameObject>("Prefabs/Ammo/Bullet_Blue"));
        ammunitionPrefabs.Add(Ammo.Bullet_Red, Resources.Load<GameObject>("Prefabs/Ammo/Bullet_Red"));
        ammunitionPrefabs.Add(Ammo.Missile, Resources.Load<GameObject>("Prefabs/Ammo/Missile"));
        ammunitionPrefabs.Add(Ammo.HomingMissile, Resources.Load<GameObject>("Prefabs/Ammo/HomingMissile"));

        enemiesPrefabs.Add(Enemies.BigBoy, Resources.Load<GameObject>("Prefabs/Spaceships/Enemies/BigBoy"));
        enemiesPrefabs.Add(Enemies.Bluster, Resources.Load<GameObject>("Prefabs/Spaceships/Enemies/Bluster"));
        enemiesPrefabs.Add(Enemies.RedKiller, Resources.Load<GameObject>("Prefabs/Spaceships/Enemies/RedKiller"));

        particlesPrefabs.Add(Particles.EnemyExplosion, Resources.Load<GameObject>("Prefabs/Particles/ExplosionEnemyParticles"));
        particlesPrefabs.Add(Particles.MissileExplosion, Resources.Load<GameObject>("Prefabs/Particles/ExplosionMissileParticles"));

        spaceshipPrefabs.Add(Spaceships.Andromeda, Resources.Load<GameObject>("Prefabs/Spaceships/Player/Andromeda"));
        spaceshipPrefabs.Add(Spaceships.Spaceglader, Resources.Load<GameObject>("Prefabs/Spaceships/Player/Spaceglader"));

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

    public static GameObject GetEnemyPrefab(Enemies enemy) 
    {
        if (prefabsDictionary.enemiesPrefabs.Count > 0)
        {
            return prefabsDictionary.enemiesPrefabs[enemy];
        }
        return null;
    }

}

