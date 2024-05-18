using System;
using UnityEngine;

public class ClientHandTrigger : MonoBehaviour
{
    public event Action<MugComponent> OnMugCollected;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mug"))
        {
            OnMugCollected?.Invoke(other.GetComponent<MugComponent>());
        }
    }
}