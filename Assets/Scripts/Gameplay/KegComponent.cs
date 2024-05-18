using System;
using UnityEngine;

public class KegComponent : MonoBehaviour
{
    [SerializeField]
    private Transform tapSocket;
    
    [SerializeField]
    private float tapRaycastLength = 0.25f;
    
    [SerializeField]
    [Tooltip("MugsPerSecond")]
    private float tippingBaseSpeed = 0.2f;

    private float currentTippingSpeed = 0f;

    private void OnDrawGizmos()
    {
        if (!tapSocket)
            return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(tapSocket.position, tapSocket.forward * tapRaycastLength);
    }

    private void Update()
    {
        if (currentTippingSpeed < Single.Epsilon)
            return;

        bool raycastHit = Physics.Raycast(
            tapSocket.position,
            tapSocket.forward * tapRaycastLength,
            out RaycastHit hit);

        if (!raycastHit || !hit.collider.CompareTag("Mug"))
            return;

        MugComponent mug = hit.transform.GetComponent<MugComponent>();
        mug.fillPercentage += currentTippingSpeed * Time.deltaTime;
    }

    public void SetTippingSpeed(float newValue)
    {
        currentTippingSpeed = tippingBaseSpeed * newValue;
    }
}
