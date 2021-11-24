using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button cinematicButton;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle soundInBackgroundToggle;
    [SerializeField] private Toggle challengingNearbyToggle;
    [SerializeField] private Toggle allowFriendsSpectateToggle;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TMPro.TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMPro.TMP_Dropdown qualityDropdown;


    private void Awake()
    {
        creditsButton.onClick.AddListener(OnCreditsButtonClick);
        cinematicButton.onClick.AddListener(OnCinematicButtonClick);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleClick);
        soundInBackgroundToggle.onValueChanged.AddListener(OnSoundInBackgroundToggleClick);
        challengingNearbyToggle.onValueChanged.AddListener(OnNearbyChallengeToggleClick);
        allowFriendsSpectateToggle.onValueChanged.AddListener(OnFriendsSpectateToggleClick);
        soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionDropdownChanged);
        qualityDropdown.onValueChanged.AddListener(OnQualityDropdownChanged);
    }

    private void OnCreditsButtonClick() 
    {
        Debug.Log($"{creditsButton.gameObject.name} was clicked");
    }
    private void OnCinematicButtonClick() 
    {
        Debug.Log($"{cinematicButton.gameObject.name} was clicked");
    }
    private void OnFullscreenToggleClick(bool toggle) 
    {
        Debug.Log($"{fullscreenToggle.gameObject.name} is set to {toggle}");
    }
    private void OnSoundInBackgroundToggleClick(bool toggle) 
    {
        Debug.Log($"{soundInBackgroundToggle.gameObject.name} is set to {toggle}");
    }
    private void OnFriendsSpectateToggleClick(bool toggle) 
    {
        Debug.Log($"{allowFriendsSpectateToggle.gameObject.name} is set to {toggle}");
    }
    private void OnNearbyChallengeToggleClick(bool toggle) 
    {
        Debug.Log($"{challengingNearbyToggle.gameObject.name} is set to {toggle}");
    }
    private void OnSoundSliderChanged(float value) 
    {
        Debug.Log($"{soundSlider.gameObject.name} is set to {value}");
    }
    private void OnMusicSliderChanged(float value) 
    {
        Debug.Log($"{musicSlider.gameObject.name} is set to {value}");
    }
    private void OnResolutionDropdownChanged(int value) 
    {
        Debug.Log($"{resolutionDropdown.gameObject.name} is set to {resolutionDropdown.options[value].text}");
    }
    private void OnQualityDropdownChanged(int value) 
    {
        Debug.Log($"{qualityDropdown.gameObject.name} is set to {qualityDropdown.options[value].text}");
    }
    

}
