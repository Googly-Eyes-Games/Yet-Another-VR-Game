using System;
using erulathra;
using UnityEngine;
using UnityEngine.Pool;
using UnityTimer;
using Random = UnityEngine.Random;

public class ClientSubsystem : SceneSubsystem
{
    public Action<int> OnSatisfiedClientExit;
    
    public float CurrentClientSpeed => GameplaySettings.Global.ClientBaseSpeed * accumulatedAcceleration;
    public int SatisfiedClients { get; private set; } = 0;
    
    private ClientQueue[] clientQueues;
    private MugSpawner mugSpawner;

    private ObjectPool<Client> leftClientPool;
    private ObjectPool<Client> rightClientPool;

    private int ActiveClients => leftClientPool.CountActive + rightClientPool.CountActive;

    private Timer autoSpawnTimer;

    private int spawnedClients = 0;
    
    private float accumulatedAcceleration = 1f;

    public override void Initialize()
    {
        clientQueues = FindObjectsByType<ClientQueue>(FindObjectsSortMode.None);
        mugSpawner = FindObjectOfType<MugSpawner>();

        leftClientPool = new ObjectPool<Client>(
            () => InitializeClient(GameplaySettings.Global.LeftClientPrefab),
            OnGetClient, OnReleaseClient);
        
        rightClientPool = new ObjectPool<Client>(
            () => InitializeClient(GameplaySettings.Global.RightClientPrefab),
            OnGetClient, OnReleaseClient);

        GameplayTimeSubsystem gameplayTimeSubsystem = SceneSubsystemManager.GetSubsystem<GameplayTimeSubsystem>();
        gameplayTimeSubsystem.OnCountDownEnd += DoAutoSpawnLogic;
    }

    private Client InitializeClient(GameObject clientPrefab)
    {
        GameObject newClient = Instantiate(clientPrefab);
        newClient.SetActive(false);

        Client clientComponent = newClient.GetComponent<Client>();
        clientComponent.OnClientExitedBar += HandleClientExitedBar;
        
        return newClient.GetComponent<Client>();
    }

    private void OnGetClient(Client client)
    {
        client.gameObject.SetActive(true);
    }

    private void OnReleaseClient(Client client)
    {
        client.gameObject.SetActive(false);
    }
    
    
    public Client SpawnClient()
    {
        ClientQueue targetQueue = clientQueues.GetRandom();

        Client newClient = targetQueue.isRightQueue ? rightClientPool.Get() : leftClientPool.Get();

        targetQueue.InitializeClient(newClient);
        spawnedClients++;

        return newClient;
    }
    
    private void HandleClientExitedBar(Client client)
    {
        if (client.State == ClientState.DrinkingBeer)
        {
            SatisfiedClients++;
            OnSatisfiedClientExit?.Invoke(SatisfiedClients);
        }
        
        if (client.IsRightClient)
            rightClientPool.Release(client);
        else
            leftClientPool.Release(client);
        
        if (ActiveClients == 0)
        {
            DoAutoSpawnLogic();
        }
    }
    
    private void DoAutoSpawnLogic()
    {
        LevelSubsystem levelSubsystem = SceneSubsystemManager.GetSubsystem<LevelSubsystem>();
        LevelConfig currentLevelConfig = levelSubsystem.CurrentLevelConfig;
        
        if (ActiveClients >= currentLevelConfig.MaxClients)
            return;

        int maxClientsNumToSpawn = currentLevelConfig.MaxClients - ActiveClients;
        int clientsNumToSpawn = Random.Range(1, maxClientsNumToSpawn);

        for (int clientID = 0; clientID < clientsNumToSpawn; clientID++)
        {
            SpawnClient();
        }

        autoSpawnTimer?.Cancel();

        Vector2 autoSpawnDurationRange = currentLevelConfig.TimeRangeToSpawnNewClient;
        float autoSpawnDuration = Random.Range(autoSpawnDurationRange.x, autoSpawnDurationRange.y);
        autoSpawnTimer = Timer.Register(autoSpawnDuration, DoAutoSpawnLogic, autoDestroyOwner: this);
    }
}
