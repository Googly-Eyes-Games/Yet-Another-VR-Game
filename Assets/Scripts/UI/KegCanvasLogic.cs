using erulathra;
using TMPro;
using UnityEngine;

public class KegCanvas : MonoBehaviour
{
    [SerializeField]
    private TMP_Text healthText;
    
    [SerializeField]
    private TMP_Text scoreText;

    private void Awake()
    {
        HeartsSubsystem heartsSubsystem = SceneSubsystemManager.GetSubsystem<HeartsSubsystem>();
        heartsSubsystem.OnLivesNumberChanged += HandleLivesNumberChanged;

        ScoreSubsystem scoreSubsystem = SceneSubsystemManager.GetSubsystem<ScoreSubsystem>();
        scoreSubsystem.OnScoreChanged += HandleScoreChanged;
    }

    private void HandleScoreChanged(int newScore)
    {
        scoreText.text = $"Score: {newScore}";
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
