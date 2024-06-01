using UnityEngine;

public class StopSlidingTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mug"))
        {
            MugComponent mug = other.GetComponent<MugComponent>();
            mug.StopSliding();
        }
    }
}
