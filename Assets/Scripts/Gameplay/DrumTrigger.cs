using System;
using UnityEngine;

public class DrumTrigger : MonoBehaviour
{
    [SerializeField]
    private float maxVelocityDirectionsOffset = 30f;
    
    [SerializeField]
    private float minimalStickSpeed = 5f;
    
    public event Action OnTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Stick"))
            return;
        
        StickComponent stick = other.GetComponent<StickComponent>();
        if (!stick)
            return;
        
        float upToOtherVelocityAngle = Vector3.Angle(stick.Velocity, -transform.up);
        float stickSpeed = stick.Velocity.magnitude;
                
        Debug.Log($"Speed: {stickSpeed}, Angle: {upToOtherVelocityAngle}");

        if (upToOtherVelocityAngle < maxVelocityDirectionsOffset
            && stickSpeed > minimalStickSpeed)
        {
            Debug.Log("boom");
            OnTrigger?.Invoke();
        }
        else
        {
            Debug.Log($"FALSE: Speed: {stickSpeed > minimalStickSpeed}, Angle: {upToOtherVelocityAngle < maxVelocityDirectionsOffset}");
        }
    }
}
