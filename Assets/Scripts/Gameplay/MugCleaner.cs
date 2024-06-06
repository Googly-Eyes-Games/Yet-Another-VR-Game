using UnityEngine;

public class MugCleaner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mug"))
        {
            MugComponent mug = other.GetComponent<MugComponent>();
            mug.CleanMug();
        }
    }
}
