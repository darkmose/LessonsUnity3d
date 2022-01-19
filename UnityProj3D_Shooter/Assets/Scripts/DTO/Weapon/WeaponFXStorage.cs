using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFXStorage : MonoBehaviour
{
    public ParticleSystem particles;
    public SpriteRenderer fireSprite;
    public Animator animator;

    public void FireWeaponAnimationEventEnter()
    {
        fireSprite.enabled = true;
        particles?.Play();
    }
    public void FireWeaponAnimationEventExit()
    {
        fireSprite.enabled = false;
        particles?.Play();
    }

}
