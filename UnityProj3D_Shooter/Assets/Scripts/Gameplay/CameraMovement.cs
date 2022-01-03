using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private const string MouseXAxisName = "Mouse X";
    private const string MouseYAxisName = "Mouse Y";
    private const float CameraViewClampMin = -20;
    private const float CameraViewClampMax = 40;
    private const float CameraShiftYClampMin = 1.3f;
    private const float CameraShiftYClampMax = 3.5f;
    private const float CameraShiftXClampMin = 1.5f;
    private const float CameraShiftXClampMax = 0.1f;
    private const float CameraThirdPersonZPos = -3f;
    private const float CameraFPSZPos = 0.5f;
    private const float CameraFPSYPos = 1.8f;
    private float _mouseSensivity = 300;
    private float _xRotation = 0f;
    private float _yRotation = 0f;
    [SerializeField] private Camera _playerCamera;
    private Transform _cameraTransform;
    private bool _fpsViewMode;

    private void Awake()
    {
        if (_playerCamera != null)
        {
            _cameraTransform = _playerCamera.transform;
        }
        else
        {
            _playerCamera = Camera.main;
            _cameraTransform = _playerCamera.transform;
        }
    }

    private void Start()
    { 
        Cursor.lockState = CursorLockMode.Locked;
    }

    private float GetScaledValue(float IN, float inMin, float inMax, float outMin, float outMax) 
    {
        float outValue = (IN - inMin) / (inMax - inMin);
        outValue = outValue * (outMax - outMin) + outMin;
        return outValue;
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
        _playerCamera.transform.localPosition = new Vector3(localPosX, CameraFPSYPos, CameraFPSZPos);
        _playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
    }

    private void CameraControlThirdPersonView ()
    {
        var moveX = Input.GetAxis(MouseXAxisName) * _mouseSensivity * Time.smoothDeltaTime;
        var moveY = Input.GetAxis(MouseYAxisName) * _mouseSensivity * Time.smoothDeltaTime;
        
        _xRotation -= moveY;
        _yRotation += moveX;

        _xRotation = Mathf.Clamp(_xRotation, CameraViewClampMin, CameraViewClampMax);

        float cameraShiftY = 0;
        float cameraShiftX = 0;
       
        cameraShiftY = GetScaledValue(_xRotation, CameraViewClampMin, CameraViewClampMax, CameraShiftYClampMin, CameraShiftYClampMax);
        cameraShiftX = GetScaledValue(_xRotation, CameraViewClampMin, CameraViewClampMax, CameraShiftXClampMin, CameraShiftXClampMax);


        transform.localRotation = Quaternion.Euler(0,_yRotation,0);
        _playerCamera.transform.localPosition = new Vector3(cameraShiftX, cameraShiftY, CameraThirdPersonZPos);
        _playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0,0);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _fpsViewMode = !_fpsViewMode;
        }
    }

    private void LateUpdate()
    {
        if (_fpsViewMode)
        {
            CameraControlFPS();
        }
        else
        {
            CameraControlThirdPersonView();
        }
    }

}
