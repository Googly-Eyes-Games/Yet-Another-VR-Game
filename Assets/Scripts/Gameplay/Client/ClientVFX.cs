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
    
    [Foldout("Config")]
    [SerializeField]
    private FeedbackVFXSettings satisfiedClient;
    
    [Foldout("Config")]
    [SerializeField]
    private FeedbackVFXSettings unSatisfiedClient;

    private Client client;

    private void Awake()
    {
        client = GetComponent<Client>();
        client.OnClientStateChanged += HandleClientStateChanged;
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
    }
}
