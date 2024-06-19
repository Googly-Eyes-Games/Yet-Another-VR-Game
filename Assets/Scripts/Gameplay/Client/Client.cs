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
    public event Action OnInitialize;
    
    [SerializeField]
    private ClientHandTrigger clientHandTrigger;
    
    [field: SerializeField]
    public bool IsRightClient { get; private set; }
    
    public float WalkSpeed { get; private set; }
    public float ReturnSpeed { get; private set; }
    public float MugReturnSpeed { get; private set; }
    
    public ClientState State { get; private set; } = ClientState.WantsBeer;

    public float GoToBarProgress
    {
        get
        {
            float queueLength = ClientQueue.length;
            float elapsedTime = Time.timeSinceLevelLoad - timeSpawned;
            return elapsedTime * WalkSpeed / queueLength;
        }
    }
    
    private float timeSpawned;
    private float timeReturningStarted;
    private Vector3 startReturnPosition;

    public ClientQueue ClientQueue { get; private set; }
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
        
        ClientQueue = queue;
        timeSpawned = Time.timeSinceLevelLoad;
        ReturnSpeed = GameplaySettings.Global.ClientReturnSpeed;
        MugReturnSpeed = currentLevelConfig.MugSpeed;

        State = ClientState.WantsBeer;
        clientHandTrigger.enabled = true;
        
        OnInitialize?.Invoke();
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
        float queueFract = GoToBarProgress;

        transform.position = Vector3.Lerp(
            ClientQueue.startPoint.position,
            ClientQueue.endPoint.position,
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
        float queueLength = Vector3.Distance(startReturnPosition, ClientQueue.returnQueueEndPoint.position);

        float elapsedTime = Time.timeSinceLevelLoad - timeReturningStarted;
        float queueFract = elapsedTime * ReturnSpeed / queueLength;

        transform.position = Vector3.Lerp(
            startReturnPosition,
            ClientQueue.returnQueueEndPoint.position,
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
            
            Rigidbody mugRigidbody = mug.GetComponent<Rigidbody>();
            mugRigidbody.velocity = Vector3.zero;
            
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
        startReturnPosition = ClientQueue.NearestReturnQueuePoint(transform.position);
        transform.rotation = ClientQueue.returnQueueStartPoint.rotation;
    }

    private void ExitTavern()
    {
        if (CollectedMug)
            ReturnMug();
        
        OnClientExitedBar?.Invoke(this);
    }

    private void ReturnMug()
    {
        ClientQueue.ReturnMug(this);
        CollectedMug.StartSliding();
        CollectedMug = null;
    }
}
