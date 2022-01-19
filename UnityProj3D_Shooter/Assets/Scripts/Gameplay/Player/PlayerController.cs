using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region CONST_PARAMS
    private const string WeaponItemTag = "WeaponItem";
    private const string AmmoItemTag = "AmmoItem";
    private const string HealthItemTag = "HealthItem";
    private const float ForwardSpeedAcceleration = 30f;
    private const float SidewaySpeedAcceleration = 25f;
    private const float ScrollAccuracyValue = 0.2f;
    private const string HorizontalAxisName = "Horizontal";
    private const string VerticalAxisName = "Vertical";
    private const int LeftMouseButtonIndex = 0;
    private const int JumpImpulseY = 50;
    private const string GroundTagName = "Ground";
    #endregion

    private Rigidbody _rigidbody;
    private Vector3 _currentVelocity;
    private Vector2 _scrollAccuracy = new Vector2(0, ScrollAccuracyValue);
    private bool _isJumping = false;
    private bool _isOnGround = true;
    [SerializeField] private VitalitySystem _vitalitySystem;
    [SerializeField] private PlayerWeaponSystem _weaponSystem;
    private OnPlayerTakeDamageEvent _onPlayerTakeDamageEvent = new OnPlayerTakeDamageEvent();
    private FloatingJoystick _moveJoystick;

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

    private void Start()
    {
        _moveJoystick = GameManager.GetMoveJoystick();
        GameManager.JumpButtonAddListener(OnJumpScreenButtonClickHandler);
        GameManager.FireButtonAddListener(OnFireScreenButtonClickHandler);
        GameManager.ReloadButtonAddListener(OnReloadButtonClickHandler);
    }

    private void OnReloadButtonClickHandler() 
    {
        Debug.Log("IS RELOAD");
        _weaponSystem?.ReloadWeapon();
    }

    private void OnFireScreenButtonClickHandler()
    {
        Debug.Log("IS Fire");
        _weaponSystem?.Fire();
    }

    private void OnJumpScreenButtonClickHandler()
    {
        if (!_isJumping && _isOnGround)
        {
            _isJumping = true;
            _isOnGround = false;  
        }
    }

    private void WeaponInputScan() 
    {
        if (_weaponSystem.IsBurstShooting)
        {
            if (Input.GetMouseButton(LeftMouseButtonIndex))
            {
                _weaponSystem.Fire();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(LeftMouseButtonIndex))
            {
                _weaponSystem.Fire();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _weaponSystem?.ReloadWeapon();
        }

        if (Input.mouseScrollDelta.magnitude > _scrollAccuracy.magnitude)
        {
            if (Input.mouseScrollDelta.y >= ScrollAccuracyValue)
            {
                _weaponSystem.NextWeapon();
            }
            if (Input.mouseScrollDelta.y <= -ScrollAccuracyValue)
            {
                _weaponSystem.PrevWeapon();
            }
        }
    }

    [ContextMenu("TakeDamage")]
    private void Test() 
    {
        TakeDamage(15, "KILLER", Weapon.WeaponType.M1911);
    }
    
    
    public void TakeDamage(int damage, string killerName, Weapon.WeaponType weaponType) 
    {
        _vitalitySystem.TakeDamage(damage, killerName, weaponType);
        _onPlayerTakeDamageEvent.damage = damage;
        _onPlayerTakeDamageEvent.currentHP = _vitalitySystem.Health;
        EventsAgregator.Post<OnPlayerTakeDamageEvent>(this, _onPlayerTakeDamageEvent);
    }

    private void PCMotionInput()
    {
        var velocityY = _rigidbody.velocity.y;
        Vector3 moveHoriz = transform.right * Input.GetAxis(HorizontalAxisName);
        Vector3 moveVert = transform.forward * Input.GetAxis(VerticalAxisName);
        _currentVelocity = (ForwardSpeedAcceleration * moveHoriz) + (SidewaySpeedAcceleration * moveVert) + Vector3.up * velocityY;

        if (!_isJumping && _isOnGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isJumping = true;
                _isOnGround = false;
            }
        }
    }


    private void AssignNewVelocity() 
    {
        if (_isJumping)
        {
            _rigidbody.AddForce(0, JumpImpulseY, 0, ForceMode.Impulse);
            _isJumping = false;
        }
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
                }
                break;
            case AmmoItemTag:
                if (other.TryGetComponent(out Ammunition ammo))
                {
                    _weaponSystem?.TakeAmmo(ammo);
                }
                break;
            case HealthItemTag:
                if (other.TryGetComponent(out MedicinePack medicine))
                {
                    _vitalitySystem?.TakeHealth(medicine);
                }
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(GroundTagName))
        {
            _isOnGround = true;
        }
    }

    private void AndroidMotionInput()
    {
        var velocityY = _rigidbody.velocity.y;
        Vector3 moveHoriz = transform.right * _moveJoystick.Horizontal;
        Vector3 moveVert = transform.forward * _moveJoystick.Vertical;
        _currentVelocity = (ForwardSpeedAcceleration * moveHoriz) + (SidewaySpeedAcceleration * moveVert) + Vector3.up * velocityY;
    }
    
    private void FixedUpdate()
    {

#if UNITY_STANDALONE_WIN
        PCMotionInput();
#else
        AndroidMotionInput();
#endif
        AssignNewVelocity();
    }



    private void Update()
    {
        //WeaponInputScan();
    }

}
