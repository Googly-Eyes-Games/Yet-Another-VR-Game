using NaughtyAttributes;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    [field: SerializeField]
    public float currentClientSpeed { get; private set; } = 1f;
    
    [field: SerializeField]
    public float clientReturnSpeed { get; private set; } = 3f;
    
    [field: SerializeField]
    public float clientReturnMugSpeed { get; private set; } = 2f;

    [SerializeField]
    private GameObject leftClientPrefab;
    
    [SerializeField]
    private GameObject rightClientPrefab;
    
    private ClientQueue[] clientQueues;

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void SpawnClient()
    {
        ClientQueue targetQueue = clientQueues.GetRandom();
        
        if (targetQueue.isRightQueue)
            targetQueue.SpawnClient(this, rightClientPrefab);
        else
            targetQueue.SpawnClient(this, leftClientPrefab);
    }

    private void Awake()
    {
        clientQueues = FindObjectsByType<ClientQueue>(FindObjectsSortMode.None);
    }
}
