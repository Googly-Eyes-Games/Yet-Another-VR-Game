using System;
using UnityEngine;

public class KegComponent : MonoBehaviour
{
    [SerializeField]
    private Transform tapSocket;
    
    [SerializeField]
    private float tapRaycastLength = 0.5f;
    
    [SerializeField]
    private float sphereCastRadius = 0.03f;
    
    [SerializeField]
    [Tooltip("MugsPerSecond")]
    private float tippingBaseSpeed = 0.2f;

    [SerializeField]
    private float neckRadius = 0.02f;

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

        LayerMask mugLayerMask = LayerMask.GetMask("Mug");
        bool raycastHit = Physics.SphereCast(
            tapSocket.position,
            sphereCastRadius,
            tapSocket.forward,
            out RaycastHit hit,
            tapRaycastLength,
            mugLayerMask
            );
        
        if (!raycastHit || !hit.collider.CompareTag("Mug"))
            return;
        
        Debug.DrawLine(tapSocket.position, hit.point, Color.green);

        MugComponent mug = hit.transform.GetComponent<MugComponent>();
        mug.FillPercentage += currentTippingSpeed * Time.deltaTime;
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
