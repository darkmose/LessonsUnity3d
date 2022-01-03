using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MotionThreshold = 0.1f;
    private const float NegativeMotionThreshold = -0.1f;
    private const float MaximumVelocity = 10f;
    private const float ForwardSpeedAcceleration = 30f;
    private const float SidewaySpeedAcceleration = 25f;
    private const string HorizontalAxisName = "Horizontal";
    private const string VerticalAxisName = "Vertical";
    [SerializeField] private Animator _animator;
    private Rigidbody _rigidbody;
    private Vector3 _controlInput;
    private Vector3 _currentVelocity;
    [SerializeField] private Camera _playerCamera;
    private enum MoveDirection { None, Left, Right, Forward, Backward }
    private MoveDirection _moveDirection = MoveDirection.None;


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


    private void FixedUpdate()
    {
        AssignNewVelocity();
    }


    private void Update()
    {
        PCMotionInput();
    }
}
