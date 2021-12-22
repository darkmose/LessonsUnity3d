using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButton : MonoBehaviour
{
    private const string CannonballTag = "Cannonball";
    [SerializeField] private BoxCollider _boxCollider;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(CannonballTag))
        {
            EventDelegate.RaiseOnPlatformButtonPress();
            Destroy(_boxCollider);
        }
    }
}
