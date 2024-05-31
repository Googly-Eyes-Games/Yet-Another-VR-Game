using UnityEngine;
using erulathra;

public class SceneSubsystemInitializer : MonoBehaviour
{
    private void InstantiateSubsystems(SceneSubsystemManager manager)
    {
        manager.FindOrAddSubsystem<ClientSubsystem>();
        manager.FindOrAddSubsystem<LivesSubsystem>();
    }
	
    public void Awake()
    {
        SceneSubsystemManager.Initialize(gameObject, InstantiateSubsystems);
    }
}
