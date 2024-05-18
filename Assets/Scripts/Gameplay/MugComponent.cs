using UnityEngine;

public class MugComponent : MonoBehaviour
{
    [field: SerializeField]
    public GameObject prefab { get; private set; }
    
    [SerializeField]
    private MeshRenderer beerMeshRenderer;
    
    private Material beerMaterialInstance;
    
    private float fillPercentageField = 0f;
    public float fillPercentage
    {
        get => fillPercentageField;
        set
        {
            fillPercentageField = Mathf.Clamp01(value);
            UpdateBeerMaterial();
        }
    }

    public void Awake()
    {
        beerMaterialInstance = beerMeshRenderer.material;
        UpdateBeerMaterial();
    }

    private void UpdateBeerMaterial()
    {
       beerMaterialInstance.SetFloat(ShaderPropertyLookUp.fill, fillPercentage);
    }
    
    readonly struct ShaderPropertyLookUp
    {
        public static readonly int fill = Shader.PropertyToID("_Fill");
    }
}
