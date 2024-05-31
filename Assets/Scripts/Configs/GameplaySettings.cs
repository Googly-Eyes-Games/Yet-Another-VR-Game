using NaughtyAttributes;
using UnityEngine;

public class GameplaySettings : ScriptableObject
{
    [field: Foldout("Lives")]
    [field: SerializeField]
    public int StartLives { get; private set; } = 3;
    
    [field: Foldout("Clients")]
    [field: SerializeField]
    public float ClientBaseSpeed { get; private set; } = 1f;

    [field: Foldout("Clients")]
    [field: SerializeField]
    public float ClientReturnSpeed { get; private set; } = 3f;

    [field: Foldout("Clients")]
    [field: SerializeField]
    public float ClientReturnMugSpeed { get; private set; } = 3f;

    [field: Foldout("Prefab")]
    [field: SerializeField]
    public GameObject LeftClientPrefab { get; private set; }

    [field: Foldout("Prefab")]
    [field: SerializeField]
    public GameObject RightClientPrefab { get; private set; }

    [field: Foldout("Scenes")]
    [field: Scene]
    [field: SerializeField]
    public string MenuScene { get; private set; }
    
    [field: Foldout("Scenes")]
    [field: Scene]
    [field: SerializeField]
    public string GameOverScene { get; private set; }
    
    [field: Foldout("Scenes")]
    [field: Scene]
    [field: SerializeField]
    public string LevelScene { get; private set; }

    private static GameplaySettings globalSettingsInstance;

    public static GameplaySettings Global
    {
        get
        {
            if (!globalSettingsInstance)
            {
                globalSettingsInstance = Resources.Load<GameplaySettings>("GlobalGameplaySettings");
            }

            return globalSettingsInstance;
        }
    }
}