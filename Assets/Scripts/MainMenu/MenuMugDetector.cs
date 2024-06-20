using UnityEngine;
using UnityEngine.Events;

public class MenuMugDetector : MonoBehaviour
{
    public UnityEvent<MugComponent> onMugDetected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mug"))
        {
            onMugDetected?.Invoke(other.GetComponent<MugComponent>());
        }
    }
}
