using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { BigBoy, Bluster, RedKiller }
    public enum EnemyFireType { Radian, Direct }
    public enum BehaviourType { D_type, O_type}

    public EnemyType type;
    public EnemyFireType fireType;
    public BehaviourType behaviour;

    [SerializeField] private List<Transform> firePoints;
    public Vector2 fireDirection;

    private Vector2[] movePoints;
    
    private new Rigidbody2D rigidbody;
    
    public struct MoveParamsO_Type
    {
        public float offsetX;
        public float offsetY;
        public float radius;
        public float angleStep;
        public float angle;
        public bool clockwise;
        public float time;
        public int loopsCount;
        public LoopType loopType;
        public Ease ease_scale;
        public Vector2 CreatePos() 
        {
            var angleResult = (clockwise ? -1 : 1) * angle * Mathf.Deg2Rad;
            var x = Mathf.Cos(angleResult) * radius + offsetX;
            var y = Mathf.Sin(angleResult) * radius + offsetY;
            return new Vector2(x, y);
        }        
    };
    public MoveParamsO_Type moveParams_O;

    public struct MoveParamsD_Type 
    {
        public Vector2[] points;
        public LoopType loopType;
        public Ease ease_scale;
        public float time;
        public int loopsCount;
    };
    public MoveParamsD_Type moveParams_D;

    private bool isMove;
    private float speed = 10f;
    private float firerate = 1f;
    private float hp = 100;
    private float defense;
    private float timeCounter;
    private float bulletSpeed;
    private int scoreGain;


    public Enemy() 
    {
        //Стандартный конструктор
    }

    private void Awake()
    {

        firePoints = new List<Transform>();
    }

    private void Start()
    {

    }

    /// <summary>
    /// Создает точки для движения объекта
    /// </summary>
    public void GenerateMovePoints(BehaviourType b_type)
    {

        switch (b_type)
        {
            case BehaviourType.D_type:
                movePoints = new Vector2[moveParams_D.points.Length];
                moveParams_D.points.CopyTo(movePoints,0);     
                break;

            case BehaviourType.O_type:
                var steps_O = (int)(360f / moveParams_O.angleStep);
                movePoints = new Vector2[steps_O + 1];
                for (int i = 0; i <= steps_O; i++)
                {
                    moveParams_O.angle = moveParams_O.angleStep * i;
                    movePoints[i] = moveParams_O.CreatePos();
                }
                break;
        }
    }

    public void InitializeEnemy(EnemyType type, EnemyFireType fireType, BehaviourType behaviourType)  
    {
        var firePointsRoot = transform.Find("FirePoints").transform;
        for (int i = 0; i < firePointsRoot.childCount; i++)
        {
            firePoints.Add(firePointsRoot.GetChild(i));
        }
        
        rigidbody = GetComponent<Rigidbody2D>();
        this.type = type;
        switch (type)
        {
            case EnemyType.BigBoy:
                hp = 300f;
                defense = 4f;
                bulletSpeed = 40;
                speed = 2f;
                firerate = 0.5f;
                scoreGain = Random.Range(300,500);
                break;

            case EnemyType.Bluster:
                hp = 250f;
                defense = 4f;
                bulletSpeed = 30;
                speed = 8f;
                firerate = 4f;
                scoreGain = Random.Range(400, 600);
                break;

            case EnemyType.RedKiller:
                hp = 200f;
                defense = 4f;
                bulletSpeed = 30;
                speed = 8f;
                firerate = 3f;
                scoreGain = Random.Range(200, 800);
                break;
        }
        this.fireType = fireType;

        this.behaviour = behaviourType;
        
        switch (behaviourType)
        {
            case BehaviourType.D_type:
                GenerateMovePoints(behaviourType);
                moveParams_D.time = TimeFromDistanceAndSpeed();
                rigidbody.position = movePoints[0];
                rigidbody.DOPath(movePoints, moveParams_D.time)
                    .SetLoops(moveParams_D.loopsCount, moveParams_D.loopType)
                    .SetEase(moveParams_D.ease_scale);
                break;
            case BehaviourType.O_type:
                GenerateMovePoints(behaviourType);
                moveParams_O.time = TimeFromDistanceAndSpeed();
                rigidbody.position = movePoints[0];
                rigidbody.DOPath(movePoints, moveParams_O.time)
                    .SetLoops(moveParams_O.loopsCount, moveParams_O.loopType)
                    .SetEase(moveParams_O.ease_scale);        
                break;
        }
    }

    float TimeFromDistanceAndSpeed() 
    {
        var S = 0f;
        var V = speed;

        for (int i = 1; i < movePoints.Length; i++)
        {
            S += Vector2.Distance(movePoints[i], movePoints[i - 1]);
        }
        if (V != 0f)
        {
            var time = S / V;
            return time;
        }
        return 0;
    }

    public void FireBullet()
    {
        timeCounter += Time.deltaTime;

        if (timeCounter >= 1 / firerate)
        {
            switch (fireType)
            {
                case EnemyFireType.Radian:
                    for (int i = 0; i < firePoints.Count; i++)
                    {
                        Vector2 localFireDirection = (firePoints[i].position - transform.position).normalized;
                        var quaternion = Quaternion.LookRotation(Vector3.forward, localFireDirection);                        
                        var _ammo = ObjectPooler.GetPooledGameObject("Bullet_Red");
                        _ammo.transform.position = firePoints[i].position;
                        _ammo.transform.rotation = quaternion;
                        _ammo.transform.parent = transform.parent;
                        _ammo.tag = "EnemyAmmo";
                        _ammo.GetComponent<Rigidbody2D>().velocity = localFireDirection * bulletSpeed;
                    }
                    break;
                case EnemyFireType.Direct:
                    for (int i = 0; i < firePoints.Count; i++)
                    {
                        var _ammo = ObjectPooler.GetPooledGameObject("Bullet_Red");
                        _ammo.transform.position = firePoints[i].position;
                        _ammo.transform.rotation = transform.rotation;
                        _ammo.transform.parent = transform.parent;
                        _ammo.tag = "EnemyAmmo";
                        _ammo.GetComponent<Rigidbody2D>().velocity = (fireDirection).normalized * bulletSpeed;
                    }
                    break;
            }
            timeCounter = 0f;
        }
    }

    public void TakeDamage(float damage) 
    {
        hp -= damage/defense;

        if (hp<=0)
        {
            hp = 0;
            Death();
        }

        Debug.Log("EnemyTakeDamage");
        EventDelegate.RaiseOnEnemyTakeDamage(damage);
    }

    private void Death() 
    {
        var explosion = Instantiate(PrefabsDictionary.GetParticlesPrefab(PrefabsDictionary.Particles.EnemyExplosion), transform.position, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();
        EventDelegate.RaiseOnEnemyDeath(scoreGain);
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

        }
        if (collision.CompareTag("PlayerAmmo"))
        {
            TakeDamage(collision.GetComponent<Projectile>().damage);
            collision.gameObject.SetActive(false);
        }
        if (collision.CompareTag("PlayerMissile"))
        {
            var point = collision.gameObject.transform.position;
            GameObject explosion = Instantiate(PrefabsDictionary.GetParticlesPrefab(PrefabsDictionary.Particles.MissileExplosion), point, Quaternion.identity);
            explosion.GetComponent<ParticleSystem>().Play();
            collision.gameObject.SetActive(false);
            TakeDamage(collision.GetComponent<Projectile>().damage);
        }
    }

    private void FixedUpdate()
    {        
        FireBullet();     
    }

}
