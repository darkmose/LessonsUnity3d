using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class EndGameHandler : MonoBehaviour
{
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject loseText;
    [SerializeField] private Text score;

    private void OnEnable()
    {
        score.text = $"You have {GameManager.GetScore()} scorepoints";
    }

    public void InitText(bool win) 
    {
        if (win)
        {
            winText.SetActive(true);
            loseText.SetActive(false);
        }
        else
        {
            winText.SetActive(false);
            loseText.SetActive(true);
        }
    }

    public void ExitButton()
    {
        AudioManager.StopMusic();
        Application.Quit();  
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1;
        AudioManager.StopMusic();
        DOTween.KillAll();
        SceneManager.LoadScene("MainMenu");
    }

}
