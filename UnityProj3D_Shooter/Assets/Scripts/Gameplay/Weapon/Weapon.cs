using GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region CONST PARAMS
    private const int AK74FireRate = 7;
    private const int AK74StartBulletCount = 30;
    private const int AK74BulletsInMagazine = 30;
    private const int AK74Damage = 3;
    private const int AK74MagazineCapacity = 30;
    private const float ViewPortCameraCenter = 0.5f;
    private const int M1911FireRate = 4;
    private const int M1911StartBulletCount = 20;
    private const int M1911BulletsInMagazine = 9;
    private const int M1911Damage = 10;
    private const int M1911MagazineCapacity = 9;
    private const string PlayersLayerName = "Player";
    private const float ShootMaxDistance = 10f;
    private const float PawAttackMaxDistance = 1f;
    private const int PawAttackDamage = 5;
    #endregion

    public bool npcIsOwner = true; 
    private WeaponType _weaponType = WeaponType.None;
    public WeaponType WeaponModel => _weaponType;
    public WeaponAnimator _weaponAnimator;
    public int BulletsCount => _bulletsCount;
    public int BulletsInMagazine => _bulletsInMagazine;
    public bool BurstShooting { get; private set; }

    private float _fireRate;
    private int _bulletsCount;
    private int _bulletsInMagazine;
    private int _magazineCapacity;
    private int _damage;
    private float _coolDownTime;
    public Transform iKLHand;
    public Transform iKRHand;
    private Vector3 _cameraViewportCenter;

    private OnWeaponShootEvent _onWeaponShootEvent;
    private OnWeaponReloadEvent _onWeaponReloadEvent;
    
    private void Awake()
    {
        _onWeaponShootEvent = new OnWeaponShootEvent();
        _onWeaponReloadEvent = new OnWeaponReloadEvent();
        _cameraViewportCenter = new Vector3(ViewPortCameraCenter, ViewPortCameraCenter, 1);
    }


    public enum WeaponType
    {
        None,
        AK74,
        M1911
    }


    public void InitializeWeaponAnimator(WeaponAnimator weaponAnimator) 
    {
        _weaponAnimator = weaponAnimator;
    }

    public void InitializeWeapon(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.None:
                _weaponType = weaponType;
                _damage = PawAttackDamage;

                break;
            case WeaponType.AK74:
                _weaponType = weaponType;
                _fireRate = AK74FireRate;
                _bulletsCount = AK74StartBulletCount;
                _bulletsInMagazine = AK74BulletsInMagazine;
                BurstShooting = true;
                _damage = AK74Damage;
                _magazineCapacity = AK74MagazineCapacity;
                break;
            case WeaponType.M1911:
                _weaponType = weaponType;
                _fireRate = M1911FireRate;
                _bulletsCount = M1911StartBulletCount;
                _bulletsInMagazine = M1911BulletsInMagazine;
                BurstShooting = false;
                _damage = M1911Damage;
                _magazineCapacity = M1911MagazineCapacity;
                break;
        }
    }

    public void ShootFX() 
    {
        _weaponAnimator.FireAnimation();
        //Sound FX;
        //Particles FX;
    }

    public void FireTarget(Transform target) //For NPC
    {
            if (_coolDownTime <= 0f)
            {
                if (_bulletsInMagazine > 0)
                {
                    ShootFX();
                    Ray ray = new Ray(transform.position, target.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, ShootMaxDistance, LayerMask.GetMask(PlayersLayerName)))
                    {
                        if (hit.collider.gameObject != this.gameObject)
                        {
                            if (hit.collider.TryGetComponent(out NPCController nPC))
                            {
                                nPC.TakeDamage(_damage, WeaponModel);
                            }
                        }
                    
                    }
                    _bulletsInMagazine--;
                    _coolDownTime = 1 / _fireRate;
                }
                else
                {
                    ReloadWeapon();
                }
            }       
    }   

    public void Fire() //ForPlayer
    {
        if (_coolDownTime <= 0f)
        {
            if (_bulletsInMagazine > 0)
            {
                ShootFX();
                FireRay();
                _bulletsInMagazine--;
                _coolDownTime = 1 / _fireRate;
                _onWeaponShootEvent.bulletsInMagazine = _bulletsInMagazine;
                if (!npcIsOwner)
                {
                    EventsAgregator.Post<OnWeaponShootEvent>(this, _onWeaponShootEvent);
                }
            }
            else
            {
                Debug.Log("Bullets over. Need to reload");
                //click Sound
            }
        }
    }


    public void AddAmmunition(int bulletsCount) 
    {
        _bulletsCount += bulletsCount;
        _onWeaponReloadEvent.bulletsCount = _bulletsCount;
        _onWeaponReloadEvent.bulletsInMagazine = _bulletsInMagazine;
        if (!npcIsOwner)
        {
            EventsAgregator.Post<OnWeaponReloadEvent>(this, _onWeaponReloadEvent);
        }
    }


    public void ReloadWeapon() 
    {
        if (_bulletsCount > 0)
        {
            var magazineCapacity = _magazineCapacity - _bulletsInMagazine;

            if (_bulletsCount >= magazineCapacity)
            {
                _bulletsInMagazine += magazineCapacity;
                _bulletsCount -= magazineCapacity;
            }
            else
            {
                _bulletsInMagazine += _bulletsCount;
                _bulletsCount = 0;
            }         
        }
        _onWeaponReloadEvent.bulletsCount = _bulletsCount;
        _onWeaponReloadEvent.bulletsInMagazine = _bulletsInMagazine;
        if (!npcIsOwner)
        {
            EventsAgregator.Post<OnWeaponReloadEvent>(this, _onWeaponReloadEvent);
        }
    }


    private void FireRay() 
    {
        Ray ray = Camera.main.ViewportPointToRay(_cameraViewportCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, ShootMaxDistance, LayerMask.GetMask(PlayersLayerName)))
        {
            if(hit.collider.CompareTag("Enemy"))
            {
                if(hit.collider.TryGetComponent<NPCController>(out NPCController controller))
                {
                    NPCController npc = controller;
                    npc.TakeDamage(_damage, WeaponModel);
                }
            }
        }
    }


    private void CooldownTimer()
    {
        if (_coolDownTime > 0)
        {
            _coolDownTime -= Time.fixedDeltaTime;
        }
    }

    private void FixedUpdate()
    {
        CooldownTimer();
    }

}
