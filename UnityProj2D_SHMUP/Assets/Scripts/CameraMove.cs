using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Camera gameCamera;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private float gameplayTime = 180;
    private float timeCounter;


    void Start()
    {
        gameCamera = GetComponent<Camera>();
        startPoint = gameCamera.transform.position;
        endPoint = gameCamera.transform.position;
        endPoint.y += 200f;
    }


    private void Move()
    {
        if (timeCounter >= gameplayTime)
        {
            OnEndMapEvent?.Invoke();
            return;
        }
        var normalizedTime = timeCounter / gameplayTime;
        gameCamera.transform.position = Vector3.Lerp(startPoint, endPoint, normalizedTime);
        timeCounter += Time.deltaTime;
    }

    event System.Action OnEndMapEvent;

    private void FixedUpdate()
    {
        Move();
    }
}
