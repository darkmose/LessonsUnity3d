using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossHandler : MonoBehaviour
{

    [System.Serializable]
    private struct Phase
    {
        public string phaseName;
        public float phaseHP;
        public BulletHell bulletHell;
    }
    private Phase currentPhase;

    public string bossName;
    private float fullBossHP;
    private float bossHP;

    public float FullBossHP { get; }
    public float BossHP 
    {
        get 
        { 
            return bossHP; 
        }
        set 
        {
            if (bossHP != value)
            {
                bossHP = value;
                if (bossHP > fullBossHP)
                {
                    bossHP = fullBossHP;
                }
                if (bossHP < 0)
                {
                    bossHP = 0;
                }
                EventDelegate.RaiseOnBossHealthChangedEvent(bossHP, fullBossHP);
            }
        }
    } 


    [SerializeField] private Phase[] phases;

    private void Awake()
    {
        EventDelegate.OnStartBossFightEvent += OnStartBossFightHandler;
        fullBossHP = 0f;

        for (int i = 0; i < phases.Length; i++)
        {             
            fullBossHP += phases[i].phaseHP;
        }
        currentPhase = phases[0];
        BossHP = fullBossHP;
        Debug.Log($"BossHP is: {BossHP}");
    }


    private void OnStartBossFightHandler()
    {
        Vector3 pointToShowBoss = transform.Find("BossSprite").position;
        pointToShowBoss -= Vector3.up * 10f;
        DOTween.Sequence()
            .Append(transform.Find("BossSprite").DOMove(pointToShowBoss, 2f))
            .OnComplete(()=> 
            { 
                Attack();             
            });
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("MainCamera"))
        {
            var dist = Vector2.Distance(collision.transform.position, transform.position);
            Debug.Log(dist);

            if (dist <= 15)
            {
                EventDelegate.RaiseOnStartBossFight();
                Destroy(transform.Find("startFightCollider").GetComponent<BoxCollider2D>());
            }
        }
    }

    private void Attack() 
    {
        currentPhase.bulletHell.StartBulletHell();
    }

    public void TakeDamage(float damage) 
    {
        EventDelegate.RaiseOnBossTakeDamage(damage);
        BossHP -= damage;
        if (bossHP <= 0)
        {
            Death();
            return;
        }
        float takenBossHP = fullBossHP - bossHP;
        int nextPhase = 0;

        for (int i = 0; i < phases.Length; i++)
        {
            if (takenBossHP > phases[i].phaseHP)
            {
                nextPhase = i+1;
            }
        }
        if (nextPhase > 0)
        {
            ChangePhase(nextPhase);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAmmo"))
        {
            var damage = collision.GetComponent<Projectile>().damage;
            TakeDamage(damage);
        }
    }


    private void ChangePhase(int phase) 
    {
        Debug.Log($"Changing BOSS phase to {phase}");
        if (phase > 0)
        {
            phases[phase - 1].bulletHell.StopBulletHell();
            currentPhase = phases[phase];
        }
        else
        {
            currentPhase = phases[phase];
        }
    }

    private void Death()
    {
        var explosion = Instantiate(PrefabsDictionary.GetParticlesPrefab(PrefabsDictionary.Particles.EnemyExplosion), transform.position, Quaternion.identity);
        explosion.transform.localScale *= 3;
        explosion.GetComponent<ParticleSystem>().Play();
        EventDelegate.RaiseOnBossDeath();
        Debug.Log("Boss is dead");
        gameObject.SetActive(false);
    }
}
