using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class LiquidHandler : MonoBehaviour
{
    [field: Foldout("General")]
    [field: SerializeField]
    [field: Range(0, 1)]
    public float FillAmount { get; set; } = 0.5f;

    [field: Foldout("General")]
    [field: SerializeField]
    public float ContainerVolume { get; set; } = 1f;
    
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

    public float LiquidVolume
    {
        get => ContainerVolume * FillAmount;
        set => FillAmount = value / ContainerVolume;
    }
    
    public Vector3 SurfacePosition { get; private set; }
    public Vector3 SurfaceNormal { get; private set; }

    public MeshRenderer MeshRenderer { get; private set; }
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
        MeshRenderer = GetComponent<MeshRenderer>();
        material = Application.isPlaying ? MeshRenderer.material : MeshRenderer.sharedMaterial;
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
        Vector3 volumeBottom = MeshRenderer.bounds.center;
        volumeBottom.y = MeshRenderer.bounds.min.y;

        volumeBottom += bottomOffset;

        float volumeHeight = MeshRenderer.bounds.max.y - MeshRenderer.bounds.min.y;
        
        SurfacePosition = volumeBottom + Vector3.up * (FillAmount * volumeHeight);
        Debug.DrawLine(volumeBottom,SurfacePosition, Color.green);
        
        material.SetVector(ShaderLookUp.PlanePosition, SurfacePosition);
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

            SurfaceNormal = wobblyQuat * Vector3.up;
            SurfaceNormal = SurfaceNormal.normalized;
            
            material.SetVector(ShaderLookUp.PlaneNormal, SurfaceNormal);
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