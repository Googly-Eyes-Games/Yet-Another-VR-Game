using UnityEngine;
using erulathra;
using UnityEngine.SceneManagement;

public class SceneSubsystemInitializer : MonoBehaviour
{
    private void InstantiateSubsystems(SceneSubsystemManager manager)
    {
        if (SceneManager.GetActiveScene().name != GameplaySettings.Global.LevelScene)
        {
            manager.FindOrAddSubsystem<GameplayTimeSubsystem>();
            manager.FindOrAddSubsystem<HeartsSubsystem>();
            manager.FindOrAddSubsystem<ScoreSubsystem>();
        }
        else
        {
            manager.FindOrAddSubsystem<GameplayTimeSubsystem>();
            manager.FindOrAddSubsystem<ClientSubsystem>();
            manager.FindOrAddSubsystem<HeartsSubsystem>();
            manager.FindOrAddSubsystem<ScoreSubsystem>();
            manager.FindOrAddSubsystem<LevelSubsystem>();
        }
    }
	
    public void Awake()
    {
        SceneSubsystemManager.Initialize(gameObject, InstantiateSubsystems);
    }
}
