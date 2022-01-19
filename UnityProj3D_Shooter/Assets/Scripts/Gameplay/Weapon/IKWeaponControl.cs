using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKWeaponControl : MonoBehaviour
{
    [SerializeField] private Transform _iKRHandPoint;
    [SerializeField] private Transform _iKLHandPoint;
    private Animator _animator;
    private bool _iKControl;


    private void Awake()
    {
        if (TryGetComponent<Animator>(out Animator animator))
        {
            _animator = animator;
        }
    }

    public void ReinitializeIKs(Transform iKLeftHand, Transform iKRightHand) 
    {
        _iKLHandPoint = iKLeftHand;
        _iKRHandPoint = iKRightHand;
    }


    public void IKControlOn() 
    {
        _iKControl = true;
    }

    public void IKControlOff() 
    {
        _iKControl = false;
    }


    private void TakeWeaponViaIK() 
    {
        if (_iKControl)
        {
            if (_iKRHandPoint != null)
            {
                _animator?.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                _animator?.SetIKPosition(AvatarIKGoal.RightHand, _iKRHandPoint.position);
                _animator?.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                _animator?.SetIKRotation(AvatarIKGoal.RightHand, _iKRHandPoint.rotation);
            }
            if (_iKLHandPoint != null)
            {
                _animator?.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                _animator?.SetIKPosition(AvatarIKGoal.LeftHand, _iKLHandPoint.position);
                _animator?.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                _animator?.SetIKRotation(AvatarIKGoal.LeftHand, _iKLHandPoint.rotation);
            }
        }
        else
        {
            _animator?.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            _animator?.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            _animator?.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            _animator?.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        TakeWeaponViaIK();
    }

}
