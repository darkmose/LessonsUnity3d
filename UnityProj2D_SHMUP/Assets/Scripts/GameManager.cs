using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Canvas continueMenu;
    [SerializeField] private Player _player;
    private Button continueYesButton;
    private Button continueNoButton;



    private int lives;
    private int credits;
    private int score;
    public static int Score 
    { 
        get 
        { 
            return instance.score;
        }
        set 
        {
            instance.score = value;
            EventDelegate.RaiseOnScoreChanged(value);        
        }
    }
    public static  int Credits
    {
        get
        {
            return instance.credits;
        }
        set
        {
            instance.credits = value;
            EventDelegate.RaiseOnCreditsChanged(value);
        }
    }
    public static int Lives
    {
        get
        {
            return instance.lives;
        }
        set
        {
            instance.lives = value;
            EventDelegate.RaiseOnHealthChanged(value);
        }
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        Credits = 2;
        Score = 0;
        Lives = 2;


        EventDelegate.OnEnemySpawnEvent += OnEnemySpawnEventHandler;
        EventDelegate.OnEnemyDeathEvent += OnEnemyDeathHandler;
        EventDelegate.OnPlayerDeathEvent += OnPlayerDeathHandler;
        EventDelegate.OnPlayerLivesOutEvent += OnPlayerLivesOutHandler;
        EventDelegate.OnBlockProjectileEvent += OnBlockProjectileHandler;

        continueYesButton = continueMenu.transform.Find("YesButton").gameObject.GetComponent<Button>();
        continueNoButton = continueMenu.transform.Find("NoButton").gameObject.GetComponent<Button>();
        continueYesButton.onClick.AddListener(OnContinueYes);
        continueNoButton.onClick.AddListener(OnContinueNo);
    }

    private void OnBlockProjectileHandler()
    {
        Score += 10;
    }

    private void OnPlayerLivesOutHandler()
    {
        DOTween.Sequence()
            .AppendInterval(0.5f)
            .OnComplete(() =>
            {
                Time.timeScale = 0;
                continueMenu.gameObject.SetActive(true);
                if (credits <= 0)
                {
                    continueYesButton.enabled = false;
                }
                else
                {
                    continueYesButton.enabled = true;
                }
            });
    }


    private void OnContinueYes() 
    {
        Credits--;
        Time.timeScale = 1;
        continueMenu.gameObject.SetActive(false);
        Lives = 2;
        _player.Respawn();
    }

    private void OnContinueNo() 
    {
        ShowEndGameWindow();
    }

    private void ShowEndGameWindow()
    {
        // DO
    }

    private void OnPlayerDeathHandler()
    {
        Lives--;
        if (Lives <= 0)
        {
            EventDelegate.RaiseOnPlayerLivesOut();
        }
        else
        {
            _player.Respawn();
        }
    }

    private void OnEnemyDeathHandler(int scoreGain)
    {
        Score += scoreGain;
    }

    private void OnEnemySpawnEventHandler()
    {
        Debug.LogWarning("Enemy was spawned");
    }
}
