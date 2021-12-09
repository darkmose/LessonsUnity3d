using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UltraAttack : MonoBehaviour
{
    [SerializeField] private enum UltraAttackType { Laser };
    [SerializeField] private UltraAttackType attackType;
    [SerializeField] private Color[] ultra_colors;
    //[SerializeField] private Color ultra_2_level;
    //[SerializeField] private Color ultra_3_level;
    private new BoxCollider2D collider;
    private SpriteRenderer sprite;
    private int attackLevel;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        transform.localScale = Vector3.up * 0.01f;

        EventDelegate.OnEnergyChangedEvent += OnEnergyChangedEventHandler;
        EventDelegate.OnEnergyLevelChangedEvent += OnEnergyLevelChangedEventHandler;
        attackLevel = 1;
        EventDelegate.OnPlayerDeathEvent += OnPlayerDeathHandler;
    }

    private void OnPlayerDeathHandler()
    {
        EndAttack();
    }

    private void OnEnergyChangedEventHandler(float energy, float fullEnergy)
    {
        if (energy <= 0 && attackLevel <= 0)
        {
            EndAttack();
        }
    }

    private void OnEnergyLevelChangedEventHandler(int level)
    {
        attackLevel = level;
        var scale = transform.localScale;
        scale.x = 0.4f * (attackLevel + 1);
        transform.DOScale(scale, 0.3f);
        sprite.DOColor(ultra_colors[level], 0.3f);
    }

    public void StartAttack()
    {
        collider.enabled = true;
        sprite.enabled = true;
        var scale = new Vector3(0.4f * (attackLevel + 1), 1f, 1f);
        transform.DOScale(scale, 0.3f);
        sprite.DOColor(ultra_colors[attackLevel], 0.2f);
    }


    private void EndAttack() 
    {
        var scale = new Vector3(0.4f * (attackLevel + 1), 0.01f, 1f);
        Color color = sprite.color;
        color.a = 0;

        DOTween.Sequence().Append(transform.DOScale(scale, 0.3f).OnComplete(() =>
        {
            sprite.enabled = false;
            collider.enabled = false;
            EventDelegate.RaiseOnUltraAttackOff();
        }))
        .Join(sprite.DOColor(color, 0.2f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {            
            case "Enemy":
                collision.GetComponent<Enemy>().TakeDamage(100);
                break;
            case "EnemyAmmo":
                collision.gameObject.SetActive(false);
                EventDelegate.RaiseOnBlockProjectileEvent();
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var damage = 10 * (attackLevel+1);
            collision.GetComponent<Enemy>().TakeDamage(damage);
        }   
        
    }

}
