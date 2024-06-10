using System;
using erulathra;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

public class HeartsSubsystem : SceneSubsystem
{
    public event Action<int> OnLivesNumberChanged;
    
    public int Hearts { get; private set; }
    
    
    public override void Initialize()
    {
        Hearts = GameplaySettings.Global.StartHearts;
    }
    
    public void HandleBrokenMug()
    {
        DecrementHearts();
    }
    
    public void HandleUnsatisfiedClient()
    {
        DecrementHearts();
    }

    private void DecrementHearts()
    {
        Hearts--;
        OnLivesNumberChanged?.Invoke(Hearts);
        
        if (Hearts < 0)
        {
            HandleGameOver();
        }
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void HandleGameOver()
    {
        TransitionsSceneManger.Get().LoadGameOver();
    }
}