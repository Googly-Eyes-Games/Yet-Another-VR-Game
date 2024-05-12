using UnityEngine;

public class DrumComponent : MonoBehaviour
{
    [SerializeField]
    private DrumTrigger drumTrigger;
    
    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        drumTrigger.OnTrigger += OnDrumTrigger;
    }

    private void OnDrumTrigger()
    {
        audioSource.Play();
    }
}
