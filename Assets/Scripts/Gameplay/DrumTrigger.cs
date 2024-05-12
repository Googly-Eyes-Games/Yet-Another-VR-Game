using System;
using UnityEngine;

public class DrumTrigger : MonoBehaviour
{
    [SerializeField]
    private float maxVelocityDirectionsOffset = 40f;
    
    [SerializeField]
    private float minimalStickVelocity = 0f;
    
    public event Action OnTrigger;
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
        
        
        if (other.gameObject.layer == LayerMask.GetMask("Stick"))
        {
            OnTrigger?.Invoke();
        }
    }
}
