using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string WeaponItemTag = "WeaponItem";
    private const string AmmoItemTag = "AmmoItem";
    private const string HealthItemTag = "HealthItem";
    private const float ForwardSpeedAcceleration = 30f;
    private const float SidewaySpeedAcceleration = 25f;
    private const string HorizontalAxisName = "Horizontal";
    private const string VerticalAxisName = "Vertical";
    private const int LeftMouseButtonIndex = 0;
    [SerializeField] private Animator _animator;
    private Rigidbody _rigidbody;
    private Vector3 _currentVelocity;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private VitalitySystem _vitalitySystem;
    [SerializeField] private PlayerWeaponSystem _weaponSystem;

    private void Awake()
    {
        if (TryGetComponent<Rigidbody>(out var rigidbody))
        {
            _rigidbody = rigidbody;
        }
        else
        {
            Debug.Log("Rigidbody doesn't exist");
        }

    }


    private void WeaponInputScan() 
    {
        if (_weaponSystem.IsBurstShooting)
        {
            if (Input.GetMouseButton(LeftMouseButtonIndex))
            {
                _weaponSystem?.Fire();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(LeftMouseButtonIndex))
            {
                _weaponSystem?.Fire();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _weaponSystem?.ReloadWeapon();
        }
    }

    private void PCMotionInput()
    {
        Vector3 moveHoriz = _playerCamera.transform.right * Input.GetAxis(HorizontalAxisName);
        Vector3 moveVert = _playerCamera.transform.forward * Input.GetAxis(VerticalAxisName);
        _currentVelocity = (ForwardSpeedAcceleration * moveHoriz) + (SidewaySpeedAcceleration * moveVert);
        _currentVelocity.y = 0;
    }

    private void AssignNewVelocity() 
    {
        _rigidbody.velocity = _currentVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case WeaponItemTag:

                if (other.TryGetComponent(out Weapon weapon))
                {
                    _weaponSystem?.TakeWeapon(weapon);
                    Debug.Log($"Take some weapon: {weapon.WeaponModel}");
                }
                
                break;
            case AmmoItemTag:
                break;
            case HealthItemTag:
                break;
        }
     
    }

    private void FixedUpdate()
    {
        PCMotionInput();
        AssignNewVelocity();
    }


    private void Update()
    {
        WeaponInputScan();
    }

}
