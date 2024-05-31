using UnityEngine;

public class MugComponent : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer beerMeshRenderer;
    
    private Material beerMaterialInstance;
    
    private float fillPercentageField = 0f;
    
    public float FillPercentage
    {
        get => fillPercentageField;
        set
        {
            fillPercentageField = Mathf.Clamp01(value);
            UpdateBeerMaterial();
        }
    }

    public bool Destroyed { get; private set; }

    public void Awake()
    {
        Destroyed = false;
        
        beerMaterialInstance = beerMeshRenderer.material;
        UpdateBeerMaterial();
    }

    public void DestroyMug()
    {
        // TODO: object pooling
        Destroyed = true;
        Destroy(gameObject);
    }

    private void UpdateBeerMaterial()
    {
       beerMaterialInstance.SetFloat(ShaderPropertyLookUp.fill, FillPercentage);
    }
    
    readonly struct ShaderPropertyLookUp
    {
        public static readonly int fill = Shader.PropertyToID("_Fill");
    }
}
