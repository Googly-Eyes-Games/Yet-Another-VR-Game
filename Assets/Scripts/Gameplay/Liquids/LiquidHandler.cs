using UnityEngine;

public class LiquidHandler : MonoBehaviour
{
    [field: SerializeField]
    [field: Range(0, 1)]
    public float FillAmount { get; set; } = 0.5f;
    
    [SerializeField]
    private float maxWobblyEffect = 45f;
    
    [SerializeField]
    private float wobblyVelocityFactor = 1f;
    
    [SerializeField]
    private float wobblyAngularVelocityFactor = 1f;
    
    [SerializeField]
    private float wobbleDamping = 1f;

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Material material;

    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 velocity;
    private Vector3 angularVelocity;
    
    private void OnEnable()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;

        meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        CalculateVelocities();
        
        CalculatePlanePosition();
        CalculatePlaneNormal();
    }

    private void CalculateVelocities()
    {
        float deltaTime = Time.deltaTime;
        if (deltaTime > Mathf.Epsilon)
        {
            Vector3 deltaPosition = lastPosition - transform.position;
            velocity = deltaPosition / deltaTime;

            angularVelocity = MathUtils.CalculateAngularVelocity(lastRotation, transform.rotation);
        }

        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }


    private void CalculatePlanePosition()
    {
        Vector3 volumeBottom = meshRenderer.bounds.center;
        volumeBottom.y = meshRenderer.bounds.min.y;

        float volumeHeight = meshRenderer.bounds.max.y - meshRenderer.bounds.min.y;
        
        Vector3 planePosition = volumeBottom + Vector3.up * (FillAmount * volumeHeight);
        Debug.DrawLine(volumeBottom,planePosition, Color.green);
        
        material.SetVector(ShaderLookUp.PlanePosition, planePosition);
    }
    
    private void CalculatePlaneNormal()
    {
        float wobbleX = velocity.x * wobblyVelocityFactor + angularVelocity.z * wobblyAngularVelocityFactor;
        wobbleX = Mathf.Clamp(wobbleX, -maxWobblyEffect, +maxWobblyEffect);
        
        float wobbleZ = -velocity.z * wobblyVelocityFactor + angularVelocity.x * wobblyAngularVelocityFactor;
        wobbleZ = Mathf.Clamp(wobbleZ, -maxWobblyEffect, +maxWobblyEffect);
        Quaternion wobblyQuat = Quaternion.Euler(new Vector3(wobbleZ, 0, wobbleX));
        
        material.SetVector(ShaderLookUp.PlaneNormal, wobblyQuat * Vector3.up);
    }

    private static class ShaderLookUp
    {
        public static readonly int PlanePosition = Shader.PropertyToID("_PlanePosition");
        public static readonly int PlaneNormal = Shader.PropertyToID("_PlaneNormal");
    }
}