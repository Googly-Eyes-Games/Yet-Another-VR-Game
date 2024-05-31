using System;
using UnityEngine;

public class ClientHandTrigger : MonoBehaviour
{
    public event Action<MugComponent> OnMugCollected;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;
            
        if (other.CompareTag("Mug"))
        {
            Rigidbody mugRigidbody = other.GetComponent<Rigidbody>();
            
            if (Vector3.Dot(mugRigidbody.velocity, transform.forward) > 0f)
                OnMugCollected?.Invoke(other.GetComponent<MugComponent>());
        }
    }
}
