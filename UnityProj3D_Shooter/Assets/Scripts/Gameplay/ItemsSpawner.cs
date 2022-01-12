using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSpawner : MonoBehaviour
{
    private const int TimeDelayToSpawnItems = 5;

    public enum ItemType { Weapon, Ammunition, Health }
    public enum MedicineType { MedicinePack }
    public enum AmmunitionType { AmmoItem }

    [SerializeField] private ItemType _itemType;
    [Space]
    [Header("If Item Type Is Weapon")]
    [SerializeField] private Weapon.WeaponType _weaponType;
    [Space]
    [Header("If Item Type Is Ammunition")]
    [SerializeField] private AmmunitionType _ammoType;
    [Space]
    [Header("If Item Type Is Medicine")]
    [SerializeField] private MedicineType _medicineType;

    [SerializeField] private Transform _itemSpawnPoint;
    private GameObject _weaponItemPrefab = null;
    private GameObject _ammoItemPrefab = null;
    private GameObject _healthItemPrefab = null;
    private bool StandIsEmpty => _itemSpawnPoint.childCount == 0;

    private void Start()
    {
        switch (_itemType)
        {
            case ItemType.Weapon:
                _weaponItemPrefab = PrefabDictionary.GetWeaponPrefab(_weaponType.ToString());
                break;
            case ItemType.Ammunition:
                _ammoItemPrefab = PrefabDictionary.GetAmmoPrefab(_ammoType.ToString());
                break;
            case ItemType.Health:
                _healthItemPrefab = PrefabDictionary.GetMedicinePrefab(_medicineType.ToString());
                break;
        }

        StartCoroutine(DelayedSpawnItems(TimeDelayToSpawnItems));

    }


    private IEnumerator DelayedSpawnItems(int sec) 
    {
    Start:
        yield return new WaitForSeconds(sec);
        SpawnItem();
        goto Start;
    }


    void SpawnItem() 
    {
        if (StandIsEmpty)
        {
            switch (_itemType)
            {
                case ItemType.Weapon:
                    var obj = Instantiate(_weaponItemPrefab, _itemSpawnPoint.position, Quaternion.identity, _itemSpawnPoint);
                    if (obj.TryGetComponent(out Weapon weapon))
                    {
                        weapon.InitializeWeapon(_weaponType);
                    }
                    break;
                case ItemType.Ammunition:
                    Instantiate(_ammoItemPrefab, _itemSpawnPoint.position, Quaternion.identity, _itemSpawnPoint);
                    break;
                case ItemType.Health:
                    Instantiate(_healthItemPrefab, _itemSpawnPoint.position, Quaternion.identity, _itemSpawnPoint);
                    break;
                default:
                    break;
            }
        }
        
    }


    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
