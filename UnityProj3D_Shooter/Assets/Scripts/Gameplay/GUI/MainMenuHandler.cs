using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    private const int GameSceneBuildIndex = 1;
    private const string ClickSoundName = "Click";
    private const string MusicMutePlayerPrefsKey = "MusicMute";
    private const string AudioMutePlayerPrefsKey = "AudioMute";
    private const int PlayerPrefsOff = 0;
    private const int PlayerPrefsOn = 1;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _buttonsPanel;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _optionsBackButton;
    [SerializeField] private Toggle _soundsMuteToggle;
    [SerializeField] private Toggle _musicMuteToggle;
    private bool _playerPrefsAudioMute;
    private bool _playerPrefsMusicMute;

    private void Awake()
    {
        _startButton.onClick.AddListener(OnStartButtonClickHandler);
        _optionsButton.onClick.AddListener(OnOptionsButtonClickHandler);
        _exitButton.onClick.AddListener(OnExitButtonClickHandler);
        _optionsBackButton.onClick.AddListener(OnOptionsBackButtonClickHandler);
        _soundsMuteToggle.onValueChanged.AddListener(OnSoundsMuteToogleValueChangedHandler);
        _musicMuteToggle.onValueChanged.AddListener(OnMusicMuteToogleValueChangedHandler);
        if (PlayerPrefs.HasKey(MusicMutePlayerPrefsKey))
        {
            _playerPrefsMusicMute = PlayerPrefs.GetInt(MusicMutePlayerPrefsKey) > PlayerPrefsOff;
            AudioManager.MusicMute(_playerPrefsMusicMute);
        }
        if (PlayerPrefs.HasKey(AudioMutePlayerPrefsKey))
        {
            _playerPrefsAudioMute = PlayerPrefs.GetInt(AudioMutePlayerPrefsKey) > PlayerPrefsOff;
            AudioManager.AudioMute(_playerPrefsMusicMute);
        }
    }

    private void OnMusicMuteToogleValueChangedHandler(bool value)
    {
        AudioManager.MusicMute(value);
        var intResult = value ? 1 : 0;
        PlayerPrefs.SetInt(MusicMutePlayerPrefsKey, intResult);
    }

    private void OnSoundsMuteToogleValueChangedHandler(bool value)
    {
        AudioManager.AudioMute(value);
        var intResult = value ? 1 : 0; 
        PlayerPrefs.SetInt(AudioMutePlayerPrefsKey, intResult);
    }

    private void ClickSound() 
    {
        AudioManager.PlaySound(ClickSoundName);
    }

    private void OnExitButtonClickHandler()
    {
        ClickSound();
        Application.Quit();
    }

    private void OnOptionsButtonClickHandler()
    {
        ClickSound();
        _optionsPanel.SetActive(true);
        _buttonsPanel.SetActive(false);
        _soundsMuteToggle.isOn = _playerPrefsAudioMute;
        _musicMuteToggle.isOn = _playerPrefsMusicMute;
    }

    private void OnOptionsBackButtonClickHandler() 
    {
        ClickSound();
        _optionsPanel.SetActive(false);
        _buttonsPanel.SetActive(true);
    }

    private void OnStartButtonClickHandler()
    {
        ClickSound();
        SceneManager.LoadScene(GameSceneBuildIndex);
    }
}
