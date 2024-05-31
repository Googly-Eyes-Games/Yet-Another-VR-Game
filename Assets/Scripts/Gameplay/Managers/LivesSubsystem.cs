using System;
using erulathra;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

public class LivesSubsystem : SceneSubsystem
{
    public event Action<int> OnLivesNumberChanged;
    
    public int Lives { get; private set; }
    
    private BrokenMugsDetector brokenMugsDetector;
    
    public override void Initialize()
    {
        Lives = GameplaySettings.Global.StartLives;
            
        brokenMugsDetector = FindObjectOfType<BrokenMugsDetector>();
        brokenMugsDetector.OnMugBroken += HandleBrokenMug;
    }
    
    private void HandleBrokenMug()
    {
        Lives--;
        OnLivesNumberChanged?.Invoke(Lives);

        if (Lives < 0)
        {
            HandleGameOver();
        }
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void HandleGameOver()
    {
        SceneManager.LoadSceneAsync(GameplaySettings.Global.GameOverScene);
    }
}