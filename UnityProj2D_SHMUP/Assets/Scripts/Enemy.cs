using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { Enemy1, Enemy2, Enemy3 }
    public enum EnemyFireType { Radian, Direct, }
    public enum BehaviourType { S_type, Z_type, }

    [SerializeField] private Transform firePoint;
    private GameObject ammo;

    public EnemyType type;
    public EnemyFireType fireType;
    public BehaviourType behaviour;

    private bool isMove;
    private float speed = 10f;
    private float damage = 10f;
    private float firerate;
    private float hp = 100;
    private float defense;
    private float timeCounter;
    private float bulletSpeed;



    public Enemy() 
    {
        //Стандартный конструктор
    }

    public void InitializeEnemy(EnemyType type, EnemyFireType fireType, BehaviourType behaviourType)  
    {
        ammo = PrefabsDictionary.GetAmmoPrefab(PrefabsDictionary.Ammo.Bullet);
        this.type = type;
        switch (type)
        {
            case EnemyType.Enemy1:
                damage = 100f;
                hp = 500f;
                defense = 4;
                bulletSpeed = 10;
                break;
            case EnemyType.Enemy2:
                damage = 100f;
                hp = 500f;
                defense = 4;
                bulletSpeed = 10;
                break;
            case EnemyType.Enemy3:
                damage = 100f;
                hp = 500f;
                defense = 4;
                bulletSpeed = 10;
                break;
        }
        this.fireType = fireType;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAmmo"))
        {

        }
        if (collision.CompareTag("PlayerMissile"))
        {
            var point = collision.gameObject.transform.position;
            Vector2 force = (Vector2)(transform.position - point).normalized*5;
            GameObject explosion = Instantiate(PrefabsDictionary.GetParticlesPrefab(PrefabsDictionary.Particles.MissileExplosion), point ,Quaternion.identity);
            explosion.GetComponent<ParticleSystem>().Play();
            GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }


        Debug.Log(collision.tag);
    }

    private IEnumerator MoveToPoint(Vector3 point) 
    {

        yield return null;
    }
    private IEnumerator MoveToPoint(Vector3[] points)
    {

        yield return null;
    }

    public void Fire(Vector3 direction)
    {
        timeCounter += Time.deltaTime;

        var startPoint = firePoint.position + direction;

        if (timeCounter >= 1 / firerate)
        {
            var missile = Instantiate(ammo, startPoint, transform.rotation);
            missile.GetComponent<Rigidbody2D>().velocity = Vector2.up * bulletSpeed * 4;
            Destroy(missile, 2);
            timeCounter = 0f;
        }
    }

    private void TakeDamage(float damage) 
    {
        Death();
        return;
        hp -= damage/defense;

        if (hp<0)
        {
            hp = 0;
            Death();
        }

        OnTakeDamageEvent?.Invoke(damage);
    }

    private void Death() 
    {

        var explosion = Instantiate(PrefabsDictionary.GetParticlesPrefab(PrefabsDictionary.Particles.EnemyExplosion), transform.position, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();
        OnDeathEvent?.Invoke();
        Destroy(this);
    }





    public event System.Action<float> OnTakeDamageEvent;
    public event System.Action OnDeathEvent;
}
