using System.Collections.Generic;
using System.Linq;
using erulathra;
using NaughtyAttributes;
using UnityEngine;
using UnityTimer;

public class ClientSubsystem : SceneSubsystem
{
    private ClientQueue[] clientQueues;

    private float AccumulatedAcceleration = 1f;
    public float CurrentClientSpeed => GameplaySettings.Global.ClientBaseSpeed * AccumulatedAcceleration;

    private HashSet<Client> activeClients = new();

    private Timer autoSpawnTimer;

    private int spawnedClients = 0;
    private int maxAliveClients = 1;

    public override void Initialize()
    {
        clientQueues = FindObjectsByType<ClientQueue>(FindObjectsSortMode.None);

        GameplayTimeSubsystem gameplayTimeSubsystem = SceneSubsystemManager.GetSubsystem<GameplayTimeSubsystem>();
        gameplayTimeSubsystem.OnCountDownEnd += DoAutoSpawnLogic;
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public Client SpawnClient()
    {
        ClientQueue targetQueue = clientQueues.GetRandom();

        Client newClient;
        if (targetQueue.isRightQueue)
            newClient = targetQueue.SpawnClient(this, GameplaySettings.Global.RightClientPrefab);
        else
            newClient = targetQueue.SpawnClient(this, GameplaySettings.Global.LeftClientPrefab);
        
        activeClients.Add(newClient);
        spawnedClients++;
        newClient.OnClientExitedBar += HandleClientExitedBar;

        return newClient;
    }
    
    private void HandleClientExitedBar(Client client)
    {
        activeClients.Remove(client);

        if (activeClients.Count == 0)
        {
            DoAutoSpawnLogic();
        }
    }
    
    private void DoAutoSpawnLogic()
    {
        if (activeClients.Count >= maxAliveClients)
            return;

        int maxClientsNumToSpawn = maxAliveClients - activeClients.Count;
        int clientsNumToSpawn = Random.Range(1, maxClientsNumToSpawn);

        for (int clientID = 0; clientID < clientsNumToSpawn; clientID++)
        {
            SpawnClient();
        }

        autoSpawnTimer?.Cancel();

        Vector2 autoSpawnDurationRange = GameplaySettings.Global.TimeRangeToSpawnNewClient;
        float autoSpawnDuration = Random.Range(autoSpawnDurationRange.x, autoSpawnDurationRange.y);
        autoSpawnTimer = Timer.Register(autoSpawnDuration, DoAutoSpawnLogic, autoDestroyOwner: this);
        
        if (spawnedClients % GameplaySettings.Global.ClientsToIncreaseDifficulty == 0)
        {
            maxAliveClients++;
        }
        
        if (spawnedClients % GameplaySettings.Global.ClientsToIncreaseSpeed == 0)
        {
            AccumulatedAcceleration *= GameplaySettings.Global.ClientsAcceleration;
        }
    }
}
