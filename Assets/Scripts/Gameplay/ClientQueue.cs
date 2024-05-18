using UnityEngine;

public class ClientQueue : MonoBehaviour
{
    [field: SerializeField]
    public Transform startPoint { get; private set; }
    
    [field: SerializeField]
    public Transform endPoint { get; private set; }
    
    [field: SerializeField]
    public Transform returnQueueStartPoint { get; private set; }
    
    [field: SerializeField]
    public Transform returnQueueEndPoint { get; private set; }
    
    [field: SerializeField]
    public Transform returnMugStartPoint { get; private set; }
    
    [field: SerializeField]
    public Transform returnMugEndPoint { get; private set; }

    public float length => Vector3.Distance(startPoint.position, endPoint.position);

    public void SpawnClient(ClientManager clientManager, GameObject clientPrefab)
    {
        GameObject newClient = Instantiate(clientPrefab, startPoint.position, startPoint.rotation);
        Client client = newClient.GetComponent<Client>();
        client.Initialize(clientManager, this);
    }

    public void ReturnMug(Client client)
    {
        Vector3 mugPosition = MathUtils.NearestPointOnSegment(
            returnMugStartPoint.position,
            returnMugEndPoint.position,
            client.transform.position
            );

        // TODO: Probably we need to use object pooling
        MugComponent returnedMug = client.collectedMug;
        returnedMug.gameObject.SetActive(true);
        returnedMug.fillPercentage = 0f;
        
        // TODO: Remove Hardcoded variables
        Rigidbody mugRigidbody = returnedMug.GetComponent<Rigidbody>();
        mugRigidbody.position = mugPosition;
        mugRigidbody.velocity = client.mugReturnSpeed * returnMugStartPoint.forward;
        mugRigidbody.angularVelocity = Vector3.up * 45f;
    }

    public Vector3 NearestReturnQueuePoint(Vector3 position)
    {
        return MathUtils.NearestPointOnSegment(returnQueueStartPoint.position, returnQueueEndPoint.position, position);
    }
}
