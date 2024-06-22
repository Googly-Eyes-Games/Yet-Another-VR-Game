using System;
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

    [field: SerializeField]
    public TableAxis tableAxis = TableAxis.Z;

    public float length => Vector3.Distance(startPoint.position, endPoint.position);

    public void InitializeClient(Client client)
    {
        client.transform.position = startPoint.position;
        client.transform.rotation = startPoint.rotation;
        
        client.Initialize( this);
    }

    public void ReturnMug(Client client)
    {
        MugComponent returnedMug = client.CollectedMug;
        returnedMug.gameObject.SetActive(true);
        returnedMug.FillPercentage = 0f;
        returnedMug.IsClean = false;
        returnedMug.StartSliding();
        
        Rigidbody mugRigidbody = returnedMug.GetComponent<Rigidbody>();
        mugRigidbody.MovePosition(returnMugStartPoint.position);
        mugRigidbody.rotation = Quaternion.identity;

        Vector3 mugReturnDirection = returnMugStartPoint.forward;
        mugRigidbody.velocity = client.MugReturnSpeed * mugReturnDirection;
        mugRigidbody.angularVelocity = Vector3.up * 45f;
        mugRigidbody.AddForce(-mugRigidbody.GetAccumulatedForce());
    }

    public Vector3 NearestReturnQueuePoint(Vector3 position)
    {
        return LinearAlgebra.NearestPointOnSegment(returnQueueStartPoint.position, returnQueueEndPoint.position, position);
    }
    
    public enum TableAxis
    {
        X,
        Z
    }
}
