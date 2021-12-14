using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsLimit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAmmo") || collision.CompareTag("EnemyAmmo"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
