using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTarget : MonoBehaviour
{
    private const string CannonballTag = "Cannonball";
    [SerializeField] private GameObject _lamp;
    
    private void Awake()
    {
        LampRedLight();
    }

    private void LampRedLight()
    {
        if (_lamp.TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.material.color = Color.red;
        }
    }
    private void LampGreenLight()
    {
        if (_lamp.TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.material.color = Color.green;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(CannonballTag))
        {
            LampGreenLight();
            EventDelegate.RaiseOnCannonballHit();
        }
    }



}
