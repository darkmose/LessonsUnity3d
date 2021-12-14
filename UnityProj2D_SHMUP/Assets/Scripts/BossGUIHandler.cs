using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossGUIHandler : MonoBehaviour
{
    [SerializeField] private Image bossHpScale;
    [SerializeField] private BossHandler boss;
    [SerializeField] private TextMeshProUGUI bossName;
    private void Awake()
    {
        EventDelegate.OnBossHealthChangedEvent += OnBossHealthChangedHandler;
    }

    private void OnEnable()
    {
        RefreshData();
    }

    private void OnBossHealthChangedHandler(float hp, float fullHP)
    {
        var scale = Vector3.one;
        scale.x = hp / fullHP;
        bossHpScale.transform.localScale = scale;
    }

    private void RefreshData() 
    {
        bossName.text = boss.bossName;
        OnBossHealthChangedHandler(1,1);
    }

}
