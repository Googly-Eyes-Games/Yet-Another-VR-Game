using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MugComponent : MonoBehaviour
{
    public event Action<MugComponent> OnDestroy; 
    public event Action<MugComponent> OnClean; 
    public event Action<bool> OnSlidingStateChanged;

    [SerializeField]
    private LiquidHandler beerLiquidHandler;
    
    [SerializeField]
    private MeshRenderer mugMeshRenderer;
    
    [SerializeField]
    private MeshRenderer foamRenderer;
    
    private Material mugMaterialInstance;
    
    public bool IsSliding { get; private set; }

    private bool isClean = true;
    public bool IsClean
    {
        get => isClean;
        set
        {
            isClean = value;
            UpdateMugMaterial();
            UpdateFoamVisibility();
        }
    }

    public float FillPercentage
    {
        get => beerLiquidHandler.FillAmount;
        set
        {
            beerLiquidHandler.FillAmount = Mathf.Clamp01(value);
            UpdateBeerMaterial();
        }
    }

    public void Awake()
    {
        XRGrabInteractable interactable = GetComponent<XRGrabInteractable>();
        interactable.selectEntered.AddListener(HandleMugGrabbed);
        
        mugMaterialInstance = mugMeshRenderer.material;
        
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
    
    public void CleanMug()
    {
        OnClean?.Invoke(this);
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
        beerLiquidHandler.FillAmount = FillPercentage;
    }

    private void UpdateMugMaterial()
    {
        mugMaterialInstance.SetFloat(ShaderPropertyLookUp.isClean, isClean ? 1f : 0f);
    }
    
    private void UpdateFoamVisibility()
    {
        foamRenderer.enabled = !isClean;
    }

    
    readonly struct ShaderPropertyLookUp
    {
        public static readonly int isClean = Shader.PropertyToID("_IsClean");
    }
}
