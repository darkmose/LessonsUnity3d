using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Spaceship spaceship;
    public GameObject shipObject;
    [SerializeField] private Camera gameCamera;
    private int lives = 2;
    private int credits = 2;

    private void Awake()
    {
        if (!gameCamera)
        {
            gameCamera = Camera.main;
        }
        
    }
    private void Start()
    {
        spaceship = shipObject.AddComponent<Spaceship>();
        spaceship.InitializeSpaceship(Spaceship.SpaceshipType.Deltashifter, this.GetComponent<Rigidbody2D>());
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            spaceship.FireMainWeapon();
        }
        if (Input.GetMouseButton(1) && !Input.GetMouseButton(0))
        {
            spaceship.FireAltWeapon();
        }
    }

    private void FixedUpdate()
    {
        spaceship.Move();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log($"{collision.collider.name} Stay");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
    }

}



