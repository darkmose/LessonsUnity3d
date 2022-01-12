using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationSystem : MonoBehaviour
{
    #region CONST PARAMETERS
    private const float VectorsDistanceAccuracy = 0.7f;
    private const int EnemiesScanRateSeconds = 3;
    private const string PlayersLayerName = "Player";
    private const float EnemyScanSphereRadius = 3f;
    private const float EnemyScanMaxDistance = 10f;
    private const int ForgetEnemySeconds = 5;
    private const int LookPointPadding = 1;
    private const int MaximumSpiedEnemies = 2;
    #endregion

    private NavMeshAgent _navMeshAgent;
    public bool IsMovedToPoint { get; private set; }
    public bool IsEnemySpied => _spiedEnemies.Count > 0;
    public Vector3 CurrentPos => transform.position;

    [SerializeField] private Transform _lookPoint;
    private Vector3 _currentDestination;
    private List<Transform> _spiedEnemies;
    private Transform _currentTargetEnemy;
    public Transform EnemyTarget 
    {
        get 
        {
            if (_currentTargetEnemy)
            {
                return _currentTargetEnemy;
            }
            else
            {
                GetRandomEnemyAsTarget();
                return _currentTargetEnemy;
            }
        }
    }

    private void Awake()
    {
        _spiedEnemies = new List<Transform>();
        if (TryGetComponent(out NavMeshAgent navMeshAgent))
        {
            _navMeshAgent = navMeshAgent;
        }

        _lookPoint.position += new Vector3(0,0, LookPointPadding);        
    }

    private void Start()
    {
        StartCoroutine(SearchForEnemies());
        StartCoroutine(ForgetEnemiesWithTime());
    }

    public void SetDestination(Vector3 destination) 
    {
        if (_navMeshAgent != null)
        {
            _navMeshAgent.SetDestination(destination);
            _currentDestination = destination;
            IsMovedToPoint = true;
        }
        else
        {
            Debug.Log($"[{this.gameObject.name}] NavMeshAgent doesn't exist!");
        }
    }



    public void GetRandomEnemyAsTarget() 
    {
        if (_spiedEnemies.Count > 0)
        {
            var randomEnemy = UnityEngine.Random.Range(0, _spiedEnemies.Count);
            _currentTargetEnemy = _spiedEnemies[randomEnemy];
        }
    }


    private IEnumerator SearchForEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(EnemiesScanRateSeconds);

            Ray ray = new Ray(transform.position,_lookPoint.position - transform.position);

            Debug.DrawRay(transform.position, _lookPoint.position - transform.position);

            RaycastHit[] hits = Physics.SphereCastAll(ray, EnemyScanSphereRadius, EnemyScanMaxDistance, LayerMask.GetMask(PlayersLayerName));
            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    if (hit.collider.gameObject != this.gameObject)
                    {
                        _spiedEnemies.Add(hit.transform);
                    }
                }
            }
        }
    }

    public void GetNearestEnemyAsTarget() 
    {
        float tmpDist = float.MaxValue;
        Transform nearestTransform = null;
        
        for (int i = 0; i < _spiedEnemies.Count; i++)
        {
            float pathDistance = 0;
            pathDistance += Vector3.Distance(transform.position, _navMeshAgent.path.corners[0]);
            for (int j = 1; j < _navMeshAgent.path.corners.Length; j++)
            {
                pathDistance += Vector3.Distance(_navMeshAgent.path.corners[j - 1], _navMeshAgent.path.corners[j]);
            }
            if (tmpDist > pathDistance)
            {
                tmpDist = pathDistance;
                nearestTransform = _spiedEnemies[i];
            }         
        }
        _currentTargetEnemy = nearestTransform;
    }

    private IEnumerator ForgetEnemiesWithTime() 
    {
        while (true)
        {
            yield return new WaitForSeconds(ForgetEnemySeconds);
            if (_spiedEnemies.Count > MaximumSpiedEnemies)
            {
                var randomEnemy = UnityEngine.Random.Range(0, _spiedEnemies.Count);
                _spiedEnemies.Remove(_spiedEnemies[randomEnemy]);
            }
        }    
    }

    private void CheckForGetDestination ()
    {
        if (IsMovedToPoint)
        {
            var distance = Vector3.Distance(transform.position, _currentDestination);
            if (distance <= VectorsDistanceAccuracy)
            {
                IsMovedToPoint = false;
            }
        }        
    }

    private void FixedUpdate()
    {
        CheckForGetDestination();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}