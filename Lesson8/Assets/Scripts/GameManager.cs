using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private const int MainSceneIndex = 0;
    [SerializeField] private Button _retryButton;
    public static GameManager instance;
    public Slider rangeSlider;
    
    private void Awake()
    {
        if (instance!=null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            instance = this;
        }

        _retryButton.onClick.AddListener(OnRetryButtonClickHandler);
    }

    private void OnRetryButtonClickHandler()
    {
        SceneManager.LoadScene(MainSceneIndex);
    }
}
