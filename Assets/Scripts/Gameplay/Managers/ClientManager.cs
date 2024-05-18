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
    private GameObject clientPrefab;
    
    private ClientQueue[] clientQueues;

    public void SpawnClient()
    {
        ClientQueue targetQueue = clientQueues.GetRandom();
        targetQueue.SpawnClient(this, clientPrefab);
    }

    private void Awake()
    {
        clientQueues = FindObjectsByType<ClientQueue>(FindObjectsSortMode.None);
    }
}
