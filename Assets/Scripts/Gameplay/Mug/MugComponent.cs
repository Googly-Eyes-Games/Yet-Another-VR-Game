using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MugComponent : MonoBehaviour
{
    public event Action<MugComponent> OnDestroy; 
    public event Action<bool> OnSlidingStateChanged;
    public event Action OnPickedUpByClient;
    
    [SerializeField]
    private MeshRenderer beerMeshRenderer;
    
    public bool IsSliding { get; private set; }
    
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
        // TODO: object pooling
        OnDestroy?.Invoke(this);
        Destroy(gameObject);
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

    public void Collect(Client client)
    {
        OnPickedUpByClient?.Invoke();
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
