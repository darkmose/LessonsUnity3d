using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class GUIHandler : MonoBehaviour
{
    [SerializeField] private Image energyBar;
    [SerializeField] private Image shieldBar;
    [SerializeField] private Image hpBar;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private TextMeshProUGUI energyLevelText;
    [SerializeField] private Sprite one_Image;
    [SerializeField] private Sprite two_Image;
    [SerializeField] private Sprite zero_Image;
    [SerializeField] private Color[] ultra_colors;
    private static float _energyScale;
    private static float _shieldScale;
    private static int _score;
    private static int _credits;
    private static int _lives;
    private static int _energyLevel;
    private static int _ultraColorIndex;


    private void Awake()
    {
        EventDelegate.OnEnergyChangedEvent += OnEnergyChangedHandler;
        EventDelegate.OnShieldEnergyChangedEvent += OnShieldEnergyChangedHandler;
        EventDelegate.OnHealthChangedEvent += OnHealthChangedHandler;
        EventDelegate.OnScoreChangedEvent += OnScoreChangedHandler;
        EventDelegate.OnCreditsChangedEvent += OnCreditsChangedHandler;
        EventDelegate.OnEnergyLevelChangedEvent += OnEnergyLevelChanged;
    }

    private void OnEnable()
    {
        shieldBar.fillAmount = _shieldScale;

        if (_shieldScale == 1f)
        {
            shieldBar.color = Color.cyan;
        }
        if (_shieldScale <= 0.2f)
        {
            shieldBar.color = Color.blue;
        }

        energyBar.fillAmount = _energyScale;

        energyBar.DOColor(ultra_colors[_energyLevel], 0.3f);
        energyLevelText.text = "x" + _energyLevel.ToString();

        creditsText.text = _credits.ToString();

        switch (_lives)
        {
            case 0:
                hpBar.sprite = zero_Image;
                break;
            case 1:
                hpBar.sprite = one_Image;
                break;
            case 2:
                hpBar.sprite = two_Image;
                break;
        }

        scoreText.text = _score.ToString();
    }


    private void OnShieldEnergyChangedHandler(float shieldEnergy, float fullShieldEnergy)
    {
        shieldBar.fillAmount = shieldEnergy / fullShieldEnergy;
        _shieldScale = shieldBar.fillAmount;

        if (shieldBar.fillAmount == 1f)
        {
            shieldBar.color = Color.cyan;
        }
        if (shieldBar.fillAmount <= 0.2f)
        {
            shieldBar.color = Color.blue;
        }
    }

    private void OnEnergyChangedHandler(float energy, float fullEnergy)
    {
        energyBar.fillAmount = energy / fullEnergy;
        _energyScale = energyBar.fillAmount;
    }

    private void OnEnergyLevelChanged(int level)
    {
        _energyLevel = level;
        energyBar.DOColor(ultra_colors[level], 0.3f);
        energyLevelText.text = "x"+level.ToString();
    }

    private void OnCreditsChangedHandler(int credits)
    {
        _credits = credits;
        creditsText.text = credits.ToString();
    }

    private void OnHealthChangedHandler(int hp)
    {
        _lives = hp;
        switch (hp)
        {
            case 0:
                hpBar.sprite = zero_Image;
                break;
            case 1:
                hpBar.sprite = one_Image;
                break;
            case 2:
                hpBar.sprite = two_Image;
                break;
        }
    }

    void OnScoreChangedHandler(int score) 
    {
        _score = score;
        scoreText.text = score.ToString();
    }
    




}
