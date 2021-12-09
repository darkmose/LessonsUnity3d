using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shield : MonoBehaviour
{
    private const float SIZE = 7f;
    private const float COLL_SIZE = 1.2f;

    private void Awake()
    {
        EventDelegate.OnShieldEnergyChangedEvent += OnShieldEnergyChangedHandler;
        EventDelegate.OnPlayerDeathEvent += OnPlayerDeathHandler;
    }

    private void OnPlayerDeathHandler()
    {
        ShieldOff();
    }

    private void OnEnable()
    {
        ShieldOn();
    }

    private void OnShieldEnergyChangedHandler(float shieldEnergy, float fullShieldEnergy)
    {
        if (shieldEnergy <= 0)
        {
            ShieldOff();
        }
    }

    private void ShieldOn()
    {
        var circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = true;
        circleCollider.radius = 0.1f;
        DOTween.To(() => circleCollider.radius, x => circleCollider.radius = x, COLL_SIZE, 1);
        transform.DOScale(SIZE, 1);
    }

    void ShieldOff()
    {
        var circleCollider = GetComponent<CircleCollider2D>();
        DOTween.To(() => circleCollider.radius, x => circleCollider.radius = x, 0.1f, 1).SetDelay(0.5f);
        transform.DOScale(0.1f, 1).SetDelay(0.5f).OnComplete(()=> 
        { 
            gameObject.SetActive(false);
            circleCollider.enabled = false;
            EventDelegate.RaiseOnShieldOff();
        });
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAmmo"))
        {
            collision.gameObject.SetActive(false);
            EventDelegate.RaiseOnBlockProjectileEvent();
        }
    }

}
