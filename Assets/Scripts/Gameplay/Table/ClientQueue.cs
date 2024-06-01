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
    
    [field: SerializeField]
    public bool isRightQueue { get; private set; }

    public float length => Vector3.Distance(startPoint.position, endPoint.position);

    public void InitializeClient(Client client)
    {
        client.transform.position = startPoint.position;
        client.transform.rotation = startPoint.rotation;
        
        client.Initialize( this);
    }

    public void ReturnMug(Client client)
    {
        Vector3 mugPosition = MathUtils.NearestPointOnSegment(
            returnMugStartPoint.position,
            returnMugEndPoint.position,
            client.transform.position
            );

        MugComponent returnedMug = client.CollectedMug;
        returnedMug.gameObject.SetActive(true);
        returnedMug.FillPercentage = 0f;
        
        // TODO: Remove Hardcoded variables
        Rigidbody mugRigidbody = returnedMug.GetComponent<Rigidbody>();
        mugRigidbody.position = mugPosition;
        mugRigidbody.velocity = client.MugReturnSpeed * returnMugStartPoint.forward;
        mugRigidbody.angularVelocity = Vector3.up * 45f;
    }

    public Vector3 NearestReturnQueuePoint(Vector3 position)
    {
        return MathUtils.NearestPointOnSegment(returnQueueStartPoint.position, returnQueueEndPoint.position, position);
    }
}
