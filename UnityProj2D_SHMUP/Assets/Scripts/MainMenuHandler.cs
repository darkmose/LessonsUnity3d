using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject chooseSpaceshipWindow;

    public void ChooseSpaceship(int indexSpaceship)
    {
        PlayerPrefs.SetInt("indexSpaceship", indexSpaceship);
    }

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnExitButtonClick()
    {
        Application.Quit();
    }

    private void OnStartButtonClick()
    {
        chooseSpaceshipWindow.SetActive(true);
    }

    public void ChooseAndromeda() 
    {
        PlayerPrefs.SetInt("indexSpaceship", 0);
        SceneManager.LoadScene("Scene1");
    }

    public void ChooseSpaceglader() 
    {
        PlayerPrefs.SetInt("indexSpaceship", 1);
        SceneManager.LoadScene("Scene1");
    }


}
