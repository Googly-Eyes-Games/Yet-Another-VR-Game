using System;
using UnityEngine;

public class BrokenMugsDetector : MonoBehaviour
{
    public event Action OnMugBroken;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Mug"))
            return;
        
        MugComponent mugComponent = other.GetComponent<MugComponent>();

        if (!mugComponent.Destroyed)
        {
            mugComponent.DestroyMug();
            OnMugBroken?.Invoke();
        }
    }
}