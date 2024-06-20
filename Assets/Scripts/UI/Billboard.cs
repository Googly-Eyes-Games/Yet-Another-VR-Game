using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        Vector3 toCamera = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(toCamera, Vector3.up);
    }
}
