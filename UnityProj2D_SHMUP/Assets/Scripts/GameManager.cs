using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Canvas continueMenu;
    [SerializeField] private Canvas mainGUI;
    [SerializeField] private Canvas bossGUI;
    [SerializeField] private Canvas endGameGUI;
    [SerializeField] private TextMeshProUGUI creditsLeft;
    private Player _player;
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
            if (instance.score != value)
            {
                instance.score = value;
                EventDelegate.RaiseOnScoreChanged(value);
            }
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
            if (instance.credits != value)
            {
                instance.credits = value;
                EventDelegate.RaiseOnCreditsChanged(value);
            }
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
            if (instance.lives != value)
            {
                instance.lives = value;
                EventDelegate.RaiseOnHealthChanged(value);
            }
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

        EventDelegate.OnStartBossFightEvent += OnStartBossFightHandler;
        EventDelegate.OnEnemySpawnEvent += OnEnemySpawnEventHandler;
        EventDelegate.OnEnemyDeathEvent += OnEnemyDeathHandler;
        EventDelegate.OnPlayerDeathEvent += OnPlayerDeathHandler;
        EventDelegate.OnPlayerLivesOutEvent += OnPlayerLivesOutHandler;
        EventDelegate.OnBlockProjectileEvent += OnBlockProjectileHandler;
        EventDelegate.OnBossTakeDamageEvent += OnBossTakeDamageHandler;
        EventDelegate.OnBossDeathEvent += OnBossDeathHandler;

        continueYesButton = continueMenu.transform.Find("InteractiveElements").Find("YesButton").gameObject.GetComponent<Button>();
        continueNoButton = continueMenu.transform.Find("InteractiveElements").Find("NoButton").gameObject.GetComponent<Button>();
        continueYesButton.onClick.AddListener(OnContinueYes);
        continueNoButton.onClick.AddListener(OnContinueNo);
    }

    private void Start()
    {
        InitializePlayer();
        EventDelegate.RaiseOnStartGame();
    }

    private void InitializePlayer()
    {
        if (PlayerPrefs.GetInt("indexSpaceship") == 0)
        {
            var _pos = Camera.main.transform.position;
            _pos.y -= 5f;
            _pos.z = -1f;
            GameObject playerGameObject = Instantiate(PrefabsDictionary.GetSpaceshipPrefab(PrefabsDictionary.Spaceships.Andromeda), _pos, Quaternion.identity);            
            _player = playerGameObject.GetComponent<Player>();
        }
        else
        {
            var _pos = Camera.main.transform.position;
            _pos.y -= 5f;
            _pos.z = -1f;
            GameObject playerGameObject = Instantiate(PrefabsDictionary.GetSpaceshipPrefab(PrefabsDictionary.Spaceships.Spaceglader), _pos, Quaternion.identity);
            _player = playerGameObject.GetComponent<Player>();
        }
    }

    private void OnBossDeathHandler()
    {
        DOTween.Sequence()
            .SetDelay(0.5f)
            .OnComplete(()=> 
            {
                ShowEndGameWindow();
                endGameGUI.GetComponent<EndGameHandler>().InitText(true);
            });
    }

    private void OnStartBossFightHandler()
    {
        mainGUI.gameObject.SetActive(false);
        bossGUI.gameObject.SetActive(true);
    }

    private void OnBossTakeDamageHandler(float damage)
    {
        Score += 5;
    }

    private void OnBlockProjectileHandler()
    {
        Score += 10;
    }

    private void OnPlayerLivesOutHandler()
    {
        DOTween.Sequence()
            .AppendInterval(1f)
            .OnComplete(() =>
            {
                Time.timeScale = 0;
                creditsLeft.text = $"You have {Credits} credits";
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
        continueMenu.gameObject.SetActive(false);
        ShowEndGameWindow();
    }

    public static int GetScore() 
    {
        return instance.score;
    }

    private void ShowEndGameWindow()
    {
        Time.timeScale = 0f;
        endGameGUI.gameObject.SetActive(true);
        endGameGUI.GetComponent<EndGameHandler>().InitText(false);
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
