using System;
using erulathra;
using TMPro;
using UnityEngine;

public class HealthCanvas : MonoBehaviour
{
    [SerializeField]
    private TMP_Text healthText;

    private void Awake()
    {
        HeartsSubsystem heartsSubsystem = SceneSubsystemManager.GetSubsystem<HeartsSubsystem>();
        heartsSubsystem.OnLivesNumberChanged += HandleLivesNumberChanged;
    }

    private void HandleLivesNumberChanged(int newNumberOfLives)
    {
        if (newNumberOfLives >= 0)
        {
            healthText.text = $"Hearts: {newNumberOfLives}";
        }
        else
        {
            healthText.text = $"Game Over";
        }
    }
}
