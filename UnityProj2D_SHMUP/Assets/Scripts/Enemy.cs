using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { BigBoy, Bluster, RedKiller }
    public enum EnemyFireType { Radian, Direct }
    public enum BehaviourType { D_type, O_type}
    private enum AmmoType { Bullet, Missile };

    public EnemyType type;
    public EnemyFireType fireType;
    public BehaviourType behaviour;
    private AmmoType ammo_type;
    

    [SerializeField] private List<Transform> firePoints;

    private GameObject ammo;
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


    public Enemy() 
    {
        //Стандартный конструктор
    }

    private void Awake()
    {
        firePoints = new List<Transform>();
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
        var movePointsRoot = transform.Find("FirePoints").transform;
        for (int i = 0; i < movePointsRoot.childCount; i++)
        {
            firePoints.Add(movePointsRoot.GetChild(i));
        }
        
        rigidbody = GetComponent<Rigidbody2D>();
        this.type = type;
        switch (type)
        {
            case EnemyType.BigBoy:
                hp = 500f;
                defense = 4f;
                bulletSpeed = 10;
                speed = 2f;
                firerate = 1f;
                ammo_type = AmmoType.Missile;
                ammo = PrefabsDictionary.GetAmmoPrefab(PrefabsDictionary.Ammo.Missile);
                break;
            case EnemyType.Bluster:
                hp = 500f;
                defense = 4f;
                bulletSpeed = 10;
                speed = 3f;
                firerate = 4f;
                ammo_type = AmmoType.Bullet;
                ammo = PrefabsDictionary.GetAmmoPrefab(PrefabsDictionary.Ammo.Bullet);
                break;
            case EnemyType.RedKiller:
                hp = 500f;
                defense = 4f;
                bulletSpeed = 10;
                speed = 4f;
                firerate = 3f;
                ammo_type = AmmoType.Bullet;
                ammo = PrefabsDictionary.GetAmmoPrefab(PrefabsDictionary.Ammo.Bullet);

                break;
        }
        this.fireType = fireType;
        switch (fireType)
        {
            case EnemyFireType.Radian:
                break;
            case EnemyFireType.Direct:
                break;
        }
        this.behaviour = behaviourType;
        switch (behaviourType)
        {
            case BehaviourType.D_type:
                GenerateMovePoints(behaviourType);
                rigidbody.position = movePoints[0];
                rigidbody.DOPath(movePoints, moveParams_D.time)
                    .SetLoops(moveParams_D.loopsCount, moveParams_D.loopType)
                    .SetEase(moveParams_D.ease_scale);
                break;
            case BehaviourType.O_type:
                GenerateMovePoints(behaviourType);
                rigidbody.position = movePoints[0];
                rigidbody.DOPath(movePoints, moveParams_O.time)
                    .SetLoops(moveParams_O.loopsCount, moveParams_O.loopType)
                    .SetEase(moveParams_O.ease_scale);        
                break;
        }
    }



    //public void FireBullet()
    //{
    //    timeCounter += Time.deltaTime;

    //    if (timeCounter >= 1 / firerate)
    //    {
    //        for (int i = 0; i < firePoints.Count; i++)
    //        {
    //            var _ammo = Instantiate(ammo, firePoint.position, transform.rotation, transform.parent);
    //            _ammo.tag = "EnemyAmmo";
    //            _ammo.GetComponent<Rigidbody2D>().velocity = (firePoint.position - transform.position).normalized * bulletSpeed * 4;
    //            Destroy(_ammo, 2);
    //        }
            
    //        timeCounter = 0f;
    //    }
    //}

    //public void FireMissile() 
    //{
    //    timeCounter += Time.deltaTime;

    //    var startPoint = firePoint.position;

    //    if (timeCounter >= 1 / firerate)
    //    {
    //        var _ammo = Instantiate(ammo, startPoint, transform.rotation, transform.parent);
    //        _ammo.tag = "EnemyMissile";
    //        _ammo.GetComponent<Rigidbody2D>().velocity = (firePoint.position - transform.position).normalized * bulletSpeed * 4;
    //        Destroy(_ammo, 2);
    //        timeCounter = 0f;
    //    }
    //}

    private void TakeDamage(float damage) 
    {
        hp -= damage/defense;

        if (hp<=0)
        {
            hp = 0;
            Death();
        }

        EventDelegate.RaiseOnEnemyTakeDamageEvent(damage);
    }

    private void Death() 
    {
        isMove = false;
        var explosion = Instantiate(PrefabsDictionary.GetParticlesPrefab(PrefabsDictionary.Particles.EnemyExplosion), transform.position, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();
        EventDelegate.RaiseOnEnemyDeathEvent();
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

        }
        if (collision.CompareTag("PlayerAmmo"))
        {
            TakeDamage(100);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("PlayerMissile"))
        {
            var point = collision.gameObject.transform.position;
            Vector2 force = (Vector2)(transform.position - point).normalized * 5;
            GameObject explosion = Instantiate(PrefabsDictionary.GetParticlesPrefab(PrefabsDictionary.Particles.MissileExplosion), point, Quaternion.identity);
            explosion.GetComponent<ParticleSystem>().Play();
            GetComponent<Rigidbody2D>().AddForce(force*Random.Range(5, 15), ForceMode2D.Impulse);
            Destroy(collision.gameObject);
            TakeDamage(250);
        }
    }

    private void Update()
    {
        switch (ammo_type)
        {
            case AmmoType.Bullet:
                //FireBullet();
                break;
            case AmmoType.Missile:
                //FireMissile();
                break;
        }
    }

}
