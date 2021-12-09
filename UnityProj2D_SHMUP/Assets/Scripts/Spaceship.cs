using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spaceship : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    public enum SpaceshipType { Andromeda = 0, Spaceglader, Deltashifter };
    public SpaceshipType type;
    public Weapon mainWeapon;
    public Weapon altWeapon;
    private GameObject mainWeapon_obj;
    private GameObject altWeapon_obj;
    [SerializeField] private Shield shield;
    [SerializeField] private UltraAttack ultraAttack;
    private GameObject spaceshipBody;
    private float spaceshipSpeed;
    private float shieldCapacity;
    private float shieldEnergy;
    public float ShieldEnergy
    {
        get
        {
            return shieldEnergy;
        }
        set
        {
            shieldEnergy = value;
            if (shieldEnergy > shieldCapacity)
            {
                shieldEnergy = shieldCapacity;
            }
            EventDelegate.RaiseOnShieldEnergyChanged(shieldEnergy, shieldCapacity);
        }
    }

    private float ultraEnergy;
    public float UltraEnergy
    {
        get
        {
            return ultraEnergy;
        }
        set
        {
            ultraEnergy = value;
            EventDelegate.RaiseOnEnergyChanged(ultraEnergy, ultraEnergyCapacity);
        }
    }

    private float ultraEnergyCapacity;
    private int ultraEnergyLevel;
    public int UltraEnergyLevel
    {
        get
        {
            return ultraEnergyLevel;
        }
        set
        {
            ultraEnergyLevel = value;
            EventDelegate.RaiseOnEnergyLevelChanged(value);
        }

    }
    private float dashSpeed;
    private bool shieldOn;
    private bool ultraAttackOn;
    private float invulnerabilityTime;
    [HideInInspector] public bool Invulnerability { get; set; }
    private bool isRespawning;



    private void Awake()
    {
        mainWeapon_obj = transform.Find("MainWeapon").gameObject;
        altWeapon_obj = transform.Find("AltWeapon").gameObject;

        mainWeapon = mainWeapon_obj.AddComponent<Weapon>();
        altWeapon = altWeapon_obj.AddComponent<Weapon>();
        shield = transform.Find("EnergyShield").GetComponent<Shield>();
        spaceshipBody = transform.Find("Body").gameObject;
        ultraAttack = transform.Find("UltraAttack").GetComponent<UltraAttack>();


        EventDelegate.OnShieldOffEvent += OnShieldOffHandler;
        EventDelegate.OnEnemyTakeDamageEvent += OnEnemyTakeDamageHandler;
        EventDelegate.OnUltraAttackOffEvent += OnUltraAttackOffHandler;

        ultraEnergyCapacity = 300f;
        UltraEnergy = ultraEnergyCapacity;
        UltraEnergyLevel = 2;
    }

    private void OnUltraAttackOffHandler()
    {
        ultraAttackOn = false;
    }

    private void OnEnemyTakeDamageHandler(float damage)
    {
        var chargeSpeed = 15f;

        if (!shieldOn)
        {
            ShieldEnergy += (damage / shieldCapacity) * chargeSpeed;
        }
        if (!ultraAttackOn)
        {

            UltraEnergy += (damage / ultraEnergyCapacity) * chargeSpeed;
            if (ultraEnergy > ultraEnergyCapacity)
            {
                if (ultraEnergyLevel < 2)
                {
                    var rest = ultraEnergy - ultraEnergyCapacity;
                    UltraEnergyLevel++;
                    UltraEnergy = rest;
                }
                else
                {
                    UltraEnergy = ultraEnergyCapacity;
                }
            }
        }

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
                spaceshipSpeed = 7f;
                shieldCapacity = 300f;
                dashSpeed = 5f;
                break;

            case SpaceshipType.Spaceglader:
                mainWeapon.InitializeWeapon(Weapon.WeaponType.SimpleTurret);
                altWeapon.InitializeWeapon(Weapon.WeaponType.SingleMissle);
                spaceshipSpeed = 8f;
                shieldCapacity = 200f;
                dashSpeed = 7f;
                break;

            case SpaceshipType.Deltashifter:
                mainWeapon.InitializeWeapon(Weapon.WeaponType.SpreadTurret);
                altWeapon.InitializeWeapon(Weapon.WeaponType.SingleMissle);
                spaceshipSpeed = 10f;
                shieldCapacity = 250f;
                dashSpeed = 10f;
                break;
        }

        shieldEnergy = shieldCapacity;
        EventDelegate.RaiseOnShieldEnergyChanged(shieldEnergy, shieldCapacity);
    }

    public void Move()
    {
        rigidbody2d.AddForce(Vector2.right * spaceshipSpeed * Input.GetAxisRaw("Horizontal") * 15, ForceMode2D.Force);
        rigidbody2d.AddForce(Vector2.up * spaceshipSpeed * Input.GetAxisRaw("Vertical") * 15, ForceMode2D.Force);
    }

    public void Dash(Vector2 direction)
    {

        rigidbody2d.AddForce(direction * dashSpeed, ForceMode2D.Impulse);
    }

    public void ActivateShield()
    {
        if (shieldEnergy > 0.1 * shieldCapacity && !isRespawning)
        {
            if (shieldEnergy == shieldCapacity)
            {
                mainWeapon.damage = mainWeapon.baseDamage + 0.2f * mainWeapon.baseDamage;
                altWeapon.damage = altWeapon.baseDamage + 0.2f * altWeapon.baseDamage;
            }
            Invulnerability = true;
            shieldOn = true;
            shield.gameObject.SetActive(true);
        }
    }

    public void FireUltraAttack()
    {
        if (!isRespawning)
        {
            if (ultraEnergy > 0.5 * ultraEnergyCapacity || ultraEnergyLevel >= 1)
            {
                ultraAttackOn = true;
                ultraAttack.StartAttack();
            }
        }  
    }

    private void OnShieldOffHandler()
    {
        shieldOn = false;
        mainWeapon.damage = mainWeapon.baseDamage;
        altWeapon.damage = altWeapon.baseDamage;
        invulnerabilityTime = 1f;
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1)
            .Append(DOTween.To(() => invulnerabilityTime, time => invulnerabilityTime = time, 0, 1))
            .OnComplete(() =>
            {
                Invulnerability = false;
            }).SetEase(Ease.Linear);
    }

    void ConsumeShieldEnergy(float consume)
    {
        ShieldEnergy -= consume;

        if (shieldEnergy <= 0)
        {
            ShieldEnergy = 0;
        }
    }

    void ConsumeUltraEnergy(float consume) 
    {
        UltraEnergy -= consume;

        if (UltraEnergy <= 0)
        {
            if (ultraEnergyLevel > 0)
            {
                UltraEnergyLevel--;
                UltraEnergy = ultraEnergyCapacity;
            }
            else
            {
                UltraEnergy = 0;
            }
        }        
    }

    public void FireMainWeapon()
    {
        if (!isRespawning && !ultraAttackOn)
        {
            mainWeapon.Fire();
        }
    }

    public void FireAltWeapon()
    {
        if (!isRespawning && !ultraAttackOn)
        {
            altWeapon.Fire();
        }
    }

    public void HideSpaceship() 
    {
        isRespawning = true;
        Invulnerability = true;
        spaceshipBody.SetActive(false);
    }

    public void ShowSpaceship() 
    {
        StartCoroutine(BlinkRespawn());
    }

    private IEnumerator BlinkRespawn() 
    {
        spaceshipBody.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        spaceshipBody.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        spaceshipBody.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        spaceshipBody.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        spaceshipBody.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        spaceshipBody.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        spaceshipBody.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        spaceshipBody.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        spaceshipBody.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        spaceshipBody.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        spaceshipBody.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        spaceshipBody.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        spaceshipBody.SetActive(true);

        isRespawning = false;
        Invulnerability = false;

        yield return null;
    }


    private void FixedUpdate()
    {
        if (shieldOn) 
        {
            ConsumeShieldEnergy(1.5f);
        }
        if (ultraAttackOn)
        {
            ConsumeUltraEnergy(4f);
        }
    }
}
