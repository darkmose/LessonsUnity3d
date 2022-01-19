using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDictionary : MonoBehaviour
{
    public static PrefabDictionary Instance { get; private set; }
    private Dictionary<string, GameObject> _prefabsNPC;
    private Dictionary<string, GameObject> _prefabsPlayers;
    private Dictionary<string, GameObject> _prefabsMedicineItems;
    private Dictionary<string, GameObject> _prefabsWeaponItems;
    private Dictionary<string, GameObject> _prefabsAmmunitionItems;

    public enum PrefabType { Weapon, Ammunition, Medicine }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            _prefabsNPC = InitializePrefabs("Prefabs/Characters/NPC");
            _prefabsPlayers = InitializePrefabs("Prefabs/Characters/Player");
            _prefabsMedicineItems = InitializePrefabs("Prefabs/Items/Medicine");
            _prefabsWeaponItems = InitializePrefabs("Prefabs/Items/Weapon");
            _prefabsAmmunitionItems = InitializePrefabs("Prefabs/Items/Ammunition");
        }
    }


    private Dictionary<string, GameObject> InitializePrefabs(string prefabsPath)
    {
        GameObject[] prefabsInDirectory = Resources.LoadAll<GameObject>(prefabsPath);
        var dictionary = new Dictionary<string, GameObject>();

        for (int i = 0; i < prefabsInDirectory.Length; i++)
        {
            dictionary.Add(prefabsInDirectory[i].name, prefabsInDirectory[i]);
        }

        return dictionary;
    }

    public static GameObject GetNPCPrefab(string name) 
    {
        if (Instance._prefabsNPC.ContainsKey(name))
        {
           return Instance._prefabsNPC[name];
        }
        else
        {
            Debug.Log($"NPC Prefab {name} doesn't exist");
            return null;
        }
    }

    public static GameObject[] GetAllNPCPrefabs()
    {
        if (Instance._prefabsNPC.Count > 0)
        {
            GameObject[] objects = new GameObject[Instance._prefabsNPC.Values.Count];
            Instance._prefabsNPC.Values.CopyTo(objects, 0);            
            return objects;
        }
        else
        {
            Debug.Log($"NPC Prefabs count equal 0");
            return null;
        }
    }

    public static GameObject GetWeaponPrefab(string name) 
    {
        if (Instance._prefabsWeaponItems.ContainsKey(name))
        {
            return Instance._prefabsWeaponItems[name];
        }
        else
        {
            Debug.Log($"Item Prefab {name} doesn't exist");
            return null;
        }
    }
    public static GameObject GetAmmoPrefab(string name) 
    {
        if (Instance._prefabsAmmunitionItems.ContainsKey(name))
        {
            return Instance._prefabsAmmunitionItems[name];
        }
        else
        {
            Debug.Log($"Item Prefab {name} doesn't exist");
            return null;
        }
    }
    public static GameObject GetMedicinePrefab(string name) 
    {
        if (Instance._prefabsMedicineItems.ContainsKey(name))
        {
            return Instance._prefabsMedicineItems[name];
        }
        else
        {
            Debug.Log($"Item Prefab {name} doesn't exist");
            return null;
        }
    }

    public static GameObject GetPlayerPrefab(string name)
    {
        if (Instance._prefabsPlayers.ContainsKey(name))
        {
            return Instance._prefabsPlayers[name];
        }
        else
        {
            Debug.Log($"Player Prefab {name} doesn't exist");
            return null;
        }
    }

    public static GameObject[] GetAllPlayerPrefabs()
    {
        if (Instance._prefabsPlayers.Count > 0)
        {
            GameObject[] objects = new GameObject[Instance._prefabsPlayers.Values.Count];
            Instance._prefabsPlayers.Values.CopyTo(objects, 0);
            return objects;
        }
        else
        {
            Debug.Log($"NPC Prefabs count equal 0");
            return null;
        }
    }

}
