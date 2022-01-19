using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const string EnemyTagName = "Enemy";
    private const string PlayerTagName = "Player";
    private const int MainMenuSceneIndex = 0;

    public static GameManager Instance { get; private set; }
    [SerializeField] private List<Camera> _cameraList;
    [SerializeField]
    [Range(1, 6)] private int _maxNPCCount = 3;
    private List<GameObject> _mainPlayerPrefabs;
    private List<GameObject> _charactersPrefabs;
    [SerializeField] private List<PlayersSpawner> _playerSpawners;
    [SerializeField] private Dictionary<string, GameObject> _currentGamePlayers;
    private readonly List<string> _playerNames = new List<string> { "Baldwyn",
                                                "Banner",
                                                "Barrett",
                                                "Blade",
                                                "Brandon",
                                                "Brant",
                                                "Brendan",
                                                "Brendon",
                                                "Briac",
                                                "Brion",
                                                "Brisan",
                                                "Cadal",
                                                "Cathal",
                                                "Caylon",
                                                "Caysee",
                                                "Cayson",
                                                "Chance",
                                                "Charles",
                                                "Chasin",
                                                "Chuck",
                                                "Clint",
                                                "Colt",
                                                "Damon",
                                                "Denzel",
                                                "Diarmuid",
                                                "Dillen",
                                                "Donley",
                                                "Donner",
                                                "Donovan",
                                                "Doug",
                                                "Drake",
                                                "Dustin",
                                                "Dustin",
                                                "Eames",
                                                "Eamon",
                                                "Eason",
                                                "Ebert",
                                                "Eegan",
                                                "Eibhear",
                                                "Eohric",
                                                "Ethan",
                                                "Farol",
                                                "Farrel",
                                                "Farrell",
                                                "Faust",
                                                "Feargal",
                                                "Fearghus",
                                                "Felix",
                                                "Ferguson",
                                                "Frederik",
                                                };
    [SerializeField] private FloatingJoystick _moveJoystick;
    [SerializeField] private FloatingJoystick _viewJoystick;
    [SerializeField] private Button _jumpScreenButton;
    [SerializeField] private Button _scopeScreenButton;
    [SerializeField] private Button _fireScreenButton;
    [SerializeField] private Button _reloadScreenButton;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        EventsAgregator.Subscribe<OnEntityDiesEvent>(OnEntityDiesHandler);
        _charactersPrefabs = new List<GameObject>();
        _mainPlayerPrefabs = new List<GameObject>();
        _currentGamePlayers = new Dictionary<string, GameObject>();
    }

    public static void JumpButtonAddListener(UnityEngine.Events.UnityAction action) 
    {
        Debug.Log("Jump Add listener");
        Instance._jumpScreenButton.onClick.AddListener(action);
    }
    
    public static void ReloadButtonAddListener(UnityEngine.Events.UnityAction action) 
    {
        Debug.Log("Reload Add listener");
        Instance._reloadScreenButton.onClick.AddListener(action);
    }

    public static void ScopeButtonAddListener(UnityEngine.Events.UnityAction action)
    {     
        Debug.Log("Scope Add listener");
        Instance._scopeScreenButton.onClick.AddListener(action);
    }
    
    public static void FireButtonAddListener(UnityEngine.Events.UnityAction action)
    {     
        Debug.Log("Fire Add listener");
        Instance._fireScreenButton.onClick.AddListener(action);
    }


    public static FloatingJoystick GetMoveJoystick() 
    {
        return Instance._moveJoystick;
    }
    public static FloatingJoystick GetViewJoystick() 
    {
        return Instance._viewJoystick;
    }

    private void Start()
    {
        PrepareCharactersPrefabs();
        PrepareMainPlayer();
        PreparePlayers();
    }

    public void PrepareCharactersPrefabs() 
    {
        _charactersPrefabs.AddRange(PrefabDictionary.GetAllNPCPrefabs());
        _mainPlayerPrefabs.AddRange(PrefabDictionary.GetAllPlayerPrefabs());
    }

    public void PreparePlayers() 
    {
        _currentGamePlayers.Clear();

        for (int i = 0; i < _maxNPCCount; i++)
        {
            var spawner = GetRandomPlayerSpawner();
            var randValue = Random.Range(0, _charactersPrefabs.Count);
            var name = GenerateName();
            var character = _charactersPrefabs[randValue];
            character.name = name;
            _currentGamePlayers.Add(name,character);
            spawner.Spawn(character);
        }
    }

    public void PrepareMainPlayer() 
    {
        var spawner = GetRandomPlayerSpawner();
        var name = GenerateName();
        var mainPlayer = _mainPlayerPrefabs[0];
        mainPlayer.name = name;
        var player = spawner.Spawn(mainPlayer);
        _currentGamePlayers.Add(name, player);
        var cameraData = Camera.main.GetUniversalAdditionalCameraData();
        foreach (Camera camera in _cameraList)
        {
            cameraData.cameraStack.Add(camera);
        }
    }

    private PlayersSpawner GetRandomPlayerSpawner() 
    {
        if (_playerSpawners.Count > 0)
        {

            var randValue = Random.Range(0, _playerSpawners.Count);
            return _playerSpawners[randValue];

        }
        return null;
    }

    private PlayersSpawner GetAvailableSpawner() 
    {
        for (int i = 0; i < _playerSpawners.Count; i++)
        {
            if (_playerSpawners[i].isAvailable)
            {
                return _playerSpawners[i];
            }
        }
        return GetRandomPlayerSpawner();
    }


    private string GenerateName()
    {
        var namesLength = _playerNames.Count;
        var randValue = Random.Range(0, namesLength);
        var result = _playerNames[randValue];
        _playerNames.Remove(result);
        return result;
    }

    private void OnEntityDiesHandler(object sender, OnEntityDiesEvent data)
    {

        if (data.tag == EnemyTagName)
        {
            var obj = (VitalitySystem)sender;
            if (_currentGamePlayers.ContainsKey(data.victimName))
            {
                var spawner = GetAvailableSpawner();
                spawner.Respawn(_currentGamePlayers[data.victimName]);
            }
            return;
        }
        if (data.tag == PlayerTagName)
        {
            var spawner = GetAvailableSpawner();
            var mainPlayer = _mainPlayerPrefabs[0];
            spawner.Respawn(mainPlayer);
        }
    }

    public static void StopTime() 
    {
        Time.timeScale = 0;
    }

    public static void ResumeTime() 
    {
        Time.timeScale = 1;
    }

    public static void LoadMainMenuScene() 
    {
        SceneManager.LoadScene(MainMenuSceneIndex);
    }


}
