using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private const string MouseXAxisName = "Mouse X";
    private const string MouseYAxisName = "Mouse Y";
    private const float CameraViewClampMin = -20;
    private const float CameraViewClampMax = 60;
    private float _mouseSensivity = 150;
    private float _xRotation = 0f;
    private float _yRotation = 0f;
    [SerializeField] private Camera _playerCamera;
    private bool _fpsViewMode = true;
    private FloatingJoystick _viewJoystick;

    private void Awake()
    {
        if (_playerCamera == null)
        {
            _playerCamera = Camera.main;
        }
    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        _viewJoystick = GameManager.GetViewJoystick();
    }


    private void CameraControlFPS()
    {
        var moveX = Input.GetAxis(MouseXAxisName) * _mouseSensivity * Time.smoothDeltaTime;
        var moveY = Input.GetAxis(MouseYAxisName) * _mouseSensivity * Time.smoothDeltaTime;

        _xRotation -= moveY;
        _yRotation += moveX;

        _xRotation = Mathf.Clamp(_xRotation, CameraViewClampMin, CameraViewClampMax);
        var localPosX = _playerCamera.transform.localPosition.z;
        var localPosY = _playerCamera.transform.localPosition.z;

        transform.localRotation = Quaternion.Euler(0, _yRotation, 0);
        _playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
    }

    private void CameraControlAndroid() 
    {
        var moveX = _viewJoystick.Horizontal * _mouseSensivity * Time.smoothDeltaTime;
        var moveY = _viewJoystick.Vertical * _mouseSensivity * Time.smoothDeltaTime;

        _xRotation -= moveY;
        _yRotation += moveX;

        _xRotation = Mathf.Clamp(_xRotation, CameraViewClampMin, CameraViewClampMax);
        var localPosX = _playerCamera.transform.localPosition.z;
        var localPosY = _playerCamera.transform.localPosition.z;

        transform.localRotation = Quaternion.Euler(0, _yRotation, 0);
        _playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
    }

    private void LateUpdate()
    {
        CameraControlAndroid();
        //CameraControlFPS();
    }

}
