using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MugComponent : MonoBehaviour
{
    public event Action<MugComponent> OnDestroy; 
    public event Action<bool> OnSlidingStateChanged;
    
    [SerializeField]
    private MeshRenderer beerMeshRenderer;
    private Material beerMaterialInstance;
    
    public bool IsSliding { get; private set; }
    
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

    public void Awake()
    {
        XRGrabInteractable interactable = GetComponent<XRGrabInteractable>();
        interactable.selectEntered.AddListener(HandleMugGrabbed);
        
        beerMaterialInstance = beerMeshRenderer.material;
        UpdateBeerMaterial();
    }

    private void HandleMugGrabbed(SelectEnterEventArgs selectArgs)
    {
        StopSliding();
    }

    public void DestroyMug()
    {
        OnDestroy?.Invoke(this);
    }
    
    public void StartSliding()
    {
        IsSliding = true;
        OnSlidingStateChanged?.Invoke(true);
    }
    
    public void StopSliding()
    {
        IsSliding = true;
        OnSlidingStateChanged?.Invoke(false);
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
