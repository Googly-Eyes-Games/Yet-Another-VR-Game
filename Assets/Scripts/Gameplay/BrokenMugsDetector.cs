using System;
using UnityEngine;

public class BrokenMugsDetector : MonoBehaviour
{
    public event Action OnMugBroken;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mug"))
        {
            MugComponent mugComponent = other.GetComponent<MugComponent>();
            mugComponent.DestroyMug();
            
            OnMugBroken?.Invoke();
        }
    }
}