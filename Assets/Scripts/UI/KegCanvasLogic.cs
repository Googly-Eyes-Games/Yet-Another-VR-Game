using erulathra;
using TMPro;
using UnityEngine;

public class KegCanvas : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;
     
    [SerializeField]
    private TMP_Text levelText;

    private void Awake()
    {

        ScoreSubsystem scoreSubsystem = SceneSubsystemManager.GetSubsystem<ScoreSubsystem>();
        scoreSubsystem.OnScoreChanged += HandleScoreChanged;
        
        LevelSubsystem levelSubsystem = SceneSubsystemManager.GetSubsystem<LevelSubsystem>();
        levelSubsystem.OnNextLevel += HandleNextLevel;
    }

    private void HandleNextLevel(int levelNumber)
    {
        levelText.text = $"Level: {levelNumber + 1}";
    }

    private void HandleScoreChanged(int newScore)
    {
        scoreText.text = $"{newScore}";
    }
}
