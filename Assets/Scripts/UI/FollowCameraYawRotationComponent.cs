using UnityEngine;

public class FollowCameraYawRotationComponent : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private float lerpSpeed = 2f;

    void Update()
    {
        Vector3 targetRotation = transform.rotation.eulerAngles;
        targetRotation.y = cameraTransform.rotation.eulerAngles.y;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(targetRotation),
            Time.unscaledDeltaTime * lerpSpeed
        );
    }
}
