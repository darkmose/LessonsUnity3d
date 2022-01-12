using GameEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GUIHandler : MonoBehaviour
{
    private const string AK74IconFileName = "AK74_Image";
    private const string M1911IconFileName = "M1911_Image";
    [SerializeField] private GameObject _gameUICanvas;
    [SerializeField] private GameObject _controlUICanvas;
    [SerializeField] private Text _weaponBulletsCountText;
    [SerializeField] private Text _weaponBulletsInMagazineText;
    [SerializeField] private Image _weaponImage;
     private Sprite _aK74Icon;
     private Sprite _m1911Icon;
    [SerializeField] private SpriteAtlas spriteAtlasGUI;


    private void Awake()
    {
        EventsAgregator.Subscribe<OnWeaponReloadEvent>(OnWeaponReloadHandler);    
        EventsAgregator.Subscribe<OnWeaponShootEvent>(OnWeaponShootHandler);    
        EventsAgregator.Subscribe<OnWeaponChangedEvent>(OnWeaponChangedHandler);
        _aK74Icon = spriteAtlasGUI.GetSprite(AK74IconFileName);
        _m1911Icon = spriteAtlasGUI.GetSprite(M1911IconFileName);
    }

    private void OnWeaponChangedHandler(object sender, OnWeaponChangedEvent data)
    {
        Debug.Log(data.weaponType);
        switch (data.weaponType)
        {
            case Weapon.WeaponType.AK74:
                _weaponImage.sprite = _aK74Icon;
                break;
            case Weapon.WeaponType.M1911:
                _weaponImage.sprite = _m1911Icon;

                break;
            default:
                break;
        }
        _weaponBulletsCountText.text = data.bulletCount.ToString();
        _weaponBulletsInMagazineText.text = data.bulletInMagazine.ToString();
    }

    private void OnWeaponShootHandler(object sender, OnWeaponShootEvent data)
    {
        _weaponBulletsInMagazineText.text = data.bulletsInMagazine.ToString();
    }

    private void OnWeaponReloadHandler(object sender, OnWeaponReloadEvent data)
    {
        _weaponBulletsCountText.text = data.bulletsCount.ToString();
        _weaponBulletsInMagazineText.text = data.bulletsInMagazine.ToString();
    }

    private void OnDestroy()
    {
        EventsAgregator.Unsubscribe<OnWeaponReloadEvent>(OnWeaponReloadHandler);
        EventsAgregator.Unsubscribe<OnWeaponShootEvent>(OnWeaponShootHandler);
        EventsAgregator.Unsubscribe<OnWeaponChangedEvent>(OnWeaponChangedHandler);
    }

}
