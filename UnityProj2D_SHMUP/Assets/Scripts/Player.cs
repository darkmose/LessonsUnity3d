using UnityEngine;
using DG.Tweening;
using System;

public class Player : MonoBehaviour
{
    private Spaceship spaceship;
    [SerializeField] private GameObject shipObject;
    [SerializeField] private Camera gameCamera;
    [SerializeField] private float timeBeforeRespawn;

    private void Awake()
    {
        if (!gameCamera)
        {
            gameCamera = Camera.main;
        }
        timeBeforeRespawn = 1f;

        EventDelegate.OnStartBossFightEvent += OnStartBossFightHandler;
    }

    private void OnStartBossFightHandler()
    {
        GetComponent<ConstantForce2D>().enabled = false;
    }

    private void Start()
    {        
        spaceship = shipObject.AddComponent<Spaceship>();
        Spaceship.SpaceshipType type = (Spaceship.SpaceshipType)PlayerPrefs.GetInt("indexSpaceship");
        spaceship.InitializeSpaceship(type, this.GetComponent<Rigidbody2D>());
    }

    private void Update()
    {
        MotionControl();
    }

    private void FixedUpdate()
    {
        spaceship.Move();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAmmo"))
        {
            TakeDamage();
        }
    }

    private void MotionControl() 
    {
        if (Input.GetButton("Fire1"))
        {
            spaceship.FireMainWeapon();
        }
        if (Input.GetButtonDown("UltraAttack"))
        {
            spaceship.FireUltraAttack();
        }
        if (Input.GetButtonDown("Dash"))
        {
            var directionToDash = new Vector2();

            if (Input.GetAxis("Horizontal") != 0)
            {
                directionToDash.x += Input.GetAxis("Horizontal");
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                directionToDash.y += Input.GetAxis("Vertical");
            }

            spaceship.Dash(directionToDash);
        }
        if (Input.GetButtonDown("Shield"))
        {
            spaceship.ActivateShield();
        }
    }


    private void TakeDamage()
    {
        if (!spaceship.Invulnerability)
        {
            Death();
        }
    }

    public void Respawn() 
    {
        DOTween.Sequence().SetDelay(timeBeforeRespawn)
        .OnComplete(() =>
        {
            spaceship.ShowSpaceship();
        });
    }

    private void Death()
    {
        GameObject explosion = Instantiate(PrefabsDictionary.GetParticlesPrefab(PrefabsDictionary.Particles.EnemyExplosion), transform.position, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();
        spaceship.HideSpaceship();
        EventDelegate.RaiseOnPlayerDeath();
    }
}



