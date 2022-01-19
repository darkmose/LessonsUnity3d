using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPointsHelper : MonoBehaviour
{
    public static MapPointsHelper Instance { get; private set; }
    [SerializeField] private Transform _weaponPointsRoot;
    [SerializeField] private Transform _ammoPointsRoot;
    [SerializeField] private Transform _enemiesPointsRoot;
    [SerializeField] private Transform _healthPacksPointsRoot;
    public enum PointsList { HealthPackPoints, EnemiesPoints, WeaponPoints, AmmoPoints }
    private Dictionary<PointsList, List<Vector3>> _pointsDictionary;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            _pointsDictionary = new Dictionary<PointsList, List<Vector3>>();

            List<Vector3> tempList = new List<Vector3>();
            tempList.Clear();

            var arrayLength = _weaponPointsRoot.childCount;


            for (int i = 0; i < arrayLength; i++)
            {
                Vector3 pos = _weaponPointsRoot.GetChild(i).position;
                tempList.Add(pos);
            }
            _pointsDictionary.Add(PointsList.WeaponPoints, new List<Vector3>(tempList));
            tempList.Clear();

            arrayLength = _ammoPointsRoot.childCount;

            for (int i = 0; i < arrayLength; i++)
            {
                Vector3 pos = _ammoPointsRoot.GetChild(i).position;
                tempList.Add(pos);
            }

            _pointsDictionary.Add(PointsList.AmmoPoints, new List<Vector3>(tempList));
            tempList.Clear();

            arrayLength = _enemiesPointsRoot.childCount;

            for (int i = 0; i < arrayLength; i++)
            {
                Vector3 pos = _enemiesPointsRoot.GetChild(i).position;
                tempList.Add(pos);
            }

            _pointsDictionary.Add(PointsList.EnemiesPoints, new List<Vector3>(tempList));
            tempList.Clear();

            arrayLength = _healthPacksPointsRoot.childCount;

            for (int i = 0; i < arrayLength; i++)
            {
                Vector3 pos = _healthPacksPointsRoot.GetChild(i).position;
                tempList.Add(pos);
            }

            _pointsDictionary.Add(PointsList.HealthPackPoints, new List<Vector3>(tempList));
            tempList.Clear();

        }
    }


    public static Vector3 GetRandomPointFromList(PointsList pointsList) 
    {
        if (Instance._pointsDictionary.ContainsKey(pointsList)) 
        {
            List<Vector3> points = Instance._pointsDictionary[pointsList];
            var pointCount = points.Count;
            var randomValue = Random.Range(0, pointCount);

            return points[randomValue];;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
