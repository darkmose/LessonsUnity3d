using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private bool isHoming;
    [SerializeField] private float timeToLive = 3f;
    public float damage;

    private void OnEnable()
    {
        Invoke("Destroy", timeToLive);
    }


    private void Destroy()
    {
        this.gameObject.SetActive(false);
    }

}
