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

    [SerializeField]
    private float neckRadius = 0.01f;

    [SerializeField]
    private ParticleSystem fluidParticleSystem;

    private float currentTippingSpeed = 0f;

    private void OnDrawGizmos()
    {
        if (!tapSocket)
            return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(tapSocket.position, tapSocket.forward * tapRaycastLength);
        
        Gizmos.DrawWireSphere(tapSocket.position, neckRadius);
    }

    private void Update()
    {
        if (currentTippingSpeed < Single.Epsilon)
            return;

        Ray tapRay = new Ray(tapSocket.position, tapSocket.forward * tapRaycastLength);
        bool raycastHit = Physics.SphereCast(
            tapRay,
            neckRadius,
            out RaycastHit hit);

        if (!raycastHit || !hit.collider.CompareTag("Mug"))
            return;

        MugComponent mug = hit.transform.GetComponent<MugComponent>();
        mug.fillPercentage += currentTippingSpeed * Time.deltaTime;
    }

    public void SetTippingSpeed(float newValue)
    {
        currentTippingSpeed = tippingBaseSpeed * newValue;

        if (!(newValue > Single.Epsilon))
            return;

        ParticleSystem.MainModule mainModule = fluidParticleSystem.main;
        mainModule.startSizeMultiplier = neckRadius * 25f * newValue;
        
        fluidParticleSystem.Play();
    }
}
