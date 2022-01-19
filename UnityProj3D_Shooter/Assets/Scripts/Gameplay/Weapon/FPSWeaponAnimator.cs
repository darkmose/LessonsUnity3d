using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSWeaponAnimator : MonoBehaviour
{
    private const string AK74FireTriggerName = "AK74Fire";
    private const string M1911FireTriggerName = "M1911Fire";
    private const string AK74ScopeBoolParamName = "AK74Scope";
    private const string M1911ScopeBoolParamName = "M1911Scope";
    private SpriteRenderer _fireSprite;
    private ParticleSystem _particles;
    private Animator _animator;


    private void Awake()
    {

    }

    public void InitAnimator(Animator animator) 
    {
        _animator = animator;
    }

    public void SetSpriteAndParticles(SpriteRenderer spriteRenderer, ParticleSystem particles) 
    {
        _fireSprite = spriteRenderer;
        _particles = particles;
    }

    public void FireAnimation(Weapon.WeaponType weaponType) 
    {
        if (weaponType != Weapon.WeaponType.None)
        {
            switch (weaponType)
            {
                case Weapon.WeaponType.AK74:
                    _animator?.SetTrigger(AK74FireTriggerName);
                    break;
                case Weapon.WeaponType.M1911:
                    _animator?.SetTrigger(M1911FireTriggerName);
                    break;
                case Weapon.WeaponType.Grenade:
                    break;
                default:
                    break;
            }
        }
    }

    public void ScopeAnimation(Weapon.WeaponType weaponType) 
    {
        if (weaponType != Weapon.WeaponType.None)
        {
            switch (weaponType)
            {
                case Weapon.WeaponType.AK74:
                    _animator?.SetBool(AK74ScopeBoolParamName, true);
                    break;
                case Weapon.WeaponType.M1911:
                    _animator?.SetBool(M1911ScopeBoolParamName, true);
                    break;
                default:
                    break;
            }
        }
    }

    public void UnscopeAnimation(Weapon.WeaponType weaponType)
    {
        if (weaponType != Weapon.WeaponType.None)
        {
            switch (weaponType)
            {
                case Weapon.WeaponType.AK74:
                    _animator?.SetBool(AK74ScopeBoolParamName, false);
                    break;
                case Weapon.WeaponType.M1911:
                    _animator?.SetBool(M1911ScopeBoolParamName, false);
                    break;
                default:
                    break;
            }
        }
    }
}
