using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using System;

public class FPSController : MonoBehaviour
{
    private const int RightMouseButtonIndex = 1;
    [SerializeField] private FPSWeaponAnimator _weaponAnimator;
    [SerializeField] private GameObject _fPSAK74prefab;
    [SerializeField] private GameObject _fPSM1911prefab;
    [SerializeField] private Transform _fPSWeaponInventory;
    [SerializeField] private ScopeController _scopeController;
    private Dictionary<Weapon.WeaponType, GameObject> _fpsWeapons;
    private GameObject _currentWeapon;
    private Weapon.WeaponType _currentWeaponType;
    private bool _isScoped;

    private void Awake()
    {
        EventsAgregator.Subscribe<OnWeaponShootEvent>(OnWeaponShootHandler);
        EventsAgregator.Subscribe<OnWeaponChangedEvent>(OnWeaponChangedHandler);
        InitializeInventory();
    }

    private void Start()
    {
        GameManager.ScopeButtonAddListener(OnScopeScreenButtonClicHandler);
    }

    private void OnScopeScreenButtonClicHandler()
    {
        _isScoped = !_isScoped;
    }

    public FPSWeaponAnimator GetWeaponAnimator() 
    {
        return _weaponAnimator;
    }

    private void InitializeInventory() 
    {
        _fpsWeapons = new Dictionary<Weapon.WeaponType, GameObject>();
        var obj = Instantiate(_fPSAK74prefab, _fPSWeaponInventory);
        _fpsWeapons.Add(Weapon.WeaponType.AK74, obj);
        obj.SetActive(false);
        obj = Instantiate(_fPSM1911prefab, _fPSWeaponInventory);
        _fpsWeapons.Add(Weapon.WeaponType.M1911, obj);
        obj.SetActive(false);
        _currentWeapon = null;
    }

    private void OnWeaponChangedHandler(object sender, OnWeaponChangedEvent data)
    {
        if (_fpsWeapons.ContainsKey(data.weaponType))
        {
            _currentWeapon?.SetActive(false);
            GameObject weapon = _fpsWeapons[data.weaponType];
            _currentWeapon = weapon;
            _currentWeapon?.SetActive(true);
            var dto = _currentWeapon.GetComponentInChildren<WeaponFXStorage>();
            _currentWeaponType = data.weaponType;
            _weaponAnimator.SetSpriteAndParticles(dto.fireSprite, dto.particles);
            _weaponAnimator.InitAnimator(dto.animator);
            switch (data.weaponType)
            {
                case Weapon.WeaponType.AK74:
                    _scopeController.InitAK74Transform(weapon.transform);
                    break;
                case Weapon.WeaponType.M1911:
                    _scopeController.InitM1911Transform(weapon.transform);
                    break;
            }
            _scopeController.CurrentWeapon = _currentWeaponType;
            _scopeController.UnScope();
        }        
    }

    private void OnWeaponShootHandler(object sender, OnWeaponShootEvent data)
    {
        FireAnimation();
    }

    private void FireAnimation() 
    {
        _weaponAnimator?.FireAnimation(_currentWeaponType);
    }

    private void InputScan() 
    {
        if (Input.GetMouseButton(RightMouseButtonIndex))
        {
            _isScoped = true;
        }
        else
        {
            _isScoped = false;
        }
    }


    private void ScopeAssign() 
    {
        if (_isScoped)
        {
            _scopeController.Scope();
        }
        else
        {
            _scopeController.UnScope();
        }
    }

    private void Update()
    {
        //InputScan();
    }

    private void LateUpdate()
    {
        ScopeAssign();
    }

}
