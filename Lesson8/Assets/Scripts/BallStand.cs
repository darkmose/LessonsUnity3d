using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStand : MonoBehaviour
{
    [SerializeField] private FixedJoint _joint;

    private void Awake()
    {
        EventDelegate.OnCannonballHitEvent += OnCannonballHitHandler;
    }

    private void OnCannonballHitHandler()
    {
        _joint.breakForce = 0f;       
    }
}
