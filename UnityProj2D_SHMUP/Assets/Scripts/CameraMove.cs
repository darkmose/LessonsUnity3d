using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CameraMove : MonoBehaviour
{
    [SerializeField] private Camera gameCamera;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private float gameplayTime = 300;
    private float timeCounter;
    private bool allowMovement;


    private void Start()
    {
        allowMovement = true;
        gameCamera = GetComponent<Camera>();
        startPoint = gameCamera.transform.position;
        endPoint = gameCamera.transform.position;
        endPoint.y += 1000f;
        EventDelegate.OnStartBossFightEvent += OnStartBossFightHandler;
    }

    private void OnStartBossFightHandler()
    {
        allowMovement = false;
    }

    private void Move()
    {
        if (timeCounter >= gameplayTime)
        {
            return;
        }
        var normalizedTime = timeCounter / gameplayTime;
        gameCamera.transform.position = Vector3.Lerp(startPoint, endPoint, normalizedTime);
        timeCounter += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (allowMovement)
        {
            Move();
        }
    }
}
