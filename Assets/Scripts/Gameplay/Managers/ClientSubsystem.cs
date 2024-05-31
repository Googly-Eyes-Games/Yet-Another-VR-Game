using erulathra;
using NaughtyAttributes;
using UnityEngine;

public class ClientSubsystem : SceneSubsystem
{
    private ClientQueue[] clientQueues;

    public float CurrentClientSpeed => GameplaySettings.Global.ClientBaseSpeed;

    public override void Initialize()
    {
        clientQueues = FindObjectsByType<ClientQueue>(FindObjectsSortMode.None);
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void SpawnClient()
    {
        ClientQueue targetQueue = clientQueues.GetRandom();
        
        if (targetQueue.isRightQueue)
            targetQueue.SpawnClient(this, GameplaySettings.Global.RightClientPrefab);
        else
            targetQueue.SpawnClient(this, GameplaySettings.Global.LeftClientPrefab);
    }

}
