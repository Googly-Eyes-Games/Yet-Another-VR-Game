using erulathra;
using UnityEngine;
using UnityEngine.Pool;
using UnityTimer;

public class ClientSubsystem : SceneSubsystem
{
    public float CurrentClientSpeed => GameplaySettings.Global.ClientBaseSpeed * accumulatedAcceleration;
    
    private ClientQueue[] clientQueues;
    private MugSpawner mugSpawner;

    private ObjectPool<Client> leftClientPool;
    private ObjectPool<Client> rightClientPool;

    private int ActiveClients => leftClientPool.CountActive + rightClientPool.CountActive;

    private Timer autoSpawnTimer;

    private int spawnedClients = 0;
    private int maxAliveClients = 1;
    
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
        if (ActiveClients >= maxAliveClients)
            return;

        int maxClientsNumToSpawn = maxAliveClients - ActiveClients;
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
            mugSpawner.SetTargetMugsAmount(maxAliveClients);
        }
        
        if (spawnedClients % GameplaySettings.Global.ClientsToIncreaseSpeed == 0)
        {
            accumulatedAcceleration *= GameplaySettings.Global.ClientsAcceleration;
        }
    }
}
