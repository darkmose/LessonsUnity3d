using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ContinueMenuHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI creditsLeft;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        creditsLeft.text = $"You have {GameManager.Credits} credits";
    }
}
