using System;
using erulathra;
using UnityEngine;

public class BrokenMugsDetector : MonoBehaviour
{
    public event Action OnMugBroken;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Mug"))
            return;
        
        MugComponent mugComponent = other.GetComponent<MugComponent>();

        mugComponent.DestroyMug();
        OnMugBroken?.Invoke();

        HeartsSubsystem heartsSubsystem = SceneSubsystemManager.GetSubsystem<HeartsSubsystem>();
        heartsSubsystem.HandleBrokenMug();
    }
}