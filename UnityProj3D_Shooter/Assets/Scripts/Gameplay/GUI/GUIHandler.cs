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
    private const float HalfHealthRatio = 0.5f;
    private const float CriticalHPRatio= 0.2f;
    [SerializeField] private GameObject _gameUICanvas;
    [SerializeField] private GameObject _controlUICanvas;
    [SerializeField] private Text _weaponBulletsCountText;
    [SerializeField] private Text _weaponBulletsInMagazineText;
    [SerializeField] private Image _weaponImage;
    [SerializeField] private Image _bloodscreenBackgroundImage;
    [SerializeField] private Image _bloodscreenBloodDropImage;
    private Color _bloodScreenColor = Color.white;
     private Sprite _aK74Icon;
     private Sprite _m1911Icon;
    [SerializeField] private SpriteAtlas spriteAtlasGUI;
    [SerializeField] private Image _healthBarValueImage;
    [SerializeField] private Canvas _pauseMenuCanvas;
    [SerializeField] private Button _pauseContinueButton; 
    [SerializeField] private Button _pauseMainMenuButton; 
    [SerializeField] private Button _pauseExitButton; 


    private void Awake()
    {
        EventsAgregator.Subscribe<OnWeaponReloadEvent>(OnWeaponReloadHandler);    
        EventsAgregator.Subscribe<OnWeaponShootEvent>(OnWeaponShootHandler);    
        EventsAgregator.Subscribe<OnWeaponChangedEvent>(OnWeaponChangedHandler);
        EventsAgregator.Subscribe<OnPlayerTakeDamageEvent>(OnPlayerTakeDamageHandler);
        EventsAgregator.Subscribe<OnPlayerHealthRefreshEvent>(OnPlayerHealthRefreshHandler);
        _pauseContinueButton.onClick.AddListener(ContinueButtonHandler);
        _pauseMainMenuButton.onClick.AddListener(MainMenuButtonHandler);

        _pauseExitButton.onClick.AddListener(ExitButtonHandler);
        _aK74Icon = spriteAtlasGUI.GetSprite(AK74IconFileName);
        _m1911Icon = spriteAtlasGUI.GetSprite(M1911IconFileName);
        _bloodscreenBloodDropImage.enabled = false;
        _bloodScreenColor.a = 0;
        _bloodscreenBackgroundImage.color = _bloodScreenColor;
        _pauseMenuCanvas.enabled = false;
        GameManager.ResumeTime();
    }


    private void RefreshHealthStatus(int currentHP) 
    {
        float healthRatio = (float)currentHP / (float)VitalitySystem.FullHealthPoints;
        _healthBarValueImage.fillAmount = healthRatio;
        if (healthRatio < HalfHealthRatio)
        {
            healthRatio = Mathf.Clamp(healthRatio, 0, HalfHealthRatio);
            _bloodScreenColor.a = 1 - healthRatio;
            if (healthRatio < CriticalHPRatio)
            {
                _bloodscreenBloodDropImage.enabled = true;
            }
        }
        else
        {
            _bloodScreenColor.a = 0;
            _bloodscreenBloodDropImage.enabled = false;
        }
        _bloodscreenBackgroundImage.color = _bloodScreenColor;
    } 

    private void OnPlayerHealthRefreshHandler(object sedner, OnPlayerHealthRefreshEvent data)
    {
        RefreshHealthStatus(data.currentHealth);
    }

    private void OnPlayerTakeDamageHandler(object sender, OnPlayerTakeDamageEvent data)
    {
        RefreshHealthStatus(data.currentHP);
    }

    private void OnWeaponChangedHandler(object sender, OnWeaponChangedEvent data)
    {
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
        EventsAgregator.Unsubscribe<OnPlayerTakeDamageEvent>(OnPlayerTakeDamageHandler);
        EventsAgregator.Unsubscribe<OnPlayerHealthRefreshEvent>(OnPlayerHealthRefreshHandler);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Menu) || Input.GetKeyDown(KeyCode.Escape))
        {
#if PLATFORM_ANDROID
#else
        Cursor.lockState = CursorLockMode.None;
#endif
            _pauseMenuCanvas.enabled = true;
            GameManager.StopTime();
        }
    }

    private void ContinueButtonHandler()
    {
        _pauseMenuCanvas.enabled = false;
#if PLATFORM_ANDROID
#else
        Cursor.lockState = CursorLockMode.Locked;
#endif
        GameManager.ResumeTime();
    }
    private void MainMenuButtonHandler()
    {
        GameManager.LoadMainMenuScene();
    }
    private void ExitButtonHandler()
    {
        Application.Quit();
    }

}
