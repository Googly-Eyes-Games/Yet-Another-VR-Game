using UnityEngine;

public class FollowCameraRotationComponent : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private float lerpSpeed = 5f;

    void Update()
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            cameraTransform.rotation,
            Time.deltaTime * lerpSpeed);

        transform.position = cameraTransform.position;
    }
}
