using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.VFX;

public class ClientVFX : MonoBehaviour
{
    [Foldout("Setup")]
    [SerializeField]
    private VisualEffect feedbackVFX;
    
    [Foldout("Setup")]
    [SerializeField]
    private SkinnedMeshRenderer meshRenderer;
    
    [Foldout("Setup")]
    [SerializeField]
    private ClientAnimator animator;
    
    [Foldout("Config")]
    [SerializeField]
    private FeedbackVFXSettings satisfiedClient;
    
    [Foldout("Config")]
    [SerializeField]
    private FeedbackVFXSettings unSatisfiedClient;

    [Foldout("Config")]
    [SerializeField]
    private VikingColorSetup[] vikingColors;
    
    [Foldout("Config")]
    [SerializeField]
    private VikingMeshSetup[] vikingMeshes;
    
    [Foldout("Debug")]
    [SerializeField]
    private int colorToApply;
    
    [Foldout("Debug")]
    [SerializeField]
    private int meshToApply;

    private Client client;
    private Material material;

    private void Awake()
    {
        client = GetComponent<Client>();
        client.OnClientStateChanged += HandleClientStateChanged;
        client.OnInitialize += HandleInitialization;

        material = meshRenderer.material;
    }

    private void HandleInitialization()
    {
        UpdateColors();
        UpdateMesh();
    }

    [Button]
    private void UpdateMesh()
    {
        VikingMeshSetup setup = vikingMeshes.GetRandom();
        // VikingMeshSetup setup = vikingMeshes[2];

        meshRenderer.sharedMesh = setup.mesh;
        animator.IsWoman = setup.isWoman;
    }

    [Button]
    private void UpdateColors()
    {
        VikingColorSetup colorSetup;
        if (Application.isPlaying)
        {
            colorSetup = vikingColors.GetRandom();
        }
        else
        {
            int indexToApply = Mathf.Clamp(colorToApply, 0, vikingColors.Length);
            colorSetup = vikingColors[indexToApply];
            
            material = meshRenderer.sharedMaterial;
        }
        
        material.SetColor(ShaderLookup.HairColor, colorSetup.hairColor);
        material.SetColor(ShaderLookup.ClothPrimaryColor, colorSetup.clothPrimaryColor);
        material.SetColor(ShaderLookup.ClothSecondaryColor, colorSetup.clothSecondaryColor);
    }

    private void HandleClientStateChanged(ClientState newClientState)
    {
        if (newClientState == ClientState.DrinkingBeer)
        {
            satisfiedClient.Apply(feedbackVFX);
            feedbackVFX.Play();
        }
        else if (newClientState == ClientState.Unsatisfied)
        {
            unSatisfiedClient.Apply(feedbackVFX);
            feedbackVFX.Play();
        }
    }

    [Serializable]
    struct FeedbackVFXSettings
    {
        [SerializeField]
        public Texture2D spriteTexture;
        
        [SerializeField]
        public Color spriteColor;
        
        [SerializeField]
        public Vector2Int flipBookSize;

        public void Apply(VisualEffect feedbackVFX)
        {
            feedbackVFX.SetTexture(ShaderLookup.ParticleSprite, spriteTexture);
            feedbackVFX.SetVector4(ShaderLookup.Color, spriteColor);
            feedbackVFX.SetVector2(ShaderLookup.FlipBookSize, flipBookSize);
        }
    }

    private static class ShaderLookup
    {
        public static readonly int ParticleSprite = Shader.PropertyToID("Particle");
        public static readonly int Color = Shader.PropertyToID("Color");
        public static readonly int FlipBookSize = Shader.PropertyToID("FlipBookSize");
        
        public static readonly int HairColor = Shader.PropertyToID("_Hair");
        public static readonly int ClothPrimaryColor = Shader.PropertyToID("_ClothPrimary");
        public static readonly int ClothSecondaryColor = Shader.PropertyToID("_ClothSecondary");
    }

    [Serializable]
    public class VikingColorSetup
    {
        [SerializeField]
        public Color hairColor;
        
        [SerializeField]
        public Color clothPrimaryColor;
        
        [SerializeField]
        public Color clothSecondaryColor;
    }
    
    [Serializable]
    public class VikingMeshSetup
    {
        [SerializeField]
        public Mesh mesh;

        [SerializeField]
        public bool isWoman;
    }
}
