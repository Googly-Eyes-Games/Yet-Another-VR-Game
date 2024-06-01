using System;
using erulathra;

public class ScoreSubsystem : SceneSubsystem
{
    public Action<int> OnScoreChanged;

    public int CurrentScore { get; private set; } = 0;
    
    public override void Initialize()
    {
    }

    public void AddPoint()
    {
        LevelSubsystem levelSubsystem = SceneSubsystemManager.GetSubsystem<LevelSubsystem>();
        
        CurrentScore += levelSubsystem.CurrentLevelConfig.ScoreMultiplier;
        OnScoreChanged?.Invoke(CurrentScore);
    }

}