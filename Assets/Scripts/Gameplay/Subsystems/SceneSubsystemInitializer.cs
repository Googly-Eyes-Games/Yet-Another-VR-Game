using UnityEngine;
using erulathra;

public class SceneSubsystemInitializer : MonoBehaviour
{
    private void InstantiateSubsystems(SceneSubsystemManager manager)
    {
        manager.FindOrAddSubsystem<GameplayTimeSubsystem>();
        manager.FindOrAddSubsystem<ClientSubsystem>();
        manager.FindOrAddSubsystem<HeartsSubsystem>();
        manager.FindOrAddSubsystem<ScoreSubsystem>();
    }
	
    public void Awake()
    {
        SceneSubsystemManager.Initialize(gameObject, InstantiateSubsystems);
    }
}
