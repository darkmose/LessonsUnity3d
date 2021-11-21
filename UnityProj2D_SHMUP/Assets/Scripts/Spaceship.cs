using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    public enum SpaceshipType { Andromeda = 0, Spaceglader, Deltashifter };
    public SpaceshipType type;
    public Weapon mainWeapon;
    public Weapon altWeapon;
    private GameObject mainWeapon_obj;
    private GameObject altWeapon_obj;
    private float horizSpeed = 10f;
    private float vertSpeed = 5f;
    private float shieldCapacity = 100f;
    private float dashSpeed = 10f;


    private void Awake()
    {
        mainWeapon_obj = transform.Find("MainWeapon").gameObject;
        altWeapon_obj = transform.Find("AltWeapon").gameObject;

        mainWeapon = mainWeapon_obj.AddComponent<Weapon>();
        altWeapon = altWeapon_obj.AddComponent<Weapon>();
    }


    public Spaceship() 
    {
        //Стандартный конструктор
    }

    public void InitializeSpaceship(SpaceshipType type, Rigidbody2D rigidbody) 
    {
        this.rigidbody2d = rigidbody;
        this.type = type;

        switch (type)
        {
            case SpaceshipType.Andromeda:
                mainWeapon.InitializeWeapon(Weapon.WeaponType.SimpleTurret);
                altWeapon.InitializeWeapon(Weapon.WeaponType.SingleMissle);
                horizSpeed = 7f;
                vertSpeed = 5f;
                shieldCapacity = 100f;
                dashSpeed = 10f;
                break;

            case SpaceshipType.Spaceglader:
                mainWeapon.InitializeWeapon(Weapon.WeaponType.SimpleTurret);
                altWeapon.InitializeWeapon(Weapon.WeaponType.SingleMissle);
                horizSpeed = 8f;
                vertSpeed = 6f;
                shieldCapacity = 200f;
                dashSpeed = 15f;
                break;

            case SpaceshipType.Deltashifter:
                mainWeapon.InitializeWeapon(Weapon.WeaponType.SimpleTurret);
                altWeapon.InitializeWeapon(Weapon.WeaponType.SingleMissle);
                horizSpeed = 10f;
                vertSpeed = 8f;
                shieldCapacity = 250f;
                dashSpeed = 20f;
                break;
        }
    }

    public void Move()
    {
        rigidbody2d.AddForce(Vector2.right*horizSpeed*Input.GetAxisRaw("Horizontal") * 10, ForceMode2D.Force);
        rigidbody2d.AddForce(Vector2.up*vertSpeed*Input.GetAxisRaw("Vertical")* 10, ForceMode2D.Force);
    }

    public void Dash(Vector2 direction)
    {

    }

    public void ActivateShield()
    {

    }

    public void DeactivateShield()
    {

    }

    public void FireMainWeapon()
    {
        mainWeapon.Fire();
    }

    public void FireAltWeapon()
    {
        altWeapon.Fire();
    }



    public event System.Action OnDeathEvent;
    public event System.Action OnTakeDamage;
}
