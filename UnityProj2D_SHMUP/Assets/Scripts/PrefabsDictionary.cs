using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsDictionary : MonoBehaviour
{
    [SerializeField] private Dictionary<string, GameObject> weaponPrefabs;
    [SerializeField] private Dictionary<string, GameObject> ammunitionPrefabs;
    [SerializeField] private Dictionary<string, GameObject> spaceshipPrefabs;

    static PrefabsDictionary prefabsDictionary;


    private void Awake()
    {
        prefabsDictionary = this;

        weaponPrefabs = new Dictionary<string, GameObject>();
        ammunitionPrefabs = new Dictionary<string, GameObject>();
        spaceshipPrefabs = new Dictionary<string, GameObject>();

        ammunitionPrefabs.Add("Bullet", Resources.Load<GameObject>("Prefabs/Bullet"));
        ammunitionPrefabs.Add("Missile", Resources.Load<GameObject>("Prefabs/Missile"));
        ammunitionPrefabs.Add("HomingMissile", Resources.Load<GameObject>("Prefabs/HomingMissile"));        
    }

    public static GameObject GetWeaponPrefab(string name) 
    {
        return prefabsDictionary.weaponPrefabs[name];
    }
    public static GameObject GetAmmoPrefab(string name) 
    {
        return prefabsDictionary.ammunitionPrefabs[name];
    }
    public static GameObject GetSpaceshipPrefab(string name) 
    {
        return prefabsDictionary.spaceshipPrefabs[name];
    }

}

