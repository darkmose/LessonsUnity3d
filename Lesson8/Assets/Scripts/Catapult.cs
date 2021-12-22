using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Catapult : MonoBehaviour
{
    private const float DefaultSpringForce = 500f; 
    private const float TimeToFreezeCatapult = 1.5f; 
    [SerializeField] private GameObject _basket;
    [SerializeField] private GameObject _weight;
    [SerializeField] private SpringJoint _weightSpring;
    private Slider _rangeSlider;
    [SerializeField] private GameObject _catapultShoulder;

    private void Start()
    {
        _rangeSlider = GameManager.instance.rangeSlider;
        _rangeSlider.onValueChanged.AddListener(OnRangeSliderValueChangedHandler);
        _weightSpring.spring = DefaultSpringForce;
    }

    private void OnRangeSliderValueChangedHandler(float value)
    {
        _weightSpring.spring = value;
    }

    private void FireCatapult() 
    {
        var basketRigidbody = _basket.GetComponent<Rigidbody>();
        var weightRigidbody = _weight.GetComponent<Rigidbody>();
        basketRigidbody.constraints = RigidbodyConstraints.None;
        weightRigidbody.constraints = RigidbodyConstraints.None;
        Invoke("FreezeCatapult", TimeToFreezeCatapult);
    }

    private void FreezeCatapult()
    {
        var shoulderRigidbody = _catapultShoulder.GetComponent<Rigidbody>();
        var basketRigidbody = _basket.GetComponent<Rigidbody>();
        var weightRigidbody = _weight.GetComponent<Rigidbody>();
        shoulderRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        basketRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        weightRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireCatapult();
        }
    }

}
