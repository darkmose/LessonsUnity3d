using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScopeController : MonoBehaviour
{
    #region CONST_PARAM
    private const float CameraFOVScoped = 30f;
    private const float CameraFOVUnscoped = 60f;
    private struct TransformParamsAK74Scoped 
    {
        public const float pos_x = -0.007f;
        public const float pos_y = -0.12f;
        public const float pos_z = 0.6f;

        public const float rot_x = 0.43f;
        public const float rot_y = 1.6f;
        public const float rot_z = 0f;
    }

    private struct TransformParamsAK74Unscoped
    {
        public const float pos_x = 0.4f;
        public const float pos_y = -0.23f;
        public const float pos_z = 0.634f;

        public const float rot_x = -4.65f;
        public const float rot_y = 5.4f;
        public const float rot_z = -0.5f;
    }

    private struct TransformParamsM1911Scoped
    {
        public const float pos_x = 0;
        public const float pos_y = -0.2f;
        public const float pos_z = 0.6f;

        public const float rot_x = 0;
        public const float rot_y = 0;
        public const float rot_z = 0;
    }

    private struct TransformParamsM1911Unscoped
    {
        public const float pos_x = 0.4f;
        public const float pos_y = -0.23f;
        public const float pos_z = 0.7f;

        public const float rot_x = -4.7f;
        public const float rot_y = 5.4f;
        public const float rot_z = -0.5f;
    }
    #endregion

    private Camera _playerCamera;
    [SerializeField] private Image targetImage;
    [SerializeField] private Camera _fpsCamera;
    private Transform _aK74Transform;
    private Transform _m1911Transform;
    private bool IsAlreadyScoped { get; set; }
    public Weapon.WeaponType CurrentWeapon { private get; set; }
    private Vector3 AK74ScopedPosition = new Vector3(TransformParamsAK74Scoped.pos_x, TransformParamsAK74Scoped.pos_y, TransformParamsAK74Scoped.pos_z);
    private Vector3 AK74UnscopedPosition = new Vector3(TransformParamsAK74Unscoped.pos_x, TransformParamsAK74Unscoped.pos_y, TransformParamsAK74Unscoped.pos_z);
    private Vector3 M1911ScopedPosition = new Vector3(TransformParamsM1911Scoped.pos_x, TransformParamsM1911Scoped.pos_y, TransformParamsM1911Scoped.pos_z);
    private Vector3 M1911UnscopedPosition = new Vector3(TransformParamsM1911Unscoped.pos_x, TransformParamsM1911Unscoped.pos_y, TransformParamsM1911Unscoped.pos_z);
    private Vector3 AK74ScopedRotation = new Vector3(TransformParamsAK74Scoped.rot_x, TransformParamsAK74Scoped.rot_y, TransformParamsAK74Scoped.rot_z);
    private Vector3 AK74UnscopedRotation = new Vector3(TransformParamsAK74Unscoped.rot_x, TransformParamsAK74Unscoped.rot_y, TransformParamsAK74Unscoped.rot_z);
    private Vector3 M1911ScopedRotation = new Vector3(TransformParamsM1911Scoped.rot_x, TransformParamsM1911Scoped.rot_y, TransformParamsM1911Scoped.rot_z);
    private Vector3 M1911UnscopedRotation = new Vector3(TransformParamsM1911Unscoped.rot_x, TransformParamsM1911Unscoped.rot_y, TransformParamsM1911Unscoped.rot_z);

    private void Start()
    {
    }

    public void InitAK74Transform(Transform transform) 
    {
        _aK74Transform = transform;
    } 
    
    public void InitM1911Transform(Transform transform) 
    {
        _m1911Transform = transform;
    } 

    
    
    
    private void ChangeTransform(Transform currentTransform, Vector3 posDestination, Vector3 rotationDestination) 
    {
        //Vector3 currentPos = currentTransform.position;
        //Quaternion currentRotation = currentTransform.rotation;
        currentTransform.localPosition = posDestination;
        currentTransform.localEulerAngles = rotationDestination;
    }



    public void Scope() 
    {
        if (!IsAlreadyScoped)
        {
            targetImage.enabled = false;
            switch (CurrentWeapon)
            {
                case Weapon.WeaponType.AK74:
                    ChangeTransform(_aK74Transform, AK74ScopedPosition, AK74ScopedRotation);
                    break;
                case Weapon.WeaponType.M1911:
                    ChangeTransform(_m1911Transform, M1911ScopedPosition, M1911ScopedRotation);
                    break;
            }
            _fpsCamera.fieldOfView = CameraFOVScoped;
            _playerCamera = Camera.main;
            _playerCamera.fieldOfView = CameraFOVScoped;
            IsAlreadyScoped = true;
        }

    }
    public void UnScope()
    {
        if (IsAlreadyScoped)
        {
            targetImage.enabled = true;
            switch (CurrentWeapon)
            {
                case Weapon.WeaponType.AK74:
                    ChangeTransform(_aK74Transform, AK74UnscopedPosition, AK74UnscopedRotation);
                    break;
                case Weapon.WeaponType.M1911:
                    ChangeTransform(_m1911Transform, M1911UnscopedPosition, M1911UnscopedRotation);
                    break;
            }
            _fpsCamera.fieldOfView = CameraFOVUnscoped;
            _playerCamera.fieldOfView = CameraFOVUnscoped;
            IsAlreadyScoped = false;
        }
    }
}
