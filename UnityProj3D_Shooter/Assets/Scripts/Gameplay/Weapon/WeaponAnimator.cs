using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    private const string AnimatorFireTriggerName = "Fire";
    private Animator _animator;
    private void Awake()
    {
        if (TryGetComponent(out Animator animator))
        {
            _animator = animator;
            Debug.Log(_animator != null);

        }
    }

    public void FireAnimation() 
    {
        _animator.SetTrigger(AnimatorFireTriggerName);    
    }
}
