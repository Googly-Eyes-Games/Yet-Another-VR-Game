using System;
using Unity.VisualScripting;
using UnityEngine;

public enum ClientState
{
    WantsBeer,
    DrinkingBeer,
    MugReturned
}

public class Client : MonoBehaviour
{
    [SerializeField]
    private ClientHandTrigger clientHandTrigger;
    
    public float walkSpeed { get; private set; }
    public float returnSpeed { get; private set; }
    public float mugReturnSpeed { get; private set; }
    
    private float timeSpawned;
    private float timeBeerCollected;
    private Vector3 startReturnPosition;

    private ClientQueue clientQueue;
    public MugComponent collectedMug { get; private set; }

    private ClientState clientState = ClientState.WantsBeer;

    public void Initialize(ClientSubsystem subsystem, ClientQueue queue)
    {
        clientQueue = queue;
        timeSpawned = Time.timeSinceLevelLoad;
        walkSpeed = subsystem.CurrentClientSpeed;
        returnSpeed = GameplaySettings.Global.ClientReturnSpeed;
        mugReturnSpeed = GameplaySettings.Global.ClientReturnMugSpeed;
    }

    public void Awake()
    {
        clientHandTrigger.OnMugCollected += OnMugCollected;
    }

    private void Update()
    {
        if (clientState == ClientState.WantsBeer)
        {
            HandleGotToBarLogic();
        }
        else if (clientState == ClientState.DrinkingBeer)
        {
            HandleReturnLogic();
        }
    }

    private void HandleGotToBarLogic()
    {
        float queueLength = clientQueue.length;

        float elapsedTime = Time.timeSinceLevelLoad - timeSpawned;
        float queueFract = elapsedTime * walkSpeed / queueLength;

        transform.position = Vector3.Lerp(
            clientQueue.startPoint.position,
            clientQueue.endPoint.position,
            queueFract
        );
    }

    private void HandleReturnLogic()
    {
        float queueLength = Vector3.Distance(startReturnPosition, clientQueue.returnQueueEndPoint.position);

        float elapsedTime = Time.timeSinceLevelLoad - timeBeerCollected;
        float queueFract = elapsedTime * returnSpeed / queueLength;

        transform.position = Vector3.Lerp(
            startReturnPosition,
            clientQueue.returnQueueEndPoint.position,
            queueFract
        );

        if (queueFract >= 1f - Single.Epsilon)
        {
            ReturnMug();
        }
    }

    private void OnMugCollected(MugComponent mug)
    {
        // TODO: HARDCODED VALUE
        if (mug.FillPercentage > 0.6f)
        {
            collectedMug = mug;
            collectedMug.gameObject.SetActive(false);
            clientState = ClientState.DrinkingBeer;
            timeBeerCollected = Time.timeSinceLevelLoad;
            startReturnPosition = clientQueue.NearestReturnQueuePoint(transform.position);
            transform.rotation = clientQueue.returnQueueStartPoint.rotation;
        }
    }

    private void ReturnMug()
    {
        clientState = ClientState.MugReturned;
        clientQueue.ReturnMug(this);
        collectedMug = null;
        Destroy(gameObject);
    }
}
