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

    private void Awake()
    {
        EventDelegate.OnEnergyChangedEvent += OnEnergyChangedHandler;
        EventDelegate.OnShieldEnergyChangedEvent += OnShieldEnergyChangedHandler;
        EventDelegate.OnHealthChangedEvent += OnHealthChangedHandler;
        EventDelegate.OnScoreChangedEvent += OnScoreChangedHandler;
        EventDelegate.OnCreditsChangedEvent += OnCreditsChangedHandler;
        EventDelegate.OnEnergyLevelChangedEvent += OnEnergyLevelChanged;
    }

    private void OnShieldEnergyChangedHandler(float shieldEnergy, float fullShieldEnergy)
    {
        shieldBar.fillAmount = shieldEnergy / fullShieldEnergy;
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
    }

    private void OnEnergyLevelChanged(int level)
    {
        energyBar.DOColor(ultra_colors[level], 0.3f);
        energyLevelText.text = "x"+level.ToString();
    }

    private void OnCreditsChangedHandler(int credits)
    {
        creditsText.text = credits.ToString();
    }

    private void OnHealthChangedHandler(int hp)
    {
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
        scoreText.text = score.ToString();
    }
    




}
