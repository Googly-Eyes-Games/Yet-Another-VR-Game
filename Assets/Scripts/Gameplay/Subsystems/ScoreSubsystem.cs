using System;
using erulathra;

public class ScoreSubsystem : SceneSubsystem
{
    public Action<int> OnScoreChanged;

    public int CurrentScore { get; private set; } = 0;
    public int CurrentScoreMultiplier { get; set; } = 25;
    
    public override void Initialize()
    {
    }

    public void AddPoint()
    {
        CurrentScore += CurrentScoreMultiplier;
        OnScoreChanged?.Invoke(CurrentScore);
    }

}