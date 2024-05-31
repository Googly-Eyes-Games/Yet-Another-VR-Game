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
        LivesSubsystem livesSubsystem = SceneSubsystemManager.GetSubsystem<LivesSubsystem>();
        livesSubsystem.OnLivesNumberChanged += HandleLivesNumberChanged;
    }

    private void HandleLivesNumberChanged(int newNumberOfLives)
    {
        if (newNumberOfLives >= 0)
        {
            healthText.text = $"Lives: {newNumberOfLives}";
        }
        else
        {
            healthText.text = $"Game Over";
        }
    }
}
