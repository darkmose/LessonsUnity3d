using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class EnemySpawner : MonoBehaviour
{
    [Space(15f)]
    [Header("---------== SPAWN PARAMETERS ==--------------------------------")]
    [Space(10f)]
    [SerializeField] private float spawnRate = 1;
    [SerializeField] private int enemyCount = 1;
    [SerializeField] private Enemy.EnemyType enemyType;
    [SerializeField] private Enemy.EnemyFireType enemyFireType;
    [SerializeField] private Enemy.BehaviourType behaviourType;
    private PrefabsDictionary.Enemies enemyPrefab;
    [Space(15f)]
    [Header("---------== FIRE DIRECTION ==--------------------------------")]
    [Space(10f)]
    [SerializeField] private Vector2 fireDirection;
    [Space(15f)]
    [Header("----== ORBITAL TYPE BEHAVIOUR ==-----------------------------")]
    [Space(10f)]
    [Range(5f, 35f)]
    [SerializeField] private float radius = 10f;
    [Range(10, 120)]
    [SerializeField] private float angleStep = 60f;
    [SerializeField] private bool clockwise;
    [Tooltip("-1 -- Infinity", order = 1)]
    [Range(-1, 1000)]
    [SerializeField] private int orbitalLoopCount = -1;
    [SerializeField] private LoopType orbitalLoopType = LoopType.Restart;
    [SerializeField] private Ease orbitalEaseScale = Ease.Linear;
    [Space(15f)]
    [Header("-----== DIRECT TYPE BEHAVIOUR ==-----------------------------")]
    [Space(10f)]
    [SerializeField] private List<Vector2> move_points = new List<Vector2>();
    [Tooltip("-1 -- Infinity", order =1)]
    [Range(-1, 1000)]
    [SerializeField] private int directLoopCount = -1;
    [SerializeField] private LoopType directLoopType = LoopType.Restart;
    [SerializeField] private Ease directEaseScale = Ease.Linear;
    [SerializeField] private bool linkFirstToLast = false;

    private void Awake()
    {
        if (move_points == null)
        {
            move_points = new List<Vector2>();
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            if (behaviourType == Enemy.BehaviourType.D_type)
            {
                Gizmos.color = Color.cyan;

                var pos = transform.position;

                if (move_points.Count > 0)
                {
                    for (int i = 0; i < move_points.Count; i++)
                    {
                        Gizmos.DrawWireSphere(pos + (Vector3)move_points[i], 1f);
                    }
                }
                if (move_points.Count > 1)
                {
                    for (int i = 1; i < move_points.Count; i++)
                    {
                        Gizmos.DrawLine(pos + (Vector3)move_points[i], pos + (Vector3)move_points[i - 1]);
                    }
                }
            }
        }        
    }

    public IEnumerator Spawn() 
    {
        switch (enemyType)
        {
            case Enemy.EnemyType.BigBoy:
                enemyPrefab = PrefabsDictionary.Enemies.BigBoy;
                break;
            case Enemy.EnemyType.Bluster:
                enemyPrefab = PrefabsDictionary.Enemies.Bluster;
                break;
            case Enemy.EnemyType.RedKiller:
                enemyPrefab = PrefabsDictionary.Enemies.RedKiller;
                break;
        }

        for (int i = 0; i < move_points.Count; i++)
        {
            move_points[i] += (Vector2)transform.position;
        }

        for (int i = 0; i < enemyCount; i++)
        {
            var pos = (Vector2)transform.position;
            var lookAt = Quaternion.LookRotation(Vector3.forward, fireDirection);
            var enemy_Gameobject = Instantiate(PrefabsDictionary.GetEnemyPrefab(enemyPrefab), pos, lookAt, transform);
            Enemy enemy = enemy_Gameobject.AddComponent<Enemy>();
            enemy.fireDirection = this.fireDirection;

            switch (behaviourType)
            {
                case Enemy.BehaviourType.D_type:
                    
                    if (directLoopType == LoopType.Restart)
                    {
                        int d_count = 0;
                        if (linkFirstToLast)
                        {
                            move_points.Add(transform.position);
                            d_count = move_points.Count;
                            move_points[d_count - 1] = move_points[0];
                        }
                        d_count = move_points.Count;
                        enemy.moveParams_D = new Enemy.MoveParamsD_Type();
                        enemy.moveParams_D.points = new Vector2[d_count];
                    }
                    else
                    {
                        var d_count = move_points.Count;
                        enemy.moveParams_D = new Enemy.MoveParamsD_Type();
                        enemy.moveParams_D.points = new Vector2[d_count];
                    }                    
                    enemy.moveParams_D.points = move_points.ToArray();
                    enemy.moveParams_D.loopsCount = directLoopCount;
                    enemy.moveParams_D.loopType = directLoopType;
                    enemy.moveParams_D.ease_scale = directEaseScale;                    
                    break;

                case Enemy.BehaviourType.O_type:
                    enemy.moveParams_O = new Enemy.MoveParamsO_Type();
                    enemy.moveParams_O.offsetX = transform.position.x;
                    enemy.moveParams_O.offsetY = transform.position.y;
                    enemy.moveParams_O.radius = radius;
                    enemy.moveParams_O.angleStep = angleStep;
                    enemy.moveParams_O.clockwise = clockwise;
                    enemy.moveParams_O.loopsCount = directLoopCount;
                    enemy.moveParams_O.loopType = directLoopType;
                    enemy.moveParams_O.ease_scale = directEaseScale;
                    break;
            }
            enemy.InitializeEnemy(enemyType, enemyFireType, behaviourType);
            if (enemy != null)
            {
                EventDelegate.RaiseOnEnemySpawn();
                Debug.Log($"move points count: {move_points.Count}");
            }

            yield return new WaitForSeconds(1/spawnRate);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Spawn());
        }
        Collider2D thisCollider = gameObject.GetComponent<Collider2D>();
        Destroy(thisCollider);
    }

}
