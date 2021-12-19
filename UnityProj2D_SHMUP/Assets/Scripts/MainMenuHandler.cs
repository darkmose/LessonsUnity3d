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
    [SerializeField] private Button optionsButton;
    [SerializeField] private GameObject chooseSpaceshipWindow;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private Toggle audioOff;
    [SerializeField] private Toggle musicOff;


    private void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        optionsButton.onClick.AddListener(OnOptionsButtonClick);
        audioOff.onValueChanged.AddListener(OnAudioOffToggle);
        musicOff.onValueChanged.AddListener(OnMusicOffToggle);

        Button[] buttons = transform.GetComponentsInChildren<Button>();
        Debug.Log($"{buttons.Length} buttons");
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(ClickSound);
        }    
    }

    private void Start()
    {
        var audioMute = PlayerPrefs.GetInt("AudioMute") == 1;
        var musicMute = PlayerPrefs.GetInt("MusicMute") == 1;
        AudioManager.AudioMute(audioMute);
        AudioManager.MusicMute(musicMute);
        audioOff.isOn = audioMute;
        musicOff.isOn = musicMute;
        AudioManager.StopMusic();
        AudioManager.PlayMusic("Poseidon's");
    }

    public void ClickSound() 
    {
        AudioManager.PlaySound("Click");
    }


    private void OnMusicOffToggle(bool state)
    {
        AudioManager.MusicMute(state);
        PlayerPrefs.SetInt("MusicMute", state ? 1 : 0);
    }

    private void OnAudioOffToggle(bool state)
    {   
        AudioManager.AudioMute(state);
        PlayerPrefs.SetInt("AudioMute", state ? 1 : 0);
    }

    private void OnOptionsButtonClick()
    {
        optionsMenu.SetActive(true);
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
        AudioManager.StopMusic();
        SceneManager.LoadScene("Scene1");
    }

    public void ChooseSpaceglader() 
    {
        PlayerPrefs.SetInt("indexSpaceship", 1);
        AudioManager.StopMusic();
        SceneManager.LoadScene("Scene1");
    }


}
