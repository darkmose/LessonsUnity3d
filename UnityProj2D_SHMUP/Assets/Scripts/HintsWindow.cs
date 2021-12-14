using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintsWindow : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            Time.timeScale = 1f;
            this.gameObject.SetActive(false);
        }
    }
}
