using NaughtyAttributes;
using UnityEngine;

[ExecuteAlways]
public class LiquidHandler : MonoBehaviour
{
    [field: Foldout("General")]
    [field: SerializeField]
    [field: Range(0, 1)]
    public float FillAmount { get; set; } = 0.5f;
    
    [field: Foldout("General")]
    [field: SerializeField]
    public Color LiquidColor { get; set; }
    
    [field: Foldout("General")]
    [field: SerializeField]
    public Color FoamColor { get; set; }

    [Foldout("General")]
    [SerializeField]
    private Vector3 bottomOffset = Vector3.zero;
    
    [Foldout("Wobbly Effect")]
    [SerializeField]
    private float maxWobblyEffect = 45f;
    
    [Foldout("Wobbly Effect")]
    [SerializeField]
    private float wobblyVelocityFactor = 1f;
    
    [Foldout("Wobbly Effect")]
    [SerializeField]
    private float wobblyAngularVelocityFactor = 1f;
    
    [Foldout("Wobbly Effect")]
    [SerializeField]
    private float wobbleDamping = 1f;
    
    [Foldout("Wobbly Effect")]
    [SerializeField]
    private float liquidDensity = 1f;

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Material material;

    private Vector2 lastPosition;
    private Quaternion lastRotation;
    private Vector2 velocity;
    private Vector2 angularVelocity;

    private Vector2 wobbleVelocity;
    private Vector2 wobble;
    
    private void OnEnable()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = Application.isPlaying ? meshRenderer.material : meshRenderer.sharedMaterial;
        meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        CalculateVelocities();
        
        CalculatePlanePosition();
        
        CalculatePlaneNormal();

        UpdateColors();
    }

    private void UpdateColors()
    {
        material.SetColor(ShaderLookUp.BaseColor, LiquidColor);

        Color foamColor = Color.Lerp(LiquidColor, FoamColor, (FillAmount + 0.5f) * 0.5f);
        material.SetColor(ShaderLookUp.FoamColor, foamColor);
    }

    private void CalculateVelocities()
    {
        float deltaTime = Time.deltaTime;
        if (deltaTime > Mathf.Epsilon)
        {
            Vector2 deltaPosition = lastPosition - transform.position.ToXZPlane();
            velocity = deltaPosition / deltaTime;

            angularVelocity = MathUtils.CalculateAngularVelocity(lastRotation, transform.rotation).ToXZPlane();
        }

        lastPosition = transform.position.ToXZPlane();
        lastRotation = transform.rotation;
    }


    private void CalculatePlanePosition()
    {
        Vector3 volumeBottom = meshRenderer.bounds.center;
        volumeBottom.y = meshRenderer.bounds.min.y;

        volumeBottom += bottomOffset;

        float volumeHeight = meshRenderer.bounds.max.y - meshRenderer.bounds.min.y;
        
        Vector3 planePosition = volumeBottom + Vector3.up * (FillAmount * volumeHeight);
        Debug.DrawLine(volumeBottom,planePosition, Color.green);
        
        material.SetVector(ShaderLookUp.PlanePosition, planePosition);
    }
    
    private void CalculatePlaneNormal()
    {
        if (FillAmount > Mathf.Epsilon)
        {
            float deltaTime = Time.deltaTime;
            if (deltaTime < Mathf.Epsilon)
                return;

            wobbleVelocity = Vector2.Lerp(wobbleVelocity, Vector2.zero, deltaTime * wobbleDamping);
            wobbleVelocity += velocity * wobblyVelocityFactor + angularVelocity * wobblyAngularVelocityFactor;

            wobble = wobbleVelocity * Mathf.Sin(Time.time * liquidDensity);
            wobble.x = Mathf.Clamp(wobble.x, -maxWobblyEffect, maxWobblyEffect);
            wobble.y = Mathf.Clamp(wobble.y, -maxWobblyEffect, maxWobblyEffect);

            Quaternion wobblyQuat = Quaternion.Euler(new Vector3(-wobble.y, 0, wobble.x));

            material.SetVector(ShaderLookUp.PlaneNormal, wobblyQuat * Vector3.up);
        }
        else
        {
            material.SetVector(ShaderLookUp.PlaneNormal, Vector3.up);
        }
    }

    private static class ShaderLookUp
    {
        public static readonly int PlanePosition = Shader.PropertyToID("_PlanePosition");
        public static readonly int PlaneNormal = Shader.PropertyToID("_PlaneNormal");
        
        public static readonly int BaseColor = Shader.PropertyToID("_Color");
        public static readonly int FoamColor = Shader.PropertyToID("_FoamColor");
    }
}