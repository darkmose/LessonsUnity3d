using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDelegate : MonoBehaviour
{
    static EventDelegate instance;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    public static event System.Action OnPlayerDeathEvent;
    public static event System.Action OnPlayerTakeDamageEvent;
    public static event System.Action <float> OnEnemyTakeDamageEvent;
    public static event System.Action <int> OnEnemyDeathEvent;
    public static event System.Action OnEnemySpawnEvent;
    public static event System.Action OnBlockProjectileEvent;
    public static event System.Action <int> OnScoreChangedEvent;
    public static event System.Action <int> OnHealthChangedEvent;
    public static event System.Action <float,float> OnShieldEnergyChangedEvent;
    public static event System.Action <int> OnEnergyLevelChangedEvent;
    public static event System.Action <float,float> OnEnergyChangedEvent;
    public static event System.Action <int> OnCreditsChangedEvent;
    public static event System.Action OnGameOverEvent;
    public static event System.Action OnShieldOffEvent;
    public static event System.Action OnUltraAttackOffEvent;
    public static event System.Action OnPlayerLivesOutEvent;
    public static event System.Action <float> OnBossTakeDamageEvent;
    public static event System.Action <float, float> OnBossHealthChangedEvent;
    public static event System.Action OnBossDeathEvent;
    public static event System.Action OnStartBossFightEvent;
    public static event System.Action OnStartGameEvent;

    public static void RaiseOnStartBossFight()
    {
        AudioManager.StopMusic();
        AudioManager.PlayMusic("Epic_Boss_Fight");

        OnStartBossFightEvent?.Invoke();
    }    
    
    public static void RaiseOnStartGame()
    {
        AudioManager.StopMusic();
        AudioManager.PlayMusic("GameMusic");

        OnStartGameEvent?.Invoke();
    }    

    public static void RaiseOnBossTakeDamage(float damage)
    {
        AudioManager.PlaySound("EnemyTakeDamage");
        OnBossTakeDamageEvent?.Invoke(damage);
    }
    public static void RaiseOnBossHealthChangedEvent(float hp, float fullHP)
    {
        OnBossHealthChangedEvent?.Invoke(hp, fullHP);
    }

    public static void RaiseOnBossDeath()
    {
        AudioManager.PlaySound("Explosion");
        OnBossDeathEvent?.Invoke();
    }
    public static void RaiseOnUltraAttackOff()
    {
        OnUltraAttackOffEvent?.Invoke();
    }


    public static void RaiseOnPlayerLivesOut() 
    {
        OnPlayerLivesOutEvent?.Invoke();
    }

    public static void RaiseOnShieldOff()
    {
        OnShieldOffEvent?.Invoke();
    }

    public static void RaiseOnEnergyLevelChanged(int level)
    {
        AudioManager.PlaySound("EnergyLvlUp");
        OnEnergyLevelChangedEvent?.Invoke(level);
    }
    public static void RaiseOnEnergyChanged(float energy, float fullEnergy)
    {
        OnEnergyChangedEvent?.Invoke(energy, fullEnergy);
    }
    public static void RaiseOnGameOverEvent()
    {
        AudioManager.StopMusic();
        OnGameOverEvent?.Invoke();
    }
    public static void RaiseOnShieldEnergyChanged(float energy, float fullEnergy)
    {
        OnShieldEnergyChangedEvent?.Invoke(energy,fullEnergy);
    }
    public static void RaiseOnScoreChanged(int score)
    {
        OnScoreChangedEvent?.Invoke(score);
    }
    public static void RaiseOnHealthChanged(int health)
    {
        OnHealthChangedEvent?.Invoke(health);
    }
    public static void RaiseOnCreditsChanged(int credits)
    {
        OnCreditsChangedEvent?.Invoke(credits);
    }

    public static void RaiseOnEnemySpawn() 
    {
        OnEnemySpawnEvent?.Invoke();
    }

    public static void RaiseOnEnemyDeath(int scoreGain) 
    {
        AudioManager.PlaySound("EnemyExplosion");
        OnEnemyDeathEvent?.Invoke(scoreGain);
    }
    public static void RaiseOnPlayerDeath() 
    {
        AudioManager.PlaySound("Explosion");
        OnPlayerDeathEvent?.Invoke();
    }
    public static void RaiseOnPlayerTakeDamage() 
    {
        OnPlayerTakeDamageEvent?.Invoke();
    }
    public static void RaiseOnEnemyTakeDamage(float damage) 
    {
        AudioManager.PlaySound("EnemyTakeDamage");
        OnEnemyTakeDamageEvent?.Invoke(damage);
    }

    public static void RaiseOnBlockProjectileEvent() 
    {
        AudioManager.PlaySound("EnemyTakeDamage");
        OnBlockProjectileEvent?.Invoke();
    }

}
