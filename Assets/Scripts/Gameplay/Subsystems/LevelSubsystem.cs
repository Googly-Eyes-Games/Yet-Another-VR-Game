using System;
using erulathra;
using NaughtyAttributes;
using UnityEngine;

public class LevelSubsystem : SceneSubsystem
{
    public event Action<int> OnNextLevel;
    
    public int CurrentLevel { get; private set; }
    public LevelConfig CurrentLevelConfig => GameplaySettings.Global.Levels[CurrentLevel];
    
    public override void Initialize()
    {
        ClientSubsystem clientSubsystem = SceneSubsystemManager.GetSubsystem<ClientSubsystem>();
        clientSubsystem.OnSatisfiedClientExit += HandleSatisfiedClientExit;
    }

    public override void PreAwake()
    {
        HandleLevelChanged(0);
    }

    private void HandleSatisfiedClientExit(int numberSatisfiedClients)
    {
        if (GameplaySettings.Global.Levels.Length > CurrentLevel + 1)
        {
            LevelConfig nextConfig = GameplaySettings.Global.Levels[CurrentLevel + 1];
            if (numberSatisfiedClients >= nextConfig.SatisfiedClientsToUnlockLevel)
            {
                HandleLevelChanged(CurrentLevel + 1);
            }
        }
    }

    private void HandleLevelChanged(int newLevel)
    {
        Debug.Log($"Level: {newLevel}");
        
        CurrentLevel = newLevel;
        OnNextLevel?.Invoke(newLevel);
    }
}

[Serializable]
public class LevelConfig
{
    [field: SerializeField]
    public int SatisfiedClientsToUnlockLevel { get; private set; }

    [field: Foldout("Client")]
    [field: SerializeField]
    public int MaxClients { get; private set; } = 1;

    [field: Foldout("Client")]
    [field: SerializeField]
    public float ClientSpeed { get; private set; } = 0.25f;

    [field: Foldout("Client")]
    [field: SerializeField]
    public float MugSpeed { get; private set; } = 1f;
    
    [field: Foldout("Clients")]
    [field: SerializeField]
    public Vector2 TimeRangeToSpawnNewClient { get; private set; } = new Vector2(2f, 5f);

    [field: Foldout("Clients")]
    [field: SerializeField]
    public bool AddHeart { get; private set; } = false;
    
    [field: Foldout("Clients")]
    [field: SerializeField]
    public int ScoreMultiplier { get; private set; } = 25;
}