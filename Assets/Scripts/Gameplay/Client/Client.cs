using System;
using erulathra;
using UnityEngine;

public enum ClientState
{
    WantsBeer,
    Unsatisfied,
    DrinkingBeer,
}

public class Client : MonoBehaviour
{
    public event Action<ClientState /* newClientState */> OnClientStateChanged;
    public event Action<Client /* client */ > OnClientExitedBar;
    
    [SerializeField]
    private ClientHandTrigger clientHandTrigger;
    
    [field: SerializeField]
    public bool IsRightClient { get; private set; }
    
    public float WalkSpeed { get; private set; }
    public float ReturnSpeed { get; private set; }
    public float MugReturnSpeed { get; private set; }
    
    public ClientState State { get; private set; } = ClientState.WantsBeer;
    
    private float timeSpawned;
    private float timeReturningStarted;
    private Vector3 startReturnPosition;

    private ClientQueue clientQueue;
    public MugComponent CollectedMug { get; private set; }

    
    public void SetState(ClientState newState)
    {
        State = newState;
        OnClientStateChanged?.Invoke(State);
    }

    public void Initialize(ClientQueue queue)
    {
        ClientSubsystem clientSubsystem = SceneSubsystemManager.GetSubsystem<ClientSubsystem>();
        WalkSpeed = clientSubsystem.CurrentClientSpeed;

        LevelSubsystem levelSubsystem = SceneSubsystemManager.GetSubsystem<LevelSubsystem>();
        LevelConfig currentLevelConfig = levelSubsystem.CurrentLevelConfig;
        
        clientQueue = queue;
        timeSpawned = Time.timeSinceLevelLoad;
        ReturnSpeed = GameplaySettings.Global.ClientReturnSpeed;
        MugReturnSpeed = currentLevelConfig.MugSpeed;

        State = ClientState.WantsBeer;
        clientHandTrigger.enabled = true;
    }

    public void Awake()
    {
        clientHandTrigger.OnMugCollected += OnMugCollected;
    }

    private void Update()
    {
        if (State == ClientState.WantsBeer)
        {
            HandleGoToBarLogic();
        }
        else if (State is ClientState.DrinkingBeer
                       or ClientState.Unsatisfied)
        {
            HandleReturnLogic();
        }
    }

    private void HandleGoToBarLogic()
    {
        float queueLength = clientQueue.length;

        float elapsedTime = Time.timeSinceLevelLoad - timeSpawned;
        float queueFract = elapsedTime * WalkSpeed / queueLength;

        transform.position = Vector3.Lerp(
            clientQueue.startPoint.position,
            clientQueue.endPoint.position,
            queueFract
        );

        if (queueFract >= 1f - Single.Epsilon)
        {
            SetState(ClientState.Unsatisfied);
            
            HeartsSubsystem heartsSubsystem = SceneSubsystemManager.GetSubsystem<HeartsSubsystem>();
            heartsSubsystem.HandleUnsatisfiedClient();
            
            ReturnToExit();
        }
    }

    private void HandleReturnLogic()
    {
        float queueLength = Vector3.Distance(startReturnPosition, clientQueue.returnQueueEndPoint.position);

        float elapsedTime = Time.timeSinceLevelLoad - timeReturningStarted;
        float queueFract = elapsedTime * ReturnSpeed / queueLength;

        transform.position = Vector3.Lerp(
            startReturnPosition,
            clientQueue.returnQueueEndPoint.position,
            queueFract
        );

        if (queueFract >= 1f - Single.Epsilon)
        {
            ExitTavern();
        }
    }

    private void OnMugCollected(MugComponent mug)
    {
        if (mug.FillPercentage > GameplaySettings.Global.MinimalMugFillAmount
            && mug.IsClean)
        {
            SetState(ClientState.DrinkingBeer);
            
            CollectedMug = mug;
            
            ScoreSubsystem scoreSubsystem = SceneSubsystemManager.GetSubsystem<ScoreSubsystem>();
            scoreSubsystem.AddPoint();
        }
        else
        {
            SetState(ClientState.Unsatisfied);
            
            mug.DestroyMug();
            
            HeartsSubsystem heartsSubsystem = SceneSubsystemManager.GetSubsystem<HeartsSubsystem>();
            heartsSubsystem.HandleUnsatisfiedClient();
        }

        mug.gameObject.SetActive(false);
        
        clientHandTrigger.enabled = false;
        ReturnToExit();
    }

    private void ReturnToExit()
    {
        timeReturningStarted = Time.timeSinceLevelLoad;
        startReturnPosition = clientQueue.NearestReturnQueuePoint(transform.position);
        transform.rotation = clientQueue.returnQueueStartPoint.rotation;
    }

    private void ExitTavern()
    {
        if (CollectedMug)
            ReturnMug();
        
        OnClientExitedBar?.Invoke(this);
    }

    private void ReturnMug()
    {
        clientQueue.ReturnMug(this);
        CollectedMug.StartSliding();
        CollectedMug = null;
    }
}
