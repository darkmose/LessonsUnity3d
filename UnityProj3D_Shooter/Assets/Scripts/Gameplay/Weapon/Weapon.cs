using GameEvents;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region CONST PARAMS
    private const int AK74FireRate = 3;
    private const int AK74StartBulletCount = 30;
    private const int AK74BulletsInMagazine = 30;
    private const int AK74Damage = 25;
    private const int AK74MagazineCapacity = 30;
    private const float ViewPortCameraCenter = 0.5f;
    private const int M1911FireRate = 2;
    private const int M1911StartBulletCount = 20;
    private const int M1911BulletsInMagazine = 9;
    private const int M1911Damage = 25;
    private const int M1911MagazineCapacity = 9;
    private const string PlayersLayerName = "Player";
    private const float ShootMaxDistance = 300f;
    private const int MaximumCooldownTime = 5;
    private const string Ak74FireSoundName = "AK74Fire";
    private const string M1911FireSoundName = "M1911Fire";
    private const string ReloadSoundName = "ReloadSound";
    private const string EnemyTagName = "Enemy";
    #endregion

    public string ownerName = "";
    public bool npcIsOwner = true;
    private WeaponType _weaponType = WeaponType.None;
    public WeaponType WeaponModel => _weaponType;
    public WeaponAnimator _weaponAnimator;
    public int BulletsCount
    {
        get 
        {
            if (isThrowingWeapon)
            {
                return _throwingWeaponCount;
            }
            else
            {
                return _bulletsCount;
            }
        }
        private set
        {
            _bulletsCount = value;
        }
    }
    public int BulletsInMagazine => _bulletsInMagazine;
    public bool BurstShooting { get; private set; }
    public int index = 0;
    private int _throwingWeaponCount = 0;

    private float _fireRate;
    private int _bulletsCount;
    private int _bulletsInMagazine;
    private int _magazineCapacity;
    private int _damage;
    private float _coolDownTime;
    private bool isThrowingWeapon = false;
    public Transform iKLHand;
    public Transform iKRHand;
    private Vector3 _cameraViewportCenter = new Vector3(ViewPortCameraCenter, ViewPortCameraCenter, 0);

    private OnWeaponShootEvent _onWeaponShootEvent = new OnWeaponShootEvent();
    private OnWeaponReloadEvent _onWeaponReloadEvent = new OnWeaponReloadEvent();


    public enum WeaponType
    {
        None,
        AK74,
        M1911,
        Grenade
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
        _weaponAnimator?.FireAnimation();
        switch (WeaponModel)
        {
            case WeaponType.AK74:
                AudioManager.PlaySound(Ak74FireSoundName);

                break;
            case WeaponType.M1911:
                AudioManager.PlaySound(M1911FireSoundName);
                break;
        }
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
                                nPC.TakeDamage(_damage, ownerName, WeaponModel);
                            }
                            if (hit.collider.TryGetComponent(out PlayerController player))
                            {
                                player.TakeDamage(_damage, ownerName, WeaponModel);
                            }
                        }
                    
                    }
                    _bulletsInMagazine--;
                    _coolDownTime = 1 / _fireRate;
                }
                else
                {
                    ReloadWeapon();
                    AudioManager.PlaySound(ReloadSoundName);

                }
            }       
    }   

    public void Fire() //ForPlayer
    {
        
        if (_coolDownTime <= 0f)
        {
            if (_bulletsInMagazine > 0)
            {
                FireRay();
                _bulletsInMagazine--;
                _coolDownTime = 1 / _fireRate;
                _onWeaponShootEvent.bulletsInMagazine = _bulletsInMagazine;
                if (!npcIsOwner)
                {
                    EventsAgregator.Post<OnWeaponShootEvent>(this, _onWeaponShootEvent);
                }
            }
        }
        
    }


    public void AddAmmunition(int bulletsCount)
    {
        Debug.Log($"Add ammo. Bullets count added: {bulletsCount}");
        _bulletsCount += bulletsCount;
        _onWeaponReloadEvent.bulletsCount = _bulletsCount;
        _onWeaponReloadEvent.bulletsInMagazine = _bulletsInMagazine;
        if (!npcIsOwner)
        {
            EventsAgregator.Post<OnWeaponReloadEvent>(this, _onWeaponReloadEvent);
        }        
    }

    public override string ToString()
    {
        string weaponParams = "";
        weaponParams += $"FireRate: {_fireRate}\n";
        weaponParams += $"BulletsCount: {_bulletsCount}\n";
        weaponParams += $"BulletsInMagazine: {_bulletsInMagazine}\n";
        weaponParams += $"MagazineCapacity: {_magazineCapacity}\n";
        weaponParams += $"Damage: {_damage}\n";
        weaponParams += $"CoolDownTime: {_coolDownTime}";

        return weaponParams;
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
            AudioManager.PlaySound(ReloadSoundName);
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
        switch (WeaponModel)
        {
            case WeaponType.AK74:
                AudioManager.PlaySound(Ak74FireSoundName);
                break;
            case WeaponType.M1911:
                AudioManager.PlaySound(M1911FireSoundName);
                break;
        }
        Ray ray = Camera.main.ViewportPointToRay(_cameraViewportCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, ShootMaxDistance, LayerMask.GetMask(PlayersLayerName)))
        {
            if(hit.collider.CompareTag(EnemyTagName))
            {
                if(hit.collider.TryGetComponent<NPCController>(out NPCController controller))
                {
                    NPCController npc = controller;
                    npc.TakeDamage(_damage, ownerName, WeaponModel);
                }
            }
        }
    }

    private void CooldownTimer()
    {
        if (_coolDownTime > 0)
        {
            _coolDownTime -= Time.fixedDeltaTime;
            Mathf.Clamp(_coolDownTime, 0, MaximumCooldownTime);

        }
    }

    private void FixedUpdate()
    {
        CooldownTimer();
    }
}
