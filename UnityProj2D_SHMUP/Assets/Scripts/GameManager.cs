using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        EventDelegate.OnEnemySpawn += OnEnemySpawnEventHandler;
    
    }

    private void OnEnemySpawnEventHandler()
    {
        Debug.LogWarning("Enemy was spawned");
    }
}
